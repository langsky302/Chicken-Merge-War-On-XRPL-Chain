using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UDEV.ChickenMerge
{
    public class OpenChestDialog : Dialog
    {
        [SerializeField] private Text m_openPriceTxt;
        [SerializeField] private Text m_coinCountingTxt;
        [SerializeField] private Text m_chestAmountRemainingTxt;
        private ChestController m_chestController;
        private int m_openPrice;

        public TMP_Text gemAmountValueText;
        public TMP_Text gemNotificationText;

        protected override void Awake()
        {
            base.Awake();
            m_chestController = ChestController.Ins;
            UpdateOpenPrice();

            AdsController.Ins.OnUserReward.AddListener(OpenChest);

            gemAmountValueText.text = UserDataHandler.Ins.gems.ToString();
        }

        private void UpdateOpenPrice()
        {
            if (m_chestController.CurrentChest == null) return;
            m_openPrice = m_chestController.CurrentChest.openPrice;
        }

        public override void Show()
        {
            base.Show();
            if (m_openPriceTxt)
            {
                m_openPriceTxt.text = Helper.BigCurrencyFormat(m_openPrice);
            }

            if (m_coinCountingTxt)
                m_coinCountingTxt.text = Helper.BigCurrencyFormat(UserDataHandler.Ins.coin);

            if (m_chestAmountRemainingTxt)
                m_chestAmountRemainingTxt.text = "x" + m_chestController.ChestRemaining.ToString("n0");
        }

        private void Update()
        {
            GameController.ChangeState(GameState.Pausing);
        }

        public void OpenUseCoin()
        {
            OpenChestUseCoin();
        }

        public void OpenChestUseAds()
        {
            Debug.Log("OpenChestUseAds");
        }

        private void OpenChestUseCoin()
        {
            if (UserDataHandler.Ins.IsEnoughCoin(m_openPrice))
            {
                UserDataHandler.Ins.coin -= m_openPrice;
                UserDataHandler.Ins.SaveData();

                OpenChest();
                return;
            }
        }

        private void OpenChest()
        {
            m_chestController?.OpenChest();
            var openChestButton = FindObjectOfType<OpenChestButton>();
            if (openChestButton != null)
            {
                openChestButton.UpdateUI();
            }

            UpdateOpenPrice();
        }

        private void OnDestroy()
        {
            AdsController.Ins.OnUserReward.RemoveListener(OpenChest);
            GameController.ChangeState(GameState.Playing);
        }

        public void OpenChestUseGems()
        {
            int gems = PlayerPrefs.GetInt("gems", 0);

            if (gems < 1)
            {
                // Không đủ Gems.
                // Hiện thông báo
                gemNotificationText.gameObject.SetActive(true);
            }
            else {
                // Đủ Gems.
                // Trừ Gems
                // Lưu Gems lên hệ thống.
                // Cập nhật số Gems 

                UserDataHandler.Ins.gems -= 1;
                PlayerPrefs.SetInt("gems", UserDataHandler.Ins.gems);
                PlayerPrefs.Save(); // Đảm bảo lưu lại ngay lập tức
                gemAmountValueText.text = UserDataHandler.Ins.gems.ToString();
                OpenChest();
                return;
            }
        }
    }
}
