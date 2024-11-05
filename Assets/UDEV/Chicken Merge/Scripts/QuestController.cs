using System;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class QuestController : Singleton<QuestController>, IActionEventDispatcher
    {
        #region ACTION
        private Action<object> m_OnLevelUnlocked;
        private Action<object> m_OnGunUnlocked;
        private Action<object> m_OnUseBooster;
        private Action<object> m_OnEnemyKilled;
        private Action<object> m_OnWavePassed;
        private Action<object> m_OnUpgradeShield;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnLevelUnlocked = param => UpdateLevelUnlockQuest();
            m_OnGunUnlocked = param => UpdateGunUnlockQuest();
            m_OnUseBooster = param => UpdateUserBoosterQuest();
            m_OnEnemyKilled = param => UpdateEnemyKilledQuest();
            m_OnWavePassed = param => UpdateWavePassedQuest();
            m_OnUpgradeShield = param => UpdateUpgradeShieldQuest();

            this.RegisterActionEvent(GameState.Completed, m_OnLevelUnlocked);
            this.RegisterActionEvent(GameplayAction.GUN_UNLOCKED, m_OnGunUnlocked);
            this.RegisterActionEvent(GameplayAction.BUY_BOOSTER, m_OnUseBooster);
            this.RegisterActionEvent(EnemyAction.DIE, m_OnEnemyKilled);
            this.RegisterActionEvent(GameplayAction.WAVE_PASSED, m_OnWavePassed);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpgradeShield);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameState.Completed, m_OnLevelUnlocked);
            this.RemoveActionEvent(GameplayAction.GUN_UNLOCKED, m_OnGunUnlocked);
            this.RemoveActionEvent(GameplayAction.BUY_BOOSTER, m_OnUseBooster);
            this.RemoveActionEvent(EnemyAction.DIE, m_OnEnemyKilled);
            this.RemoveActionEvent(GameplayAction.WAVE_PASSED, m_OnWavePassed);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpgradeShield);
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

        private void UpdateLevelUnlockQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.LevelUnlock, 1);
        }

        private void UpdateGunUnlockQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.WarriorUnlock, 1);
        }

        private void UpdateEnemyKilledQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.EnemyKill, 1);
        }

        private void UpdateUserBoosterQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.UseBooster, 1);
        }

        private void UpdateWavePassedQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.WavePassed, 1);
        }

        private void UpdateUpgradeShieldQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.UpgradeShield, 1);
        }
    }
}
