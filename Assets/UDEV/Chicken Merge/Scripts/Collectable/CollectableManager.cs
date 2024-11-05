using System.Linq;
using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class CollectableManager : MonoBehaviour
    {
        [SerializeField] private CollectableGroupSO m_dataSO;
        private float[] m_spawnRates;

        private void Awake()
        {
            UpdateSpawnRates();
        }

        public void SpawnCollectable(Vector3 spawnPoint, int bonusMultier = 1)
        {
            if(m_dataSO == null || m_dataSO.collectableItems == null || m_dataSO.collectableItems.Length <= 0) return;

            int collectableItemIndex = GetCollectableItemIndex();
            var collectableItem = m_dataSO.collectableItems[collectableItemIndex];
            var collectableClone = 
                PoolersManager.Ins.Spawn(PoolerTarget.NONE, collectableItem.poolKey, spawnPoint, Quaternion.identity);
            var collectableComp = collectableClone.GetComponent<Collectable>();
            collectableComp?.Init(bonusMultier);
        }

        private void UpdateSpawnRates()
        {
            if (m_dataSO == null || m_dataSO.collectableItems == null || m_dataSO.collectableItems.Length <= 0) return;

            var spawnRates = m_dataSO.collectableItems.Select(c => c.spawnRate).ToArray();
            m_spawnRates = spawnRates;
        }

        private int GetCollectableItemIndex()
        {
            if (m_spawnRates == null || m_spawnRates.Length <= 0) return 0;
            float totalSpawnRate = 0;
            for (int i = 0; i < m_spawnRates.Length; i++)
            {
                var spawnRate = m_spawnRates[i];
                totalSpawnRate += spawnRate;
            }

            float randomValue = Random.Range(0, totalSpawnRate);
            float spawnRateTemp = 0;
            for (int i = 0; i < m_spawnRates.Length; i++)
            {
                float spawnRate = m_spawnRates[i];
                float maxSpawnRate = spawnRateTemp + spawnRate;
                if(randomValue >= spawnRateTemp && randomValue < maxSpawnRate)
                {
                    return i;
                }
                spawnRateTemp += spawnRate;
            }
            return 0;
        }
    }

}