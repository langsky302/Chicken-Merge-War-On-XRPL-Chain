using System.Collections;
using System.Collections.Generic;
using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class RainOfBullets : SkillController
    {
        [SerializeField] private LayerMask m_checkingLayer;
        [SerializeField] private float m_checkingRadius;
        [SerializeField] private Transform m_centerPoint;
        [SerializeField] private Transform[] m_spawnPoints;
        private RainOfBulletSO m_curStat;

        private void Awake()
        {
            m_curStat = (RainOfBulletSO)skillStat;
        }

        private void OnEnable()
        {
            OnTriggerEnter.AddListener(SpawnBullets);
        }

        private void OnDisable()
        {
            OnTriggerEnter.RemoveListener(SpawnBullets);
        }

        private void SpawnBullets()
        {
            StartCoroutine(SpawnBulletsCo());
        }

        private IEnumerator SpawnBulletsCo()
        {
            var randomTargets = GetRandomTarget();
            if(randomTargets != null && randomTargets.Count > 0)
            {
                for (int i = 0; i < m_curStat.numberOfBullet; i++)
                {
                    var randomTarget = randomTargets[i%randomTargets.Count];
                    var spawnPoint = Helper.GetRandom(m_spawnPoints);
                    var fireballClone = PoolersManager.Ins.Spawn(
                        PoolerTarget.NONE, m_curStat.bulletPoolKey, spawnPoint.position, Quaternion.identity);
                    var fireballCtr = fireballClone.GetComponent<SkillBullet>();
                    Helper.RotateFollowDirection(fireballClone.transform, randomTarget, fireballClone.transform);
                    fireballCtr?.Init(randomTarget, GetRealDamage());
                    float randomSpawnTime = Random.Range(0.1f, 0.2f);
                    yield return new WaitForSeconds(randomSpawnTime);
                }
            }
            yield return null;
        }

        private float GetRealDamage()
        {
            if (m_curStat == null) return 0;

            float missedDamage = m_curStat.damage * Random.Range(0f, m_curStat.damageMissRange);

            return m_curStat.damage - missedDamage;
        }

        private List<Transform> GetRandomTarget()
        {
            List<Transform> randomTargets = new List<Transform>();
            var targets = ScanTargets();
            if (targets == null || targets.Count <= 0 || m_curStat == null) return null;
            var numberOfFireball = targets.Count > m_curStat.numberOfBullet 
                ? m_curStat.numberOfBullet : targets.Count;
            while(numberOfFireball > 0)
            {
                int randomIdx = Random.Range(0, targets.Count);
                var randomTarget = targets[randomIdx];
                randomTargets.Add(randomTarget);
                targets.Remove(randomTarget);
                numberOfFireball--;
            }
            return randomTargets;
        }

        private List<Transform> ScanTargets()
        {
            List<Transform> findedTargets = new List<Transform>();

            var targets = Physics2D.OverlapCircleAll(m_centerPoint.position, m_checkingRadius, m_checkingLayer);

            if(targets == null || targets.Length <= 0 ) return null;

            foreach( var target in targets)
            {
                if(target == null) continue;
                findedTargets.Add(target.GetComponent<Transform>());
            }

            return findedTargets;
        }

        private void OnDrawGizmos()
        {
            if (m_centerPoint == null) return;
            Gizmos.DrawWireSphere(m_centerPoint.position, m_checkingRadius);
        }
    }
}
