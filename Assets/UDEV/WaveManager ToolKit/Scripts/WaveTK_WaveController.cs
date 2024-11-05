using System.Collections;
using System.Collections.Generic;
using UDEV.SPM;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.WaveManagerToolkit
{
    public class WaveTK_WaveController : Singleton<WaveTK_WaveController>
    {
        public static WaveTK_State state;
        public List<WaveTK_Wave> waves = new List<WaveTK_Wave>();
        [HideInInspector]
        public List<GameObject> enemyRemainings = new List<GameObject>();
        [Space]
        public Vector3 spawnPosition;       
        public bool spawnAtTransform;          
        public List<Transform> spawnTransforms = new List<Transform>();
        public Vector3 randomSpawnPos;

        private WaveTK_Wave m_currentWave;

        private int m_currentWaveIdx = 0;        
        private int m_waveCounter;            
        private bool m_inWave;                
        private bool m_allSpawnFinished = false;   
        private int m_lastCompleted = -1;
        [Space]
        [Header("Events:")]
        [Space]
        public UnityEvent OnFinalWaveFinished;
        public UnityEvent OnAllSpawnFinished;
        public UnityEvent OnWaveHasFinished;
        public UnityEvent OnWaveStarts;
        public UnityEvent OnResetWave;
        public UnityEvent<GameObject> OnEnemySpawned;

        private float m_spawnXpos, m_spawnYpos, m_spawnZpos;                         

        protected override void Awake()
        {
            MakeSingleton(false);
        }

        #region GETTER
        public int CurrentWaveIndex
        {
            get => m_currentWaveIdx;
        }
        public int WaveRemaining
        {
            get => waves.Count - m_currentWaveIdx;
        }
        public int EnemiesRemaining
        {
            get => enemyRemainings.Count;
        }
        public bool CurrentlyInWave
        {
            get => m_inWave;
        }
        public int LastCompletedWave
        {
            get => m_lastCompleted;
        }
        public bool AllEnemiesSpawned
        {
            get => m_allSpawnFinished;
        }
        public WaveTK_Wave CurrentWave { get => m_currentWave;}
        public float WaveProgress { get => (float)m_currentWave.enemyKilled / m_currentWave.totalEnemy; }

        #endregion

        #region User Functions
        public void StartWave()
        {
            if (m_currentWaveIdx >= waves.Count) return;
            state = WaveTK_State.RUNNING;
            UpdateEnemyInfo();
            m_inWave = true;
            OnWaveStarts?.Invoke();
            m_waveCounter = 0;
            StartCoroutine(CoreHandleCorountine());
        }

        public void PauseWave()
        {
            state = WaveTK_State.PAUSING;
        }

        public void ResumeWave()
        {
            state = WaveTK_State.RUNNING;
        }

        public void EndWave()
        {
            StopAllCoroutines();
            m_lastCompleted = m_currentWaveIdx;
            m_currentWaveIdx += 1;
            OnWaveHasFinished?.Invoke();
            StartWave();
            m_allSpawnFinished = false;
            m_inWave = false;
            if (m_currentWaveIdx == waves.Count)
            {
                state = WaveTK_State.FINISHED;
                OnFinalWaveFinished.Invoke();
            }

        }

        public void ResetCurrentWave()
        {
            m_currentWaveIdx = 0;
            StopAllCoroutines();
            foreach (GameObject enemy in enemyRemainings)
            {
                enemy.SetActive(false);
            }
        }
        #endregion

        void UpdateEnemyInfo()
        {
            WaveTK_Spawner currentSpawner;
            int spawnerCounter = 0;
            m_currentWave = waves[m_currentWaveIdx];
            m_currentWave.totalEnemy = 0;
            m_currentWave.enemyKilled = 0;
            while (spawnerCounter < m_currentWave.spawners.Count)
            {
                currentSpawner = m_currentWave.spawners[spawnerCounter];

                switch (currentSpawner.type)
                {
                    case WaveTK_SpawnerType.BUNDLE:
                        m_currentWave.totalEnemy += currentSpawner.totalEnemy;
                        break;
                    case WaveTK_SpawnerType.SINGLE:
                        m_currentWave.totalEnemy++;
                        break;
                }
                spawnerCounter++;
            }
        }

        public void AddEnemyKilled(int killed)
        {
            if (m_currentWaveIdx >= waves.Count) return;
            m_currentWave.enemyKilled += killed;
        }

        private IEnumerator CoreHandleCorountine()
        {
            WaveTK_Spawner currentSpawner;
            while (m_waveCounter < m_currentWave.spawners.Count) 
            {
                currentSpawner = m_currentWave.spawners[m_waveCounter];

                switch (currentSpawner.type)
                {
                    case WaveTK_SpawnerType.BUNDLE:
                        int bundleCounting = 0;
                        while (bundleCounting < currentSpawner.totalEnemy)
                        {
                            if (state != WaveTK_State.RUNNING)
                            {
                                yield return new WaitForSeconds(0.5f);
                                continue;
                            }
                            Spawn(currentSpawner);
                            yield return new WaitForSeconds(currentSpawner.GetTimeSpawn());
                            bundleCounting += 1;
                        }
                        break;
                    case WaveTK_SpawnerType.SINGLE:
                        Spawn(currentSpawner, currentSpawner.isBoss);
                        break;
                    case WaveTK_SpawnerType.DELAY:
                        yield return new WaitForSeconds(currentSpawner.GetTimeSpawn());
                        break;
                }
                m_waveCounter++;
            }
            m_allSpawnFinished = true;
            OnAllSpawnFinished?.Invoke();

            StartCoroutine(EndOfWaveCheckingCoroutine());
        }

        private IEnumerator EndOfWaveCheckingCoroutine()
        {
            while (enemyRemainings.Count > 0)
            {
                yield return new WaitForSeconds(0.5f);
                enemyRemainings.RemoveAll(x => !x.activeInHierarchy);
                if (enemyRemainings.Count == 0)
                {
                    EndWave();
                }
            }
        }

        private float MakeRandom(float start, float value)
        {
            return start + Random.Range((0 - value), value);
        }

        public Transform GetRandomSpawnPoint()
        {
            if (spawnTransforms == null || spawnTransforms.Count <= 0) return null;

            int randIdx = Random.Range(0, spawnTransforms.Count);
            return spawnTransforms[randIdx].transform;
        }

        private void Spawn(WaveTK_Spawner spawner, bool isBoss = false)
        {
            var randomSpawnPoint = GetRandomSpawnPoint();
            Vector3 spawnPos = randomSpawnPoint != null ? randomSpawnPoint.position : Vector3.zero;
            Vector3 startPoint = spawnAtTransform ? spawnPos : spawnPosition;

            m_spawnXpos = MakeRandom(startPoint.x, randomSpawnPos.x);
            m_spawnYpos = MakeRandom(startPoint.y, randomSpawnPos.y);
            m_spawnZpos = MakeRandom(startPoint.z, randomSpawnPos.z);
            startPoint = new Vector3(m_spawnXpos, m_spawnYpos, m_spawnZpos);
            string enemyPoolKey = (spawner.randomEnemy) ? spawner.enemySet.GetEnemy() : spawner.enemyPoolKey;
            var enemySpawned = PoolersManager.Ins.Spawn(PoolerTarget.NONE, enemyPoolKey, startPoint, Quaternion.identity);
            enemyRemainings.Add(enemySpawned);

            OnEnemySpawned?.Invoke(enemySpawned);
        }
    }
}
