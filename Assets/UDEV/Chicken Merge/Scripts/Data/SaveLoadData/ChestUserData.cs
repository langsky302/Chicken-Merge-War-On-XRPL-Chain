using System.Collections.Generic;

namespace UDEV.ChickenMerge
{
    [System.Serializable]
    public class ChestUserData
    {
        public int openPrice;
        public int coinsBonus;
        public List<SkillBonusUserData> skillBonusList = new List<SkillBonusUserData>();
    }

    [System.Serializable]
    public class SkillBonusUserData
    {
        public SkillCollectedType type;
        public int bonusAmount;
    }
}
