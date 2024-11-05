using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class RainOfBulletSO : SkillCollectedSO
    {
        public int numberOfBullet;
        public int damage;
        [Range(0f , 1f)]
        public float damageMissRange;
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string bulletPoolKey;
    }
}
