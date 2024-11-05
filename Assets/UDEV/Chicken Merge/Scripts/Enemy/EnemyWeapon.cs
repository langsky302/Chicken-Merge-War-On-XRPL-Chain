using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class EnemyWeapon : MonoBehaviour
    {
        [SerializeField] private LayerMask shieldLayer;
        [SerializeField] private float searchRadius;
        protected Collider2D m_shiedlHitted;

        protected void ShieldHittedCheck()
        {
            m_shiedlHitted = Physics2D.OverlapCircle(transform.position, searchRadius, shieldLayer);
        }

        protected virtual void FixedUpdate()
        {
            ShieldHittedCheck();
        }

        public virtual void DealDamage(Enemy enemy)
        { 

        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }

        private void OnDisable()
        {
            m_shiedlHitted = null;
        }
    }
}
