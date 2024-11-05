using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class AudioSO : ScriptableObject
    {
        [Header("Game sounds and musics: ")]
        public AudioClip gameOver;
        public AudioClip levelCompleted;
        public AudioClip upgradeSuccess;
        public AudioClip buySuccess;
        public AudioClip buyBooster;
        public AudioClip gunUnlocked;
        public AudioClip reward;
        public AudioClip btnClick;
        public AudioClip[] bgms;
        public AudioClip[] menus;
    }

}