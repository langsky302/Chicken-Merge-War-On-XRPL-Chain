using System.Collections;
using UDEV.SPM;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class GunController : MonoBehaviour
    {
        [Header("Base Settings:")]
        [SerializeField] private GunStatSO m_stat;
        [SerializeField] Transform[] m_shootingPoints;
        private GunBonusReceiver m_bonusReceiver;

        [Header("Pool Settings:")]
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_bulletPoolKey;

        private float m_curFR;
        private float m_curDMG;
        private float m_critDamage;
        private int m_curGunId;
        private bool m_canShoot;
        private bool m_isShooted;
        private bool m_isShooting;
        private int m_bulletCounting;

        public UnityEvent<Transform> OnShoot;

        public GunStatSO Stat { get => m_stat; }
        public float CritDamage { get => m_critDamage; }
        public bool IsShooted { get => m_isShooted;}
        public bool IsShooting { get => m_isShooting;}

        private void Awake()
        {
            m_bonusReceiver = GetComponent<GunBonusReceiver>();
        }

        public void Init(int gunId)
        {
            m_curGunId = gunId;
            LoadStats(gunId);
            m_bonusReceiver?.UpdateSkillBoosterBonus();
        }

        public void LoadStats(int gunId)
        {
            if (m_stat == null || m_curGunId != gunId) return;
            m_stat.Load(gunId);

            m_curFR = m_stat.fireRate;
            m_curDMG = m_stat.damage;
            m_critDamage = m_curDMG * 2;
            m_canShoot = false;
            m_bulletCounting = m_stat.numberOfBullets;
        }

        private float GetRealDamage()
        {
            if (m_bonusReceiver == null) return m_curDMG;

            float critRate = Random.value;

            if (critRate <= m_stat.critRate)
            {
                return m_bonusReceiver.GetRealCritDamage();
            }

            return m_bonusReceiver.GetRealNormalDamage();
        }

        private void ShootCore()
        {
            if (m_shootingPoints == null || m_shootingPoints.Length <= 0) return;

            foreach (var shootingpoint in m_shootingPoints)
            {
                if(shootingpoint == null) continue;
                var projectileClone = PoolersManager.Ins?.Spawn(PoolerTarget.NONE, m_bulletPoolKey, shootingpoint.position, Quaternion.identity);
                if (projectileClone == null) return;
                var projectile = projectileClone.GetComponent<Projectile>();
                projectile?.Init(GetRealDamage());
                OnShoot?.Invoke(shootingpoint);
            }
        }

        private IEnumerator ShootCorountine()
        {
            while(m_bulletCounting > 0)
            {
                yield return new WaitForSeconds(Random.Range(0.08f, 0.2f));
                ShootCore();
                m_isShooting = true;
                m_bulletCounting--;
            }
            yield return new WaitForSeconds(Random.Range(0.08f, 0.12f));
            m_isShooting = false;
        }

        private void Shoot()
        {
            if (m_isShooted) return;
            m_isShooted = true;
            StartCoroutine(ShootCorountine());
        }

        private void Update()
        {
            ReduceFirerate();
        }

        private void ReduceFirerate()
        {
            if (!m_isShooted || !m_canShoot) return;
            m_curFR -= Time.deltaTime;

            if (m_curFR <= 0 && m_isShooted)
            {
                m_isShooted = false;
            }
        }

        public void ShootTrigger()
        {
            m_canShoot = true;

            if (m_isShooted) return;
            m_bulletCounting = m_stat.numberOfBullets;
            if (m_bonusReceiver)
                m_curFR = m_bonusReceiver.GetRealFireRate();
            m_curFR += Random.Range(-0.05f, 0.05f);
            Shoot();
        }

        public void StopShoot()
        {
            StopAllCoroutines();
            m_isShooting = false;
            m_canShoot = false;
        }
    }
}
