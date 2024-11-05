using UnityEngine;

namespace UDEV.ChickenMerge
{
    [RequireComponent(typeof(BundleReward))]
    public class ChestCollectable : Collectable
    {
        private BundleReward m_bundleReward;

        private void Awake()
        {
            m_bundleReward = GetComponent<BundleReward>();
        }

        public override void Init(int bonusMultier = 1)
        {
            base.Init(bonusMultier);
            Trigger();
        }

        protected override void TriggerHandle()
        {
            m_bundleReward?.GetReward(m_bonusMultier);
        }
    }
}
