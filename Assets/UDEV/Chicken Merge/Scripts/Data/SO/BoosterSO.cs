using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class BoosterSO : ScriptableObject
    {
        public int price;
        public int timeTrigger;
        public Sprite icon;
        [TextArea(10, 10)]
        public string description;
    }
}
