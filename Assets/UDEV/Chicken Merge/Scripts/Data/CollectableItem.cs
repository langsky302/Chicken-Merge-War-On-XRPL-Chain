using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    [System.Serializable]
    public class CollectableItem
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string poolKey;
        [Range(0f, 1f)]
        public float spawnRate;
    }
}
