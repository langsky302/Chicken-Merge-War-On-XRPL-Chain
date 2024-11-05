using TMPro;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class ClaimLevelBonusDialog : Dialog
    {
        [SerializeField] private Text m_bonusAmountTxt;

        public override void Show()
        {
            base.Show();
            gemAmountValueText.text = UserDataHandler.Ins.gems.ToString();
            title.SetText($"LEVEL {UserDataHandler.Ins.curLevelId} CLEARED");
            if(m_bonusAmountTxt)
            {
                m_bonusAmountTxt.text = Helper.BigCurrencyFormat(GameController.Ins.LevelCoinBonus);
            }

            //AdsController.Ins.OnUserReward.AddListener(ClaimDoubleEvent);
        }

        public void ClaimDouble()
        {
            //AdsController.Ins.ShowRewardedVideo(); 
        }

        private void ClaimDoubleEvent()
        {
            ClaimHandle(2);
        }

        public void ClaimBtnEvent()
        {
            ClaimHandle();
        }

        private void ClaimHandle(int multier = 1)
        {
            Debug.Log("A");
            AutoSpawnGun AutoSpawnGunReference = GameObject.Find("AutoSpawnGun").GetComponent<AutoSpawnGun>();
            Debug.Log("B");
            AutoSpawnGunReference.TurnOffAutoSpawn();
            Debug.Log("C");
            GameController.Ins.ClaimLevelBonus(multier);
            Debug.Log("D");
            RewardDialog rewardDialog = (RewardDialog)DialogDB.Ins.GetDialog(DialogType.Reward);
            Debug.Log("F");
            if (rewardDialog)
            {
                Debug.Log("G");
                rewardDialog.AddCoinRewardItem(GameController.Ins.LevelCoinBonus *  multier);
                Debug.Log("H");
                DialogDB.Ins.Show(rewardDialog);
                Debug.Log("I");
                rewardDialog.onDialogCompleteClosed = () =>
                {
                    Debug.Log("J");
                    DialogDB.Ins.Show(DialogType.LevelCompleted);
                };
                Debug.Log("K");
                GameController.Ins.CancelInvoke();
            }
        }

        private void OnDisable()
        {
            AdsController.Ins.OnUserReward.RemoveListener(ClaimDoubleEvent);
        }

        public TMP_Text gemAmountValueText;
        public TMP_Text gemNotificationText;

        public void ClaimUsingGems()
        {
            int gems = PlayerPrefs.GetInt("gems", 0);

            if (gems < 1)
            {
                // Không đủ Gems.
                // Hiện thông báo
                gemNotificationText.gameObject.SetActive(true);
            }
            else
            {
                // Đủ Gems.
                // Trừ Gems
                // Lưu Gems lên hệ thống.
                // Cập nhật số Gems 

                UserDataHandler.Ins.gems -= 1;
                PlayerPrefs.SetInt("gems", UserDataHandler.Ins.gems);
                PlayerPrefs.Save(); // Đảm bảo lưu lại ngay lập tức
                gemAmountValueText.text = UserDataHandler.Ins.gems.ToString();
                ClaimDoubleEvent();
                return;
            }
        }
    }
}
