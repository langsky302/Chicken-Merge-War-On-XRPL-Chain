using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class SkillController : MonoBehaviour
    {
        public SkillCollectedType type;
        public SkillCollectedSO skillStat;
        protected bool m_isTriggered;
        protected bool m_isCooldowning;

        protected float m_cooldownTime;
        protected float m_timeTrigger;

        public UnityEvent OnTriggerEnter;
        public UnityEvent OnTriggerUpdate;
        public UnityEvent OnCooldown;
        public UnityEvent OnStop;
        public UnityEvent<SkillCollectedType> OnStopWithType;
        public UnityEvent OnCooldownStop;

        public float cooldownProgress
        {
            get => m_cooldownTime / skillStat.cooldownTime;
        }

        public float TriggerProgress
        {
            get => m_timeTrigger / skillStat.timeTrigger;
        }
        public float CooldownTime { get => m_cooldownTime; }
        public bool IsTriggered { get => m_isTriggered; }
        public bool IsCooldown { get => m_isCooldowning; }

        public virtual void LoadStat()
        {
            if (skillStat == null) return;

            m_cooldownTime = skillStat.cooldownTime;
            m_timeTrigger = skillStat.timeTrigger;
        }

        public void Trigger()
        {
            if (m_isTriggered || m_isCooldowning) return;
            m_isCooldowning = true;
            m_isTriggered = true;
            OnTriggerEnter?.Invoke();
            Debug.Log("Skill Was Trigger");
        }

        private void Update()
        {
            CoreHandle();
        }

        private void CoreHandle()
        {
            ReduceTriggerTime();
            ReduceCooldownTime();
        }

        private void ReduceCooldownTime()
        {
            if (!m_isCooldowning) return;
            m_cooldownTime -= Time.deltaTime;
            OnCooldown?.Invoke();
            if (m_cooldownTime > 0) return;
            m_isCooldowning = false;
            OnCooldownStop?.Invoke();
            m_cooldownTime = skillStat.cooldownTime;
        }

        private void ReduceTriggerTime()
        {
            if (!m_isTriggered) return;
            m_timeTrigger -= Time.deltaTime;
            if (m_timeTrigger <= 0)
            {
                Stop();
            }
            OnTriggerUpdate?.Invoke();
        }

        public void Stop()
        {
            m_timeTrigger = skillStat.timeTrigger;
            m_isTriggered = false;
            OnStopWithType?.Invoke(type);
            OnStop?.Invoke();
        }

        public void ForceStop()
        {
            m_isCooldowning = false;
            m_isTriggered = false;
            LoadStat();
        }
    }
}
