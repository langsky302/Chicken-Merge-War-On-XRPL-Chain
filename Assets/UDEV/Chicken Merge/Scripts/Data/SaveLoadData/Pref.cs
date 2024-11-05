using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public static class Pref
    {
        public static int bestScore
        {
            set
            {
                int oldScore = PlayerPrefs.GetInt(PrefKey.BestScore.ToString(), 0);

                if (value > oldScore || oldScore == 0)
                {
                    PlayerPrefs.SetInt(PrefKey.BestScore.ToString(), value);
                }
            }

            get => PlayerPrefs.GetInt(PrefKey.BestScore.ToString(), 0);
        }

        public static bool IsFirstTime
        {
            set => SetBool(PrefKey.IsFirstTime.ToString(), value);
            get => GetBool(PrefKey.IsFirstTime.ToString(), true);
        }

        public static int SpriteOrder
        {
            set => PlayerPrefs.SetInt(PrefKey.SpriteOrder.ToString(), value);
            get => PlayerPrefs.GetInt(PrefKey.SpriteOrder.ToString(), 0);
        }

        public static string GameData
        {
            set => PlayerPrefs.SetString(PrefKey.GameData.ToString(), value);
            get => PlayerPrefs.GetString(PrefKey.GameData.ToString());
        }

        public static void SetBool(string key, bool isOn)
        {
            if (isOn)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            else
            {
                PlayerPrefs.SetInt(key, 0);
            }
        }

        public static bool GetBool(string key, bool defaultVal = false)
        {
            return PlayerPrefs.HasKey(key) ?
                PlayerPrefs.GetInt(key) == 1 ? true : false
                : defaultVal;
        }
    }
}
