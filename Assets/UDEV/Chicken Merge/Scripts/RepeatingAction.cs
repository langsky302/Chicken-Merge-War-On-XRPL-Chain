using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class RepeatingAction : MonoBehaviour
    {
        [SerializeField] protected string m_actionName = "";
        [SerializeField] protected int m_repeatRateSeconds = 1;
        [SerializeField] protected int m_inTimeSeconds = 0;

        public bool IsActionReached
        {
            get => TimeActionHelper.IsActionAvailable(UserDataHandler.Ins.timeActions, m_actionName, m_repeatRateSeconds);
        }

        public UnityEvent onActionReached;
        public UnityEvent onActionWaiting;

        protected virtual void Start()
        {
            if (TimeActionHelper.GetAction(UserDataHandler.Ins.timeActions, m_actionName) == null) // First time.
            {
                double repeatRateSeconds = TimeActionHelper.GetCurrentTime() - m_repeatRateSeconds + m_inTimeSeconds;
                TimeActionHelper.SetActionTime(ref UserDataHandler.Ins.timeActions, m_actionName, repeatRateSeconds);
                UserDataHandler.Ins?.SaveData();
            }

            UpdateAction();
        }

        public virtual void UpdateAction()
        {
            if (IsActionReached)
            {
                TimeActionHelper.SetActionTime(ref UserDataHandler.Ins.timeActions, m_actionName);

                UserDataHandler.Ins?.SaveData();

                onActionReached?.Invoke();
            }
            else
            {
                onActionWaiting?.Invoke();
            }
        }
    }
}
