using UDEV.ChickenMerge;
using UnityEngine;

namespace UDEV
{
    public class GameConfigSO : ScriptableObject
    {
        [Header("Common:")]
        [Tooltip("All levels and guns will be unlock. You have 1M coins.")]
        public bool testingModeEnable;
        public int startingCoin;
        [Header("Level Bonus:")]
        public int minLevelBonus;
        public int maxLevelBonus;

        [Header("Game Chest Settings:")]
        [Tooltip("Multiply by Game Level")]
        public int openChestPrice;

        public int LevelBonus
        {
            get => Random.Range(minLevelBonus, maxLevelBonus) * (UserDataHandler.Ins.curLevelId + 1);
        }
    }
}
