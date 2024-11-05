using UDEV.DMTool;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class IAPHandle : MonoBehaviour
    {
        private string m_buyingMessage;

        private void Start()
        {
#if USE_IAP
            IAPManager.Ins?.OnProccessing.RemoveAllListeners();
            IAPManager.Ins?.OnAdsBuying.RemoveAllListeners();
            IAPManager.Ins?.OnItemBuying.RemoveAllListeners();
            IAPManager.Ins?.OnBuyingFailed.RemoveAllListeners();

            IAPManager.Ins?.OnAdsBuying.AddListener(BuyNoAds);
            IAPManager.Ins?.OnItemBuying.AddListener((IAPItem item) => BuyItem(item));
            IAPManager.Ins?.OnBuyingFailed.AddListener((string reason) => BuyFailed(reason));
            IAPManager.Ins?.OnProccessing.AddListener(Proccessing);
#endif
        }

        private void BuyNoAds()
        {
            UserDataHandler.Ins.setting.isNoAds = true;
            UserDataHandler.Ins.SaveData();
            m_buyingMessage = "Ads remove successful!.Please restart game.";

            DialogDB.Ins.current.Close();

            Timer.Schedule(this, 0.5f, () =>
            {
                Dialog messDialog = DialogDB.Ins.GetDialog(DialogType.Message);

                if (messDialog)
                {
                    messDialog.message.text = m_buyingMessage;
                    DialogDB.Ins.Show(messDialog);
                }
            });
        }

        private void BuyItem(IAPItem buyingItem)
        {
            if (buyingItem == null) return;

            m_buyingMessage = buyingItem.amount.ToString("n0") + " Coins";
            UserDataHandler.Ins.coin += buyingItem.amount;
            UserDataHandler.Ins.SaveData();

            DialogDB.Ins.current.Close();

            Timer.Schedule(this, 0.5f, () =>
            {
                RewardDialog rewardDialog = (RewardDialog)DialogDB.Ins.GetDialog(DialogType.Reward);

                if (rewardDialog)
                {
                    rewardDialog.AddCoinRewardItem(buyingItem.amount);
                    DialogDB.Ins.Show(rewardDialog);
                }
            });
        }

        private void BuyFailed(string reason)
        {
            DialogDB.Ins.current.Close();

            Dialog messDialog = DialogDB.Ins.GetDialog(DialogType.Message);


            if (messDialog)
            {
                messDialog.message.text = reason;
                DialogDB.Ins.Show(messDialog);
            }
        }

        private void Proccessing()
        {
            Dialog messDialog = DialogDB.Ins.GetDialog(DialogType.Message);

            if (messDialog)
            {
                messDialog.message.text = "Processing...";
                DialogDB.Ins.Show(messDialog);
            }
        }
    }
}
