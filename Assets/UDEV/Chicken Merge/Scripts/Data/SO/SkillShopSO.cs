using System.Linq;
using UnityEngine;


namespace UDEV.ChickenMerge
{
    public class SkillShopSO : ScriptableObject
    {
        public PassiveSkillSO[] skillStats;

        public float GetSkillBonus(PassiveSkillType type, float value)
        {
            var findeds = skillStats.Where(s => s.type == type).ToArray();

            if(findeds == null || findeds.Length <= 0) return 0;
            return findeds[0].GetBonusValue(value);
        }
    }
}
