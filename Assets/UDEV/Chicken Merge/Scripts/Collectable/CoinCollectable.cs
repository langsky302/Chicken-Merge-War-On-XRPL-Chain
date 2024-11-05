using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class CoinCollectable : Collectable
    {
        public override void Init(int bonusMultier = 1)
        {
            base.Init(bonusMultier);
            m_bonus *= bonusMultier;
            float bonusFromSkill = DataGroup.Ins.GetSkillBonus(PassiveSkillType.INCREASE_ENEMY_BONUS, m_bonus);
            m_bonus += Mathf.RoundToInt(bonusFromSkill);
            Trigger();
        }

        protected override void TriggerHandle()
        {
            UserDataHandler.Ins.coin += m_bonus;
            this.PostActionEvent(GameplayAction.UPDATE_COIN);
        }
    }
}
