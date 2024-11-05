using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class HomingMissleController : SkillBullet
    {
        private Rigidbody2D m_rb;
        private float m_speed;
        private Vector2 m_targetDirection;
        private Vector2 m_startingDirection;
        private bool m_isHaveTarget;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            m_speed = Random.Range((int)m_minSpeed, (int)m_maxSpeed);
            m_isHaveTarget = false;
        }

        private void FixedUpdate()
        {
            Move();
        }

        protected override void Move()
        {
            if(m_rb == null) return;
            if(m_target == null)
            {
                m_targetDirection = Vector2.right;
                ForceHandle(m_targetDirection);
                return;
            }

            if (m_target.gameObject.activeInHierarchy)
            {
                m_targetDirection = (m_target.transform.position - transform.position).normalized;  
                if(!m_isHaveTarget)
                {
                    m_startingDirection = m_targetDirection;
                    m_isHaveTarget = true;
                }

                ForceHandle(m_targetDirection);

                float distanceToTarget = Vector2.Distance(transform.position, m_target.position);

                DealDamage(distanceToTarget);
            }else
            {
                ForceHandle(m_startingDirection);
            }
        }

        private void ForceHandle(Vector2 direction)
        {
            Vector2 desiredVelocity = direction * m_speed;
            Vector2 steeringForce = desiredVelocity - m_rb.velocity;

            if (m_rb.velocity.x < 0)
            {
                m_rb.AddForce(new Vector2(steeringForce.x, steeringForce.y));
            }
            else
                m_rb.AddForce(steeringForce);

            float angleRad = Mathf.Atan2(m_rb.velocity.y,
                                      m_rb.velocity.x);

            float angleDeg = angleRad * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angleDeg - 90);
        }
    }
}
