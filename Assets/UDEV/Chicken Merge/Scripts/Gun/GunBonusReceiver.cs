using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class GunBonusReceiver : MonoBehaviour
    {
        private float m_dmgBonusFromSkill;
        private float m_critDmgBonusFromSkill;
        private bool m_isDoubleDmgBoosterActitve;
        private bool m_isDoubleFRBoosterActitve;
        private GunController m_gunCtr;

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

        private void Awake()
        {
            m_gunCtr = GetComponent<GunController>();
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            RegisterEvents();
        }

        public float GetRealNormalDamage()
        {
            float missRate = UnityEngine.Random.Range(0f, m_gunCtr.Stat.damageMissRange);
            float totalDamage = m_gunCtr.Stat.damage + m_dmgBonusFromSkill;
            if (m_isDoubleDmgBoosterActitve)
            {
                totalDamage = m_gunCtr.Stat.damage * 2 + m_dmgBonusFromSkill;
            }
            float damageMissed = totalDamage * missRate;
            return totalDamage - damageMissed;
        }

        public float GetRealCritDamage()
        {
            float missRate = UnityEngine.Random.Range(0f, m_gunCtr.Stat.damageMissRange);
            float totalCritDamage = m_gunCtr.CritDamage;
            if (m_isDoubleDmgBoosterActitve)
            {
                totalCritDamage = m_gunCtr.CritDamage * 2;
            }
            totalCritDamage += m_critDmgBonusFromSkill + m_dmgBonusFromSkill;
            float damageMissed = totalCritDamage * missRate;
            return totalCritDamage - damageMissed;
        }

        public float GetRealFireRate()
        {
            if (m_isDoubleFRBoosterActitve)
            {
                return m_gunCtr.Stat.fireRate / 2f;
            }
            else
            {
                return m_gunCtr.Stat.fireRate;
            }
        }

        public void UpdateSkillBoosterBonus()
        {
            m_dmgBonusFromSkill = DataGroup.Ins.GetSkillBonus(PassiveSkillType.GUN_DAMAGE_UP, m_gunCtr.Stat.damage);
            m_critDmgBonusFromSkill = DataGroup.Ins.GetSkillBonus(PassiveSkillType.CRIT_DAMAGE_UP, m_gunCtr.CritDamage);
            m_isDoubleDmgBoosterActitve = BoosterManager.Ins.IsBoosterActive(BoosterType.DOUBLE_GUN_DAMAGE);
            m_isDoubleFRBoosterActitve = BoosterManager.Ins.IsBoosterActive(BoosterType.DOUBLE_GUN_FIRE_RATE);
        }
    }
}
