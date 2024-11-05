using UDEV.SPM;
using UnityEngine;

namespace UDEV.WaveManagerToolkit
{
    [System.Serializable]
    public class WaveTK_Spawner
    {
        public WaveTK_SpawnerType type;
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string enemyPoolKey; 
        public WaveTK_EnemySet enemySet; 
        public bool randomEnemy = false;    
        public int totalEnemy;               
        public bool randomTime;             
        public float timeSpawn;                    
        public Vector2 timeRange;              
        public bool isBoss;

        public float GetTimeSpawn()
        {
            if (randomTime)
            {
                return Random.Range(timeRange.x, timeRange.y);
            }
            else
            {
                return timeSpawn;
            }
        }
    }
}
