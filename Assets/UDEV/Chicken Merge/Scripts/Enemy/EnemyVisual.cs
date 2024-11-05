using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class EnemyVisual : MonoBehaviour
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_appearVfx;
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_damageTxtPool;
        [Header("Vfx:")]
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_healthBarPool;
        [SerializeField] private Vector3 m_hpBarOffset;
        [SerializeField] private Vector3 m_hpBarScale = Vector3.one;
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] protected string m_deadVfxPool;
        private HealthBar m_healthBar;
        private Enemy m_enemy;
        private EnemyStatSO m_enemyStat;

        public HealthBar HealthBar { get => m_healthBar;}

        #region EVENTS
        private void AddEventsToEnemy()
        {
            if (m_enemy == null) return;

            m_enemy.OnLoadStat.AddListener(OnLoadStat);
            m_enemy.OnTakeDamge.AddListener(OnTakeDamage);
            m_enemy.OnDead.AddListener(OnDead);
        }

        private void RemoveEventsFromEnemy()
        {
            if (m_enemy == null) return;

            m_enemy.OnLoadStat.RemoveListener(OnLoadStat);
            m_enemy.OnTakeDamge.RemoveListener(OnTakeDamage);
            m_enemy.OnDead.RemoveListener(OnDead);
        }
        #endregion

        private void Awake()
        {
            m_enemy = GetComponent<Enemy>();
        }

        private void OnEnable()
        {
            AddEventsToEnemy();
        }

        private void LateUpdate()
        {
            if (m_healthBar)
            {
                m_healthBar.transform.position = transform.position + m_hpBarOffset;
            }
        }

        private void OnLoadStat(Stat stat)
        {
            m_enemyStat = (EnemyStatSO)stat;
            CreateHealthBarUI();
            m_healthBar?.UpdateValue(1);
        }
        
        private void OnTakeDamage(float curHp)
        {
            if(m_enemyStat == null) return;
            m_healthBar?.UpdateValue(curHp / m_enemyStat.RealHp);

            var damageTxtClone = PoolersManager.Ins.Spawn(
                PoolerTarget.NONE, m_damageTxtPool, m_healthBar.floatingDamagePoint.position, Quaternion.identity);
            var floatingDamage = damageTxtClone.GetComponent<FloatingDamage>();
            floatingDamage?.UpdateDamageTxt(m_enemy.DamageTaked);
        }

        protected void CreateHealthBarUI()
        {
            GameObject hpBar = PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_healthBarPool, transform.position, Quaternion.identity);
            if (hpBar == null) return;

            hpBar.transform.localScale = m_hpBarScale;
            m_healthBar = hpBar.GetComponent<HealthBar>();

            if (m_healthBar == null) return;

            m_healthBar.Show(true);
            m_healthBar.Root = transform;
        }

        public void SpawnAppearVfx()
        {
            PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_appearVfx, m_enemy.transform.position, Quaternion.identity);
        }

        private void OnDead()
        {
            PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_deadVfxPool, transform.position, Quaternion.identity);
            m_healthBar?.Show(false);
        }

        public void OnDisable()
        {
            RemoveEventsFromEnemy();

            if(m_healthBar != null)
            {
                m_healthBar.gameObject?.SetActive(false);
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (!string.IsNullOrEmpty(m_healthBarPool))
            {
                Gizmos.DrawIcon(transform.position + m_hpBarOffset, "HPBar_Icon.png", true);
            }
        }
    }
}
