using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class AutoSpawnGun : MonoBehaviour
    {
        private GunStatSO[] m_gunStats;
        private Dictionary<int, GunStatSO> m_gunUnlockeds;
        [SerializeField] private float m_spawnTime;
        private bool m_isActive;
        private float m_spawnTimeCounting;

        public UnityEvent<bool, Sprite> OnUpdateUI;
        public UnityEvent<float> OnCooldown;

        public float Progress { get => m_spawnTimeCounting / m_spawnTime; }

        #region ACTION
        private Action<object> m_OnInit;
        private Action<object> m_OnUpdateGunUnlockeds;
        private Action<object> m_OnStopSpawn;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnInit = param => Init();
            m_OnUpdateGunUnlockeds = param => UpdateGunUnlockeds((NodeItem)param);
            m_OnStopSpawn = param => CancelInvoke();

            this.RegisterActionEvent(GameplayAction.GUN_UNLOCKED, m_OnUpdateGunUnlockeds);
            this.RegisterActionEvent(GameState.Starting, m_OnInit);            
            this.RegisterActionEvent(GameState.Gameover, m_OnStopSpawn);
            this.RegisterActionEvent(GameState.Completed, m_OnStopSpawn);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.GUN_UNLOCKED, m_OnUpdateGunUnlockeds);
            this.RemoveActionEvent(GameState.Starting, m_OnInit);
            this.RemoveActionEvent(GameState.Gameover, m_OnStopSpawn);
            this.RemoveActionEvent(GameState.Completed, m_OnStopSpawn);
        }
        #endregion

        private void Init()
        {
            m_isActive = UserDataHandler.Ins.setting.isAutoSpawnGun;
            UpdateGunUnlockeds();
            m_spawnTimeCounting = m_spawnTime;
            OnUpdateUI?.Invoke(m_isActive, null); 
        }

        private void OnEnable()
        {
            RegisterEvents();            
        }

        private void OnDisable()
        {
            UnregisterEvents();
            CancelInvoke();
        }

        private void Update()
        {
            if (!m_isActive || m_spawnTimeCounting <= 0) return;
            m_spawnTimeCounting -= Time.deltaTime;
            if(m_spawnTimeCounting <= 0)
            {
                SpawnGun();
                m_spawnTimeCounting = m_spawnTime;
            }
            OnCooldown?.Invoke(Progress);
        }

        public void TurnOffAutoSpawn() {
            m_isActive = false;
        }


        public void AutoSpawnTrigger()
        {
            m_isActive = !m_isActive;
            m_spawnTimeCounting = m_spawnTime;
            UserDataHandler.Ins.setting.isAutoSpawnGun = m_isActive;
            OnUpdateUI?.Invoke(m_isActive, null);
        }

        private void SpawnGun()
        {
            
            if(BoardController.Ins.IsAllNodeFull() || BoardController.state != BoardState.None || !GameController.IsPlaying) return;

            var gunNeededId = GetGunNeededId();
            if(gunNeededId < 0) return;
            var gunStat = m_gunUnlockeds[gunNeededId];
            if(gunStat == null) return;
            UserDataHandler.Ins.coin -= gunStat.buyingPrice;
            gunStat.InscreaseBuyingPrice();
            gunStat.Save(gunNeededId);
            BoardController.Ins.PlaceItem(gunNeededId);

            this.PostActionEvent(GameplayAction.UPDATE_COIN);
            this.PostActionEvent(GameplayAction.AUTO_SPAWN_GUN_DONE, gunNeededId);

            OnUpdateUI?.Invoke(m_isActive, gunStat.thumb);
        }

        private int GetGunNeededId()
        {
            if (m_gunUnlockeds == null || m_gunUnlockeds.Count <= 0) return -1;
            var gunIds = m_gunUnlockeds.Keys.ToArray();
            if (gunIds == null || gunIds.Length <= 0) return -1;
            for (int i = gunIds.Length - 1; i >= 0; i--)
            {
                var gunId = gunIds[i];
                var gunStat = m_gunUnlockeds[gunId];
                if (gunStat == null) continue;
                if(IsCoinValid(gunStat.buyingPrice))
                    return gunId;
            }
            return -1;
        }

        private bool IsCoinValid(int coin)
        {
            return UserDataHandler.Ins.coin >= coin;
        }

        private void UpdateGunUnlockeds(NodeItem nodeItem)
        {
            if (UserDataHandler.Ins.IsGunUnlocked(nodeItem.Id)) return;

            Invoke("UpdateGunUnlockeds", 2f);
        }

        private void UpdateGunUnlockeds()
        {
            m_gunUnlockeds = new Dictionary<int, GunStatSO>();
            if (DataGroup.Ins == null || DataGroup.Ins.gunShopData == null) return;
            m_gunStats = DataGroup.Ins.gunShopData.gunStats;
            if(m_gunStats == null || m_gunStats.Length <= 0) return;
            for(int i = 0; i < m_gunStats.Length; i++)
            {
                var gunStat = m_gunStats[i];
                if(gunStat == null || !UserDataHandler.Ins.IsGunUnlocked(i)) continue;
                m_gunUnlockeds.Add(i, gunStat);
            }
        }
    }
}
