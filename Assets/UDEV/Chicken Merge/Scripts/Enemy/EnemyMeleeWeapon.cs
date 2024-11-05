using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class EnemyMeleeWeapon : EnemyWeapon
    {
        public override void DealDamage(Enemy enemy)
        {
            if (enemy == null) return; 

            if (m_shiedlHitted == null || enemy.CurStat == null) return;

            var damageable = m_shiedlHitted.GetComponent<IDamageable>();
            damageable?.TakeDamage(enemy.CurStat.RealDamage);
        }
    }
}
