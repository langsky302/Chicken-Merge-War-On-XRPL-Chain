using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ShieldBonusReceiver : MonoBehaviour, IActionEventDispatcher
    {
        private bool m_isInvincibleShieldActived;

        #region ACTION
        private Action<object> m_OnSkillBoosterUpdate;

        public bool IsInvincibleShieldActived { get => m_isInvincibleShieldActived;}
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

        public void UpdateSkillBoosterBonus()
        {
            m_isInvincibleShieldActived = BoosterManager.Ins.IsBoosterActive(BoosterType.INVINCIBLE_SHIELD);
        }
    }
}
