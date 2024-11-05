using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class Shield : MonoBehaviour, IDamageable, IActionEventDispatcher
    {
        [SerializeField] private ShieldStatSO m_shieldStat;
        private ShieldBonusReceiver m_bonusReceiver;

        private float m_durability;
        private int m_upgradePrice;
        private int m_repairPrice;

        public ShieldStatSO ShieldStat { get => m_shieldStat; }
        public float DurabilityRemainingRate { get => m_durability / m_shieldStat.durability; }
        public int RepairPrice { get => m_repairPrice;}
        public int UpgradePrice { get => m_upgradePrice;}

        #region ACTION
        private Action<object> m_OnUpgradeSkill;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnUpgradeSkill += (param) => UpdateShieldUpgradePrice();

            this.RegisterActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpgradeSkill);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpgradeSkill);
        }
        #endregion

        private void Awake()
        {
            m_bonusReceiver = GetComponent<ShieldBonusReceiver>();
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void Init()
        {
            LoadStats();
        }

        private void LoadStats()
        {
            if (m_shieldStat == null) return;
            m_shieldStat.Load();

            m_durability = m_shieldStat.durability;
            m_repairPrice = m_shieldStat.repairPrice;
            m_repairPrice += m_shieldStat.repairPriceUp * m_shieldStat.level;
            m_upgradePrice = m_shieldStat.upgradePrice;
        }

        public void Upgrade(UnityAction UpgradeSuccess = null, UnityAction UpgradeFailed = null)
        {
            if(m_shieldStat == null) return;

            if (UserDataHandler.Ins.IsEnoughCoin(m_upgradePrice))
            {
                UserDataHandler.Ins.coin -= m_upgradePrice;
                m_shieldStat.Upgrade(() =>
                {
                    LoadStats();
                    UpgradeSuccess?.Invoke();
                    this.PostActionEvent(GameplayAction.UPGRADE_SHEILD);
                });
                return;
            }
            else
            {
                UpgradeFailed?.Invoke();
            }
        }

        public void Repair(UnityAction RepairSuccess = null, UnityAction RepairFailed = null)
        {
            if (m_shieldStat == null || m_durability >= m_shieldStat.durability) return;
           
            if (UserDataHandler.Ins.IsEnoughCoin(m_repairPrice))
            {
                UserDataHandler.Ins.coin -= m_repairPrice;
                UserDataHandler.Ins?.SaveData();

                m_repairPrice += m_shieldStat.repairPriceUp * m_shieldStat.level;
                m_durability = m_shieldStat.durability;

                RepairSuccess?.Invoke();
                this.PostActionEvent(GameplayAction.REPAIR_SHIELD);
                return;
            }
            else
            {
                RepairFailed?.Invoke();
            }
        }

        public int GetShieldPrice(int originalPrice, PassiveSkillType affectedType)
        {
            float bonusFromSkill = DataGroup.Ins.GetSkillBonus(affectedType, originalPrice);
            int reducedPrice = Mathf.RoundToInt(bonusFromSkill);
            return originalPrice - reducedPrice;
        }

        private void UpdateShieldUpgradePrice()
        {
            m_upgradePrice = GetShieldPrice(m_upgradePrice, PassiveSkillType.REDUCE_UPGRADE_PRICE);
        }

        public void TakeDamage(float damage)
        {
            if (!GameController.IsPlaying) return;
            if (m_bonusReceiver != null && m_bonusReceiver.IsInvincibleShieldActived) return;

            m_durability -= damage;
            if(m_durability <= 0)
            {
                this.PostActionEvent(GameState.Gameover);
                return;
            }
            this.PostActionEvent(GameplayAction.SHIELD_HITTED);
        }
    }
}
