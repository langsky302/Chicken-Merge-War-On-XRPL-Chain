using System;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UDEV.WaveManagerToolkit;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class GUIManager : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField] private Text m_coinCountingTxt;
        [SerializeField] private Text m_waveCountingTxt;
        [SerializeField] private ImageFilled m_waveProgress;
        [SerializeField] private Text m_waveCountingAnimTxt;

        [SerializeField] private SkillCollectedDrawer m_collectedSkillDrawer;
        [SerializeField] private ActiveBoosterDrawer m_activeBoosterDrawer;

        #region ACTION
        private Action<object> m_OnGunUnlocked;
        private Action<object> m_OnDrawSkillCollected;
        private Action<object> m_OnBuyingBooster;
        private Action<object> m_OnUpdateCoin;
        private Action<object> m_OnWaveTxtUpdate;
        private Action<object> m_OnWaveBegin;
        private Action<object> m_OnLevelCompleted;
        private Action<object> m_OnLevelFailed;
        private Action<object> m_OnEnemyDie;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnGunUnlocked = param => ShowGunUnlockedDialog((NodeItem)param);
            m_OnDrawSkillCollected = param => DrawCollectedSkillItems();
            m_OnBuyingBooster = param => DrawActiveBoosters();
            m_OnUpdateCoin = param => UpdateCoinCounting();
            m_OnWaveTxtUpdate = param => UpdateWaveCounting((WaveTK_WaveController)param);
            m_OnWaveBegin += param => UpdateWaveCountingAnim((WaveTK_WaveController)param);
            m_OnWaveBegin += param => UpdateWaveProgress(GameController.Ins.CurWave);
            m_OnLevelCompleted = param => ShowLevelCompletedDialog();
            m_OnLevelFailed = param => ShowLevelFailedDialog();
            m_OnEnemyDie = param => UpdateWaveProgress(GameController.Ins.CurWave);

            this.RegisterActionEvent(GameplayAction.GUN_UNLOCKED, m_OnGunUnlocked);
            this.RegisterActionEvent(GameplayAction.BUY_BOOSTER, m_OnBuyingBooster);
            this.RegisterActionEvent(GameplayAction.BUY_GUN, m_OnUpdateCoin);
            this.RegisterActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpdateCoin);
            this.RegisterActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE, m_OnUpdateCoin);
            this.RegisterActionEvent(GameplayAction.REWARD_DIALOG_CLOSED, m_OnUpdateCoin);
            this.RegisterActionEvent(GameplayAction.UPDATE_COIN, m_OnUpdateCoin);
            this.RegisterActionEvent(GameState.Starting, m_OnUpdateCoin);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpdateCoin);
            this.RegisterActionEvent(GameplayAction.REPAIR_SHIELD, m_OnUpdateCoin);
            this.RegisterActionEvent(GameState.Starting, m_OnDrawSkillCollected);
            this.RegisterActionEvent(GameState.Starting, m_OnBuyingBooster);
            this.RegisterActionEvent(GameState.Starting, m_OnWaveTxtUpdate);
            this.RegisterActionEvent(GameplayAction.WAVE_BEGIN, m_OnWaveBegin);
            this.RegisterActionEvent(GameplayAction.WAVE_BEGIN, m_OnWaveTxtUpdate);
            this.RegisterActionEvent(GameState.Completed, m_OnLevelCompleted);
            this.RegisterActionEvent(GameState.Gameover, m_OnLevelFailed);
            this.RegisterActionEvent(EnemyAction.DIE, m_OnEnemyDie);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.GUN_UNLOCKED, m_OnGunUnlocked);
            this.RemoveActionEvent(GameplayAction.BUY_BOOSTER, m_OnBuyingBooster);
            this.RemoveActionEvent(GameplayAction.BUY_GUN, m_OnUpdateCoin);
            this.RemoveActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpdateCoin);
            this.RemoveActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE, m_OnUpdateCoin);
            this.RemoveActionEvent(GameplayAction.REWARD_DIALOG_CLOSED, m_OnUpdateCoin);
            this.RemoveActionEvent(GameplayAction.UPDATE_COIN, m_OnUpdateCoin);
            this.RemoveActionEvent(GameState.Starting, m_OnUpdateCoin);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpdateCoin);
            this.RemoveActionEvent(GameplayAction.REPAIR_SHIELD, m_OnUpdateCoin);
            this.RemoveActionEvent(GameState.Starting, m_OnDrawSkillCollected);
            this.RemoveActionEvent(GameState.Starting, m_OnBuyingBooster);
            this.RemoveActionEvent(GameState.Starting, m_OnWaveTxtUpdate);
            this.RemoveActionEvent(GameplayAction.WAVE_PASSED, m_OnWaveBegin);
            this.RemoveActionEvent(GameplayAction.WAVE_PASSED, m_OnWaveTxtUpdate);
            this.RemoveActionEvent(GameState.Completed, m_OnLevelCompleted);
            this.RemoveActionEvent(GameState.Gameover, m_OnLevelFailed);
            this.RemoveActionEvent(EnemyAction.DIE, m_OnEnemyDie);
        }
        #endregion

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void DrawCollectedSkillItems()
        {
            m_collectedSkillDrawer?.Draw();
        }

        public void DrawActiveBoosters()
        {
            m_activeBoosterDrawer?.Draw();
        }

        private void UpdateCoinCounting()
        {
            if(m_coinCountingTxt)
                m_coinCountingTxt.text = Helper.BigCurrencyFormat(UserDataHandler.Ins.coin);
        }

        public void UpdateWaveCounting(WaveTK_WaveController wave)
        {
            if (wave == null || m_waveCountingTxt == null) return;
            int currentWave = wave.CurrentWaveIndex + 1;
            int totalWave = wave.waves.Count;
            if (m_waveCountingTxt)
                m_waveCountingTxt.text = $"WAVE {currentWave} - {totalWave}";

            TweenUltis.ScaleBlink(m_waveCountingTxt.transform, 1f, 1.2f, 0.2f);
        }

        private void UpdateWaveCountingAnim(WaveTK_WaveController wave)
        {
            if (m_waveCountingAnimTxt == null || wave == null) return;
            int currentWave = wave.CurrentWaveIndex + 1;
            int totalWave = wave.waves.Count;
            m_waveCountingAnimTxt.text = $"WAVE {currentWave} - {totalWave}";
            m_waveCountingAnimTxt.gameObject.SetActive(true);
            var rect = m_waveCountingAnimTxt.GetComponent<RectTransform>();

            var inPos = new Vector3(0f, 200f, 0f);
            TweenUltis.LineMoveInOut(rect, inPos, 0.5f, false, 
                () =>
                {
                    TweenUltis.ScaleBlink(m_waveCountingAnimTxt.transform, 1f, 1.25f, 0.3f);
                }, () =>
                {
                    m_waveCountingAnimTxt.gameObject.SetActive(false);
                }
            );
        }

        private void UpdateWaveProgress(WaveTK_WaveController wave)
        {
            if(wave == null) return;
            m_waveProgress?.UpdateValue(wave.WaveProgress);
        }

        private void ShowGunUnlockedDialog(NodeItem nodeItem)
        {
            if (nodeItem == null) return;

            var gunUnlockedDialog = (GunUnlockedDialog)DialogDB.Ins.GetDialog(DialogType.GunUnlocked);
            if (gunUnlockedDialog)
            {
                gunUnlockedDialog.UpdateUI(nodeItem.ChickenCtr.ChickenPreview, (nodeItem.Id + 1));
                DialogDB.Ins.Show(gunUnlockedDialog, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
            }
        }

        public void OpenChestDialog()
        {
            if (ChestController.Ins.CurrentChest == null || ChestController.Ins.ChestRemaining <= 0) return;

            OpenChestDialog openChestDialog = (OpenChestDialog)DialogDB.Ins.GetDialog(DialogType.OpenChest);
            if (openChestDialog)
            {
                DialogDB.Ins.Show(openChestDialog, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
            }
        }

        public void OpenBoosterDialog()
        {
            DialogDB.Ins.Show(DialogType.BoosterShop, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void ShowLevelCompletedDialog()
        {
            DialogDB.Ins.Show(DialogType.ClaimLevelBonus, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void ShowLevelFailedDialog()
        {
            DialogDB.Ins.Show(DialogType.LevelFailed, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }
    }
}
