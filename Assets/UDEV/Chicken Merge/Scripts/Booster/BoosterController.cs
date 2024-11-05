using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class BoosterController : RepeatingAction
    {
        public BoosterType type;
        public BoosterSO stat;

        protected bool m_isActive;
        public UnityEvent<double> OnBoosterRunning;

        public bool IsActive { get => m_isActive; }

        private void Awake()
        {
            if (stat)
            {
                m_repeatRateSeconds = stat.timeTrigger;
                m_inTimeSeconds = 0;
            }

            onActionWaiting.AddListener(ActiveBooster);
            onActionReached.AddListener(DeactiveBooster);
        }

        private void OnDisable()
        {
            onActionReached.RemoveListener(DeactiveBooster);
            onActionWaiting.RemoveListener(ActiveBooster);
        }

        private void ActiveBooster()
        {
            m_isActive = true;
        }

        public void DeactiveBooster()
        {
            m_isActive = false;
        }

        public override void UpdateAction()
        {
            if (IsActionReached)
            {
                onActionReached?.Invoke();
            }
            else
            {
                double waitingTime = m_repeatRateSeconds - TimeActionHelper.GetActionDeltaTime(UserDataHandler.Ins.timeActions, m_actionName);
                OnBoosterRunning?.Invoke(waitingTime);
                onActionWaiting?.Invoke();
                this.PostActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE);
            }
        }

        public void SetTimeAction()
        {
            TimeActionHelper.SetActionTime(ref UserDataHandler.Ins.timeActions, m_actionName);
        }
    }
}
