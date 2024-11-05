using System.Linq;

namespace UDEV.ChickenMerge
{
    public class BoosterManager : Singleton<BoosterManager>
    {
        public BoosterController[] boosterControllers;

        public BoosterController GetBoosterCtr(BoosterType type)
        {
            var findeds = boosterControllers.Where(b => b.type == type).ToArray();

            if (findeds == null || findeds.Length <= 0) return null;

            return findeds[0];
        }

        public BoosterController[] GetActiveBoosterCtrs()
        {
            var findeds = boosterControllers.Where(b => b.IsActive).ToArray();
            return findeds;
        }

        public bool IsBoosterActive(BoosterType type)
        {
            var boosterCtr = GetBoosterCtr(type);

            if(boosterCtr == null) return false;

            return boosterCtr.IsActive;
        }

        public bool IsHaveBoosterActive()
        {
            if(boosterControllers == null || boosterControllers.Length <= 0) return false;

            for (var i = 0; i < boosterControllers.Length; i++)
            {
                var boosterCtr = boosterControllers[i];
                if (boosterCtr == null) continue;
                if(boosterCtr.IsActive) return true;
            }
            return false;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            var currentBoosterCtrs = GetComponentsInChildren<BoosterController>();

            boosterControllers = currentBoosterCtrs;
        }
#endif
    }
}
