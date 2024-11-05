using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class SkillCollectedSO : ScriptableObject
    {
        public float timeTrigger;
        public float cooldownTime;
        public int capacity;
        public Sprite skillIcon;
        public AudioClip triggerSoundFx;
    }
}
