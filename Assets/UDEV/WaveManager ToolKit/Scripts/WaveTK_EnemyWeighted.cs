using UDEV.SPM;

namespace UDEV.WaveManagerToolkit
{
    [System.Serializable]
    public class WaveTK_EnemyWeighted
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string enemyPoolKey;
        public int weight;
    }
}
