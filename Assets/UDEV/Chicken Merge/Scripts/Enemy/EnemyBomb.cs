using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class EnemyBomb : EnemyWeapon
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_explosionVfxPool;

        public override void DealDamage(Enemy enemy)
        {
            if (enemy == null) return;

            if (m_shiedlHitted == null || enemy.CurStat == null) return;

            var damageable = m_shiedlHitted.GetComponent<IDamageable>();
            damageable?.TakeDamage(enemy.CurStat.RealDamage);

            enemy.DeadTrigger();
            PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_explosionVfxPool, transform.position, Quaternion.identity);
        }
    }
}
