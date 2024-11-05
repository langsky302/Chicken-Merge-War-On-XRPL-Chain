using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class EnemySkillAffected : MonoBehaviour
    {
        private bool m_isFreezing;
        private bool m_isMovementReduced;

        public bool IsFreezing { get => m_isFreezing;}
        public bool IsMovementReduced { get => m_isMovementReduced;}

        public void SetupSkill()
        {
            var freezeSkill = AddEventToSkill(SkillCollectedType.FreezeAllEnemy, Freeze, Unfreeze);
            var reduceMovementSkill = AddEventToSkill(SkillCollectedType.ReduceHaftMovement, MovingReduced, BackToNormalMoving);

            m_isFreezing = freezeSkill.IsTriggered;
            m_isMovementReduced = reduceMovementSkill.IsTriggered;
        }

        private SkillController AddEventToSkill(SkillCollectedType type, UnityAction OnTrigger = null, UnityAction OnStop = null)
        {
            var skillController = SkillCollectedManager.Ins.GetSkillController(type);
            if (skillController == null) return null;
            skillController.OnTriggerEnter.RemoveListener(OnTrigger);
            skillController.OnStop.RemoveListener(OnStop);

            skillController.OnTriggerEnter.AddListener(OnTrigger);
            skillController.OnStop.AddListener(OnStop);
            return skillController;
        }

        private void Freeze()
        {
            m_isFreezing = true;
        }

        private void Unfreeze()
        {
            m_isFreezing = false;
        }

        private void MovingReduced()
        {
            m_isMovementReduced = true;
        }

        private void BackToNormalMoving()
        {
            m_isMovementReduced = false;
        }
    }
}
