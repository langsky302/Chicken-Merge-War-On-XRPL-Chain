using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class EnemyAnimEvent : MonoBehaviour
    {
        [SerializeField] private Enemy m_enemy;
        [SerializeField] private EnemyWeapon m_weapon;

        public void MoveTrigger()
        {
            if(m_enemy == null) return;

            m_enemy.CanMove = true;
        }

        public void DealWeaponDamage()
        {
            if (m_enemy == null) return;

            m_weapon?.DealDamage(m_enemy);
        }
    }
}
