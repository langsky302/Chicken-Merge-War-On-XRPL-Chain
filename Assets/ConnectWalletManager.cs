using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class ConnectWalletManager : MonoBehaviour
{
    public string address { get; private set; }

    public Button tokenGateButton;
    public Button playButton;
    public TMP_Text claimStatusText;

    private string nftAddress = "0xe70EAaf2e307a83c23bd15fd17DfBee3BF7bF484";

    private void Start()
    {
        tokenGateButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        claimStatusText.gameObject.SetActive(false);
    }

    public async void Login()
    {
        address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(nftAddress);
        List<NFT> nftList = await contract.ERC721.GetOwned(address);
        if (nftList.Count == 0)
        {
            tokenGateButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(true);
        }
    }

    public async void ClaimNFTPass()
    {
        address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        claimStatusText.text = "Claiming...";
        claimStatusText.gameObject.SetActive(true);
        tokenGateButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract(nftAddress);
        try
        {
            var result = await contract.ERC721.ClaimTo(address, 1);
            claimStatusText.text = "Claimed NFT Pass!";
            tokenGateButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }
        catch (Exception ex)
        {
            claimStatusText.text = "Failed to claim NFT Pass: " + ex.Message;
            tokenGateButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("ShopAndPlay");
    }
}