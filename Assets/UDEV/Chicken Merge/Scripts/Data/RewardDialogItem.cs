using UnityEngine;

namespace UDEV.ChickenMerge
{
    [System.Serializable]
    public class RewardDialogItem
    {
        public Sprite icon;
        public int amount;

        public RewardDialogItem()
        {

        }
        public RewardDialogItem(Sprite icon, int amount)
        {
            this.icon = icon;
            this.amount = amount;
        }
    }
}
