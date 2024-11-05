using System;
using System.Collections.Generic;
using System.Linq;
using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class SkillCollectedManager : Singleton<SkillCollectedManager>, IActionEventDispatcher
    {
        [SerializeField]
        private SkillController[] m_skillControllers;
        private Dictionary<SkillCollectedType, int> m_skillCollecteds;
        public Dictionary<SkillCollectedType, int> SkillCollecteds { get => m_skillCollecteds; }

        #region ACTION
        private Action<object> m_OnStopAllSkill;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnStopAllSkill = param => StopAll();

            this.RegisterActionEvent(GameState.Gameover, m_OnStopAllSkill);
            this.RegisterActionEvent(GameState.Completed, m_OnStopAllSkill);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameState.Gameover, m_OnStopAllSkill);
            this.RegisterActionEvent(GameState.Completed, m_OnStopAllSkill);
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void Init()
        {
            m_skillCollecteds = new Dictionary<SkillCollectedType, int>();

            if (m_skillControllers == null || m_skillControllers.Length <= 0) return;

            for (int i = 0; i < m_skillControllers.Length; i++)
            {
                var skillController = m_skillControllers[i];
                if(skillController == null) continue;
                skillController.OnStopWithType.AddListener(RemoveSkill);
                m_skillCollecteds.Add(skillController.type, 0);
            }
        }

        public void AddRandomSkillForTest()
        {
            for (int i = 0; i < 5; i++)
            {
                var randomSkillType = Helper.GetRandomEnum<SkillCollectedType>();
                AddSkill(randomSkillType);
            }
        }

        public SkillController GetSkillController(SkillCollectedType skillType)
        {
            var findeds = m_skillControllers.Where(s => s.type == skillType).ToArray();
            if (findeds == null || findeds.Length <= 0) return null;
            return findeds[0];
        }

        public int GetInGameSkillAmount(SkillCollectedType skillType)
        {
            if(!IsExist(skillType)) return 0;
            return m_skillCollecteds[skillType];
        }

        public void AddSkill(SkillCollectedType skillType, int amount = 1)
        {
            if (IsExist(skillType))
            {
                if (IsMaxCapacity(skillType)) return;

                var skillAmount = m_skillCollecteds[skillType];
                skillAmount += amount;
                m_skillCollecteds[skillType] = skillAmount;
            }else
            {
                m_skillCollecteds.Add(skillType, amount);
            }
        }

        public void RemoveSkill(SkillCollectedType skillType)
        {
            if (IsExist(skillType))
            {
                var skillAmount = m_skillCollecteds[skillType];
                skillAmount--;
                m_skillCollecteds[skillType] = skillAmount;
            }
        }

        public void ClearAll()
        {
            m_skillCollecteds.Clear();
        }

        public bool IsExist(SkillCollectedType skillType)
        {
            return m_skillCollecteds.ContainsKey(skillType);
        }

        public bool IsMaxCapacity(SkillCollectedType skillType)
        {
            if (!IsExist(skillType)) return false;

            var skillController = GetSkillController(skillType);
            var skillStat = skillController.skillStat;
            var skillAmount = m_skillCollecteds[skillType];
            return skillAmount >= skillStat.capacity;
        }

        public void Stop(SkillCollectedType skillType)
        {
            var skillController = GetSkillController(skillType);
            if(skillController == null) return;
            skillController.Stop();
        }

        public void StopAll()
        {
            if (m_skillControllers == null || m_skillControllers.Length <= 0) return;

            foreach (SkillController skillController in m_skillControllers)
            {
                if (skillController == null) continue;
                skillController.ForceStop();
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            var currentSkillCtrs = GetComponentsInChildren<SkillController>();

            m_skillControllers = currentSkillCtrs;
        }
#endif
    }
}