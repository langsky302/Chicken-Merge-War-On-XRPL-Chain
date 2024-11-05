using System;
using UDEV.ActionEventDispatcher;

namespace UDEV.ChickenMerge
{
    public class AudioController : AudioBase
    {
        #region ACTION
        private Action<object> m_OnReward;
        private Action<object> m_OnBuyBooster;
        private Action<object> m_OnBuyGunSuccess;
        private Action<object> m_OnUpgrade;
        private Action<object> m_OnUpgradeSkill;
        private Action<object> m_OnGunUnlocked;
        private Action<object> m_OnGameStarting;
        private Action<object> m_OnLevelCompleted;
        private Action<object> m_OnGameover;
        #endregion

        #region REGISTER_EVENTS
        public override void RegisterEvents()
        {
            m_OnReward = param => PlayReward();
            m_OnBuyBooster = param => PlayBuyBooster();
            m_OnBuyGunSuccess = param => PlayBuyingGun();
            m_OnUpgrade = param => PlayUpgradeSound();
            m_OnUpgradeSkill = param => PlayUpgradeSkill();
            m_OnGunUnlocked = param => PlayGunUnlocked();
            m_OnGameStarting = param => PlayGameBG();
            m_OnLevelCompleted = param => PlayLevelCompleted();
            m_OnGameover = param => PlayGameover();

            UnregisterEvents();
            this.RegisterActionEvent(GameplayAction.GET_REWARD, m_OnReward);
            this.RegisterActionEvent(GameplayAction.BUY_BOOSTER, m_OnBuyBooster);
            this.RegisterActionEvent(GameplayAction.BUY_GUN, m_OnBuyGunSuccess);
            this.RegisterActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpgrade);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpgrade);
            this.RegisterActionEvent(GameplayAction.REPAIR_SHIELD, m_OnUpgrade);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpgradeSkill);
            this.RegisterActionEvent(GameplayAction.GUN_UNLOCKED, m_OnGunUnlocked);
            this.RegisterActionEvent(GameState.Starting, m_OnGameStarting);
            this.RegisterActionEvent(GameState.Completed, m_OnLevelCompleted);
            this.RegisterActionEvent(GameState.Gameover, m_OnGameover);
        }

        public override void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.GET_REWARD, m_OnReward);
            this.RemoveActionEvent(GameplayAction.BUY_BOOSTER, m_OnBuyBooster);
            this.RemoveActionEvent(GameplayAction.BUY_GUN, m_OnBuyGunSuccess);
            this.RemoveActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpgrade);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpgrade);
            this.RemoveActionEvent(GameplayAction.REPAIR_SHIELD, m_OnUpgrade);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpgradeSkill);
            this.RemoveActionEvent(GameplayAction.GUN_UNLOCKED, m_OnGunUnlocked);
            this.RemoveActionEvent(GameState.Starting, m_OnGameStarting);
            this.RemoveActionEvent(GameState.Completed, m_OnLevelCompleted);
            this.RemoveActionEvent(GameState.Gameover, m_OnGameover);
        }
        #endregion
        private void PlayReward()
        {
            PlaySound(data.reward);
        }

        private void PlayBuyBooster()
        {
            PlaySound(data.buyBooster);
        }

        private void PlayBuyingGun()
        {
            PlaySound(data.buySuccess);
        }

        private void PlayUpgradeSound()
        {
            PlaySound(data.upgradeSuccess);
        }

        private void PlayUpgradeSkill()
        {
            PlaySound(data.upgradeSuccess);
        }

        private void PlayGunUnlocked()
        {
            PlaySound(data.gunUnlocked);
        }

        private void PlayGameBG()
        {
            PlayMusic(data.bgms);
        }

        private void PlayLevelCompleted()
        {
            StopPlayMusic();
            PlaySound(data.levelCompleted);
        }

        private void PlayGameover()
        {
            StopPlayMusic();
            PlaySound(data.gameOver);
        }
    }
}
