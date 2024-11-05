using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class Actor : MonoBehaviour, IDamageable
    {
        [Header("Common:")]
        [SerializeField] protected Stat m_stat;
        [SerializeField] protected Animator m_anim;
        public Vector3 spawnOffset;
        [Header("Layer:")]
        [LayerList]
        [SerializeField] protected int m_normalLayer;
        [LayerList]
        [SerializeField] protected int m_deadLayer;

        protected Rigidbody2D m_rb;

        protected float m_curHp;
        protected float m_curMoveSpeed;
        protected float m_damageTaked;
        protected bool m_canMove;
        protected bool m_isBoss;
        protected float m_startingAnimSpeed;

        public UnityEvent<Stat> OnLoadStat;
        public UnityEvent<float> OnTakeDamge;
        public UnityEvent OnDead;

        public bool CanMove { get => m_canMove; set => m_canMove = value; }
        public float DamageTaked { get => m_damageTaked; }

        protected virtual void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
            m_startingAnimSpeed = m_anim.speed;
        }

        public virtual void Init(bool isBoss = false)
        {
            m_isBoss = isBoss;
        }

        protected virtual void LoadStats()
        {

        }

        protected void SetVelocity(Vector2 velocity)
        {
            if (m_rb == null) return;
            m_rb.velocity = velocity;
        }

        protected virtual void Move()
        {
            if (!m_canMove) return;
            SetVelocity(Vector2.left * m_curMoveSpeed);
        }

        protected void StopMove()
        {
            SetVelocity(Vector2.zero);
            m_canMove = false;
        }

        public virtual void TakeDamage(float damage)
        {
            if (m_curHp > 0)
            {
                m_damageTaked = damage;
                m_curHp -= m_damageTaked;
                OnTakeDamge?.Invoke(m_curHp);
                if (m_curHp <= 0)
                {
                    m_curHp = 0;
                    Dead();
                }
            }
        }

        protected virtual void Dead()
        {

        }
    }
}
