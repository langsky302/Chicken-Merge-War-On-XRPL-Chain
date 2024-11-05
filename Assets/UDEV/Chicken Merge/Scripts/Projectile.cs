using UnityEngine;
using UDEV.SPM;

namespace UDEV.ChickenMerge
{
    public class Projectile : MonoBehaviour, IDamageCreator
    {
        [Header("Base Setting:")]
        [SerializeField]
        private LayerMask m_targetLayer;
        [SerializeField]
        private float m_speed;
        private float m_damage;
        public bool deactiveWhenHitted;

        [PoolerKeys(target = PoolerTarget.NONE)]
        public string bodyHitPool;

        private Vector2 m_prevPos;
        private RaycastHit2D m_hit;
        private Vector2 m_dir;

        private void Update()
        {
            transform.Translate(transform.right * m_speed * Time.deltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            DealDamage();

            RefreshLastPos();
        }

        public void Init(float damage)
        {
            m_damage = damage;
            RefreshLastPos();
        }

        public void DealDamage()
        {
            m_dir = (Vector2)transform.position - m_prevPos;
            float distance = m_dir.magnitude;
            m_dir.Normalize();

            m_hit = Physics2D.Raycast(m_prevPos, m_dir, distance, m_targetLayer);

            if (!m_hit || m_hit.collider == null) return;

            var targetHitted = m_hit.collider;

            Checking(targetHitted);
        }

        private void Checking(Collider2D collisionTarget)
        {
            IDamageable actor = collisionTarget.GetComponent<IDamageable>();

            if (actor == null) return;

            actor.TakeDamage(m_damage);

            PoolersManager.Ins?.Spawn(PoolerTarget.NONE, bodyHitPool, m_hit.point, Quaternion.identity);

            if (deactiveWhenHitted)
            {
                gameObject.SetActive(false);
            }
        }

        private void RefreshLastPos()
        {
            m_prevPos = (Vector2)transform.position;
        }

        private void OnDisable()
        {
            m_hit = new RaycastHit2D();
            transform.position = new Vector3(1000f, 1000f, 0);
        }
    }
}
