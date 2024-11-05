using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class FireballController : SkillBullet
    {
        private void Update()
        {
            Move();
        }

        protected override void Move()
        {
            if (m_target == null) return;

            m_curSpeed = Random.Range((int)m_minSpeed, (int)m_maxSpeed);
            float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
            Vector2 targetDir = m_target.position - transform.position;
            DealDamage(distanceToTarget);
            if (m_target.gameObject.activeInHierarchy)
            {
                transform.Translate(targetDir.normalized * Time.deltaTime * m_curSpeed, Space.World);
            }
            else
            {
                transform.Translate(transform.right * Time.deltaTime * m_curSpeed, Space.World);
            }
        }
    }
}
