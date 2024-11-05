using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ExplosionController : MonoBehaviour, IDamageCreator
    {
        [SerializeField] private LayerMask m_checkingLayer;
        [SerializeField] private float m_damageRadius;
        private float m_damage;
        List<Actor> m_targets;

        public float Damage { get => m_damage; set => m_damage = value; }

        private void Update()
        {
            if (m_targets != null && m_targets.Count > 0) return;
                DealDamage();
        }

        public void DealDamage()
        {
            m_targets = ScanTargets();

            if(m_targets == null || m_targets.Count <= 0) return;

            foreach(var target in m_targets)
            {
                if (target == null) continue;
                Vector2 centerPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 rayCastDir = (Vector2)target.transform.position - centerPos;
                RaycastHit2D[] hitPoints = Physics2D.RaycastAll(centerPos, rayCastDir, m_damageRadius, m_checkingLayer);
                foreach (var hitPoint in hitPoints)
                {
                    if (!hitPoint || hitPoint.collider == null) continue;
                    if (hitPoint.collider.gameObject != target.gameObject) return;
                    Vector2 hitPointDir = (Vector2)target.transform.position - hitPoint.point;
                    float finalDamage = Mathf.Abs(m_damage * (1 - hitPointDir.magnitude / m_damageRadius));
                    
                    target.TakeDamage(finalDamage);
                }
            }
        }

        private List<Actor> ScanTargets()
        {
            List<Actor> findedTargets = new List<Actor>();

            var targets = Physics2D.OverlapCircleAll(transform.position, m_damageRadius, m_checkingLayer);

            if (targets == null || targets.Length <= 0) return null;
            foreach (var target in targets)
            {
                if (target == null) continue;
                var actor = target.GetComponent<Actor>();
                if (actor == null) continue;
                findedTargets.Add(actor);
            }

            return findedTargets;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, m_damageRadius);
        }
    }
}
