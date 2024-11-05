using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class EnemyBonusReceiver : MonoBehaviour, IActionEventDispatcher
    {
        private bool m_isX2CoinBoosterActive;

        #region ACTION
        private Action<object> m_OnSkillBoosterUpdate;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnSkillBoosterUpdate = param => UpdateSkillBoosterBonus();

            this.RegisterActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE, m_OnSkillBoosterUpdate);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE, m_OnSkillBoosterUpdate);
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

        public int GetBonusMultier(int bonusMultier)
        {
            if(m_isX2CoinBoosterActive)
                return bonusMultier * 2;

            return bonusMultier;
        }

        public void UpdateSkillBoosterBonus()
        {
            m_isX2CoinBoosterActive = BoosterManager.Ins.IsBoosterActive(BoosterType.DOUBLE_ENEMY_COIN);
        }
    }
}
