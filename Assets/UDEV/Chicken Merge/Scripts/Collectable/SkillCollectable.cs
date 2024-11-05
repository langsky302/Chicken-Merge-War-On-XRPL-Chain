using UDEV.ActionEventDispatcher;

namespace UDEV.ChickenMerge
{
    public class SkillCollectable : Collectable
    {
        public override void Init(int bonusMultier = 1)
        {
            base.Init(bonusMultier);
            Trigger();
        }

        protected override void TriggerHandle()
        {
            for (int i = 0; i < m_bonus; i++) {
                var randomSkill = Helper.GetRandomEnum<SkillCollectedType>();
                SkillCollectedManager.Ins?.AddSkill(randomSkill);
            }

            Invoke("UpdateSkillCollected", 0.15f);
        }

        private void UpdateSkillCollected()
        {
            this.PostActionEvent(GameplayAction.SKILL_COLLECTED_UPDATE);
        }
    }
}
