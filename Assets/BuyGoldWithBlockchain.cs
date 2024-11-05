using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.ChickenMerge;
using UDEV.DMTool;
using System.Threading;
using UnityEngine.UI;

namespace UDEV.ChickenMerge {
    public class BuyGoldWithBlockchain : MonoBehaviour
    {
        //private ThirdwebSDK sdk;
        //private string gold50ContractAddress = "0x6e5800Dc44dfA6862BB69a1534B35b225cfB2Dd6";
        //private string gold1000ContractAddress = "0x6e5800Dc44dfA6862BB69a1534B35b225cfB2Dd6";
        //private string gold5000ContractAddress = "0x6e5800Dc44dfA6862BB69a1534B35b225cfB2Dd6";
        //private string gold10000ContractAddress = "0x6e5800Dc44dfA6862BB69a1534B35b225cfB2Dd6";

        //private string m_buyingMessage;

        //public Button ClaimGold50Tokenbtn;
        //public Text ClaimGold50TokenbtnTxt;
        //public Button ClaimGold1000Tokenbtn;
        //public Text ClaimGold1000TokenbtnTxt;
        //public Button ClaimGold5000Tokenbtn;
        //public Text ClaimGold5000TokenbtnTxt;
        //public Button ClaimGold10000Tokenbtn;
        //public Text ClaimGold10000TokenbtnTxt;

        //private void Start()
        //{
        //    sdk = ThirdwebManager.Instance.SDK;
        //}

        //private void HideAllClaimBtn() {
        //    ClaimGold50Tokenbtn.interactable = false;
        //    ClaimGold1000Tokenbtn.interactable = false;
        //    ClaimGold5000Tokenbtn.interactable = false;
        //    ClaimGold10000Tokenbtn.interactable = false;
        //}
        //private void ShowAllClaimBtn()
        //{
        //    ClaimGold50Tokenbtn.interactable = true;
        //    ClaimGold1000Tokenbtn.interactable = true;
        //    ClaimGold5000Tokenbtn.interactable = true;
        //    ClaimGold10000Tokenbtn.interactable = true;
        //}
        //private void ResetTextToNormal()
        //{
        //    ClaimGold50TokenbtnTxt.text = "Claim Free";
        //    ClaimGold1000TokenbtnTxt.text = "0.001 BNB";
        //    ClaimGold5000TokenbtnTxt.text = "0.002 BNB";
        //    ClaimGold10000TokenbtnTxt.text = "0.004 BNB";
        //}

        //public async void ClaimGold50Token()
        //{
        //    HideAllClaimBtn();
        //    ClaimGold50TokenbtnTxt.text = "Claiming...";
        //    Contract contract = sdk.GetContract(gold50ContractAddress);
        //    var data = await contract.ERC20.Claim("1");
        //    Debug.Log("50 Gold were claimed!");
        //    BuyItem(100);
        //}

        //public async void ClaimGold1000Token()
        //{
        //    HideAllClaimBtn();
        //    ClaimGold1000TokenbtnTxt.text = "Claiming...";
        //    Contract contract = sdk.GetContract(gold1000ContractAddress);
        //    var data = await contract.ERC20.Claim("1");
        //    Debug.Log("1000 Gold were claimed!");
        //    BuyItem(1000);
        //}

        //public async void ClaimGold5000Token()
        //{
        //    HideAllClaimBtn();
        //    ClaimGold5000TokenbtnTxt.text = "Claiming...";
        //    Contract contract = sdk.GetContract(gold5000ContractAddress);
        //    var data = await contract.ERC20.Claim("1");
        //    Debug.Log("5000 Gold were claimed!");
        //    BuyItem(5000);
        //}

        //public async void ClaimGold10000Token()
        //{
        //    HideAllClaimBtn();
        //    ClaimGold10000TokenbtnTxt.text = "Claiming...";
        //    Contract contract = sdk.GetContract(gold10000ContractAddress);
        //    var data = await contract.ERC20.Claim("1");
        //    Debug.Log("10000 Gold were claimed!");
        //    BuyItem(10000);
        //}

        private void BuyItem(int coinValue)
        {
            UserDataHandler.Ins.coin += coinValue;
            UserDataHandler.Ins.SaveData();

            DialogDB.Ins.current.Close();
            RewardDialog rewardDialog = (RewardDialog)DialogDB.Ins.GetDialog(DialogType.Reward);

            if (rewardDialog)
            {
                rewardDialog.AddCoinRewardItem(coinValue);
                DialogDB.Ins.Show(rewardDialog);
            }
            //ShowAllClaimBtn();
            //ResetTextToNormal();
        }
    }
}



