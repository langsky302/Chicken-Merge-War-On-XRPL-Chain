using System.Collections;
using System.Collections.Generic;
using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class SkillBullet : MonoBehaviour
    {
        [SerializeField] protected float m_minSpeed;
        [SerializeField] protected float m_maxSpeed;
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] protected string m_hitPoolKey;
        protected float m_curSpeed;
        protected float m_damage;
        protected Transform m_target;

        public virtual void Init(Transform target, float damage)
        {
            m_target = target;
            m_damage = damage;
        }

        protected virtual void Move()
        {

        }

        protected virtual void DealDamage(float distanceToCheck)
        {
            if (distanceToCheck > 0.16f || !m_target.gameObject.activeInHierarchy) return;
            m_curSpeed = 0;
            transform.position = m_target.position;
            var idamageable = m_target.GetComponent<IDamageable>();
            idamageable?.TakeDamage(m_damage);
            gameObject.SetActive(false);
            PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_hitPoolKey, transform.position, Quaternion.identity);
        }

        protected virtual void OnTriggerEnter2D(Collider2D target)
        {
            if (m_target.gameObject.activeInHierarchy) return;

            if (target.CompareTag(GameTag.Enemy.ToString()))
            {
                m_target = target.transform;
            }
        }
    }
}
