using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UDEV.ChickenMerge;
using Aptos.Unity.Rest.Model.Resources;


public class GuessAndPlayManager : MonoBehaviour
{
    public string Address { get; private set; }

    private string smartContract = "0x9D5Ee5A427a18656c7D12f20Cfdf8d4fD3fc4cC3";
    private string abiString = "[{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"_initialNumber\",\"type\":\"int256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"checkGuess\",\"outputs\":[{\"internalType\":\"int256\",\"name\":\"\",\"type\":\"int256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"isGuessingAllowed\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"_guess\",\"type\":\"int256\"}],\"name\":\"setGuess\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"_newNumber\",\"type\":\"int256\"}],\"name\":\"updateOwnerNumber\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]"
;
    private string receiveAddress = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
    private string ownerAddress = "0xAe909F999CE1334eD02d40f0Afb883A967B03DEA";
    public TextMeshProUGUI buyingStatusText;
    public TextMeshProUGUI totalCoinBoughtValueText;

    public TMP_InputField inputField; // Tham chiếu đến Input Field

    public Button startGuessButton;
    public Button setGuessButton;

    private void Start()
    {
        setGuessButton.gameObject.SetActive(false);
        CheckIfOwnerOfContract();
        ShowAndHideNFTButton();
        UpdateAllNFTStatus();
    }

    private void Update()
    {
        totalGoldBoughtText.text = "Gold Bought: " + ResourceBoost.Instance.golds.ToString();
        totalGemBoughtText.text = "Gem Bought: " + ResourceBoost.Instance.gems.ToString();
        totalCoinBoughtValueText.text = ResourceBoost.Instance.golds.ToString();
    }

    public async void CheckIfOwnerOfContract()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        if (Address == ownerAddress)
        {
            setGuessButton.gameObject.SetActive(true);
        }
    }

    public async void CreateRandomNumberOnOasis()
    {
        int valueToCompare = 10; // Initial value to compare

        // Check if the input is empty
        if (string.IsNullOrEmpty(inputField.text))
        {
            buyingStatusText.text = "Input cannot be empty!"; // Notify if the input is empty
            return;
        }
        else if (int.TryParse(inputField.text, out int userInput)) // Check if the input can be parsed to an int
        {
            valueToCompare = userInput; // Assign the input value to valueToCompare
        }
        else
        {
            buyingStatusText.text = "Please enter a valid number!"; // Display a message if the input is not a valid number
        }

        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            smartContract,
            abiString
            );

        startGuessButton.interactable = false;
        setGuessButton.interactable = false;

        try
        {
            // Check if guessing is allowed
            buyingStatusText.text = "Checking...";
            bool isGuessAllowing = await contract.Read<bool>("isGuessingAllowed");

            if (isGuessAllowing)
            {
                try
                {
                    // Send the guess value
                    buyingStatusText.text = "Guessing...";
                    await contract.Write("setGuess", valueToCompare);

                    try
                    {
                        // Check the result of the guess
                        buyingStatusText.text = "Getting Result...";
                        int resultValue = await contract.Read<int>("checkGuess");

                        if (resultValue == 1)
                        {
                            Debug.Log("Correct");
                            buyingStatusText.text = "You are correct. You have received 5000 Gold.";

                            //Trao thưởng
                            ResourceBoost.Instance.golds += 5000;
                        }
                        else
                        {
                            Debug.Log("Wrong");
                            buyingStatusText.text = "You guessed wrong.Please try again.";
                        }
                        startGuessButton.interactable = true;
                        setGuessButton.interactable = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error while checking the guess: {ex.Message}");
                        buyingStatusText.text = $"Error while checking the guess: {ex.Message}";
                        startGuessButton.interactable = true;
                        setGuessButton.interactable = true;
                    }

                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error while setting the guess: {ex.Message}");
                    buyingStatusText.text = $"Error while setting the guess: {ex.Message}";
                    startGuessButton.interactable = true;
                    setGuessButton.interactable = true;
                    return; // Exit if there's an error while writing the guess
                }


            }
            else
            {
                Debug.Log("Guessing is not allowed.");
                buyingStatusText.text = "The number has been guessed correctly. Please wait for a new number.";
                startGuessButton.interactable = true;
                setGuessButton.interactable = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while checking if guessing is allowed: {ex.Message}");
            buyingStatusText.text = $"Error while checking if guessing is allowed: {ex.Message}";
            startGuessButton.interactable = true;
            setGuessButton.interactable = true;
        }

    }

    public async void SetRandomNumberOnOasis()
    {
        int valueToSet = 56; // Initial value to compare

        // Check if the input is empty
        if (string.IsNullOrEmpty(inputField.text))
        {
            buyingStatusText.text = "Input cannot be empty!"; // Notify if the input is empty
            return;
        }
        else if (int.TryParse(inputField.text, out int userInput)) // Check if the input can be parsed to an int
        {
            valueToSet = userInput; // Assign the input value to valueToCompare
        }
        else
        {
            buyingStatusText.text = "Please enter a valid number!"; // Display a message if the input is not a valid number
        }

        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            smartContract,
            abiString
            );

        setGuessButton.interactable = false;
        startGuessButton.interactable = false;

        try
        {
            buyingStatusText.text = "Setting Number...";
            await contract.Write("updateOwnerNumber", valueToSet);

            buyingStatusText.text = "Set Number successfully";
            setGuessButton.interactable = true;
            startGuessButton.interactable = true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while setting number: {ex.Message}");
            buyingStatusText.text = $"Error while setting number: {ex.Message}";
            setGuessButton.interactable = true;
            startGuessButton.interactable = true;
        }
    }

    public Button rookieNFTBtn;
    public Button challengerNFTBtn;
    public Button eliteNFTBtn;
    public Button legendNFTBtn;

    public Button gold5kButton;
    public Button gold20kButton;
    public Button gold50kButton;

    public Button gem10Button;
    public Button gem30Button;
    public Button gem80Button;

    public TMPro.TextMeshProUGUI shoppingStatusText;

    public TMPro.TextMeshProUGUI rookieNFTBtnCostText;
    public TMPro.TextMeshProUGUI challengerNFTBtnCostText;
    public TMPro.TextMeshProUGUI eliteNFTBtnCostText;
    public TMPro.TextMeshProUGUI legendNFTBtnCostText;

    public TMPro.TextMeshProUGUI totalGoldBoughtText;
    public TMPro.TextMeshProUGUI totalGemBoughtText;

    private string rookieAddress = "0x6124133Ea7Bbb732cB2a02db15EBc5c754bFa664";
    private string challengerAddress = "0x805eEE950C2511e560C3Ac97C13a7113D9B9Fac5";
    private string eliteAddress = "0x97b47c92D41008dd5Fd8945874354812624D0FdD";
    private string legendAddress = "0xae07CBeB196f48C9f2E7c578C5d31b3FbA50FF6A";

    private void ShowAndHideNFTButton()
    {
        int nextLevelId = PlayerPrefs.GetInt("NextLevelId", 1); // 1 là giá trị mặc định nếu không có giá trị nào được lưu
        Debug.Log("nextLevelId: " + nextLevelId);

        if (nextLevelId < 3)
        {
            rookieNFTBtn.gameObject.SetActive(true);
            challengerNFTBtn.gameObject.SetActive(false);
            eliteNFTBtn.gameObject.SetActive(false);
            legendNFTBtn.gameObject.SetActive(false);
        }
        else if (nextLevelId >= 3 && nextLevelId < 10)
        {
            rookieNFTBtn.gameObject.SetActive(true);
            challengerNFTBtn.gameObject.SetActive(true);
            eliteNFTBtn.gameObject.SetActive(false);
            legendNFTBtn.gameObject.SetActive(false);
        }
        else if (nextLevelId >= 10 && nextLevelId < 20)
        {
            rookieNFTBtn.gameObject.SetActive(true);
            challengerNFTBtn.gameObject.SetActive(true);
            eliteNFTBtn.gameObject.SetActive(true);
            legendNFTBtn.gameObject.SetActive(false);
        }
        else if (nextLevelId >= 20)
        {
            rookieNFTBtn.gameObject.SetActive(true);
            challengerNFTBtn.gameObject.SetActive(true);
            eliteNFTBtn.gameObject.SetActive(true);
            legendNFTBtn.gameObject.SetActive(true);
        }
    }

    private void HideAllButton()
    {
        rookieNFTBtn.interactable = false;
        challengerNFTBtn.interactable = false;
        eliteNFTBtn.interactable = false;
        legendNFTBtn.interactable = false;

        gold5kButton.interactable = false;
        gold20kButton.interactable = false;
        gold50kButton.interactable = false;

        gem10Button.interactable = false;
        gem30Button.interactable = false;
        gem80Button.interactable = false;
    }
    private void ShowAllButton()
    {
        rookieNFTBtn.interactable = true;
        challengerNFTBtn.interactable = true;
        eliteNFTBtn.interactable = true;
        legendNFTBtn.interactable = true;

        gold5kButton.interactable = true;
        gold20kButton.interactable = true;
        gold50kButton.interactable = true;

        gem10Button.interactable = true;
        gem30Button.interactable = true;
        gem80Button.interactable = true;
    }

    public async void UpdateAllNFTStatus()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();

        //Rookie
        Contract rookieContract = ThirdwebManager.Instance.SDK.GetContract(rookieAddress);
        try
        {
            List<NFT> nftList = await rookieContract.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                rookieNFTBtnCostText.text = "FREE";
            }
            else
            {
                rookieNFTBtnCostText.text = "Owned";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
        }

        //Challenger
        Contract challengerContract = ThirdwebManager.Instance.SDK.GetContract(challengerAddress);
        try
        {
            List<NFT> nftList = await challengerContract.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                challengerNFTBtnCostText.text = "FREE";
            }
            else
            {
                challengerNFTBtnCostText.text = "Owned";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
        }

        //Elite
        Contract eliteContract = ThirdwebManager.Instance.SDK.GetContract(eliteAddress);
        try
        {
            List<NFT> nftList = await eliteContract.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                eliteNFTBtnCostText.text = "FREE";
            }
            else
            {
                eliteNFTBtnCostText.text = "Owned";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
        }

        //Legend
        Contract legendContract = ThirdwebManager.Instance.SDK.GetContract(legendAddress);
        try
        {
            List<NFT> nftList = await legendContract.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                legendNFTBtnCostText.text = "FREE";
            }
            else
            {
                legendNFTBtnCostText.text = "Owned";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
        }
    }

    //Claim NFT
    public async void ClaimRewardNFT(int indexValue)
    {
        string NFTAddressSmartContract = "";
        if (indexValue == 1)
        {
            NFTAddressSmartContract = rookieAddress;
        }
        else if (indexValue == 2)
        {
            NFTAddressSmartContract = challengerAddress;
        }
        else if (indexValue == 3)
        {
            NFTAddressSmartContract = eliteAddress;
        }
        else if (indexValue == 4)
        {
            NFTAddressSmartContract = legendAddress;
        }

        //Check Balance
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        shoppingStatusText.text = "Checking Balance";
        shoppingStatusText.gameObject.SetActive(true);
        try
        {
            HideAllButton();
            List<NFT> nftList = await contract.ERC721.GetOwned(Address);

            if (nftList.Count == 0)
            {
                shoppingStatusText.text = "Claiming...";
                shoppingStatusText.gameObject.SetActive(true);
                try
                {
                    var result = await contract.ERC721.ClaimTo(Address, 1);
                    shoppingStatusText.text = "Claimed NFT Pass!";
                    ShowAllButton();

                    if (indexValue == 1)
                    {
                        rookieNFTBtnCostText.text = "Owned";
                    }
                    else if (indexValue == 2)
                    {
                        challengerNFTBtnCostText.text = "Owned";
                    }
                    else if (indexValue == 3)
                    {
                        eliteNFTBtnCostText.text = "Owned";
                    }
                    else if (indexValue == 4)
                    {
                        legendNFTBtnCostText.text = "Owned";
                    }

                }
                catch (Exception ex)
                {
                    Debug.LogError($"An error occurred while claiming the NFT: {ex.Message}");
                    // Optionally, update the UI to inform the user of the error
                    shoppingStatusText.text = "Failed to claim NFT. Please try again.";
                    ShowAllButton();
                }
            }
            else
            {
                shoppingStatusText.text = "Already Owned this NFT";
                ShowAllButton();
                return;
            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
            ShowAllButton();
        }
    }

    private static float ConvertStringToFloat(string numberStr)
    {
        // Convert the string to a float
        float number = float.Parse(numberStr);

        // Return the float value
        return number;
    }

    public async void SpendTokenToBuyGold(int indexValue)
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        HideAllButton();

        float costValue = 0.1f;

        if (indexValue == 1 || indexValue == 4)
        {
            costValue = 0.1f;
        }
        else if (indexValue == 2 || indexValue == 5)
        {
            costValue = 0.2f;
        }
        else if (indexValue == 3 || indexValue == 6)
        {
            costValue = 0.4f;
        }

        shoppingStatusText.text = "Buying...";
        shoppingStatusText.gameObject.SetActive(true);

        var userBalance = await ThirdwebManager.Instance.SDK.Wallet.GetBalance();
        if (ConvertStringToFloat(userBalance.displayValue) < costValue)
        {
            shoppingStatusText.text = "Not Enough OASIS";
        }
        else
        {
            try
            {
                // Thực hiện chuyển tiền, nếu thành công thì tiếp tục xử lý giao diện
                await ThirdwebManager.Instance.SDK.Wallet.Transfer(receiveAddress, costValue.ToString());

                // Chỉ thực hiện các thay đổi giao diện nếu chuyển tiền thành công

                ShowAllButton();

                if (indexValue == 1)
                {
                    shoppingStatusText.text = "+5,000 GOLD";
                    ResourceBoost.Instance.golds += 5000;
                }
                else if (indexValue == 2)
                {
                    shoppingStatusText.text = "+20,000 GOLD";
                    ResourceBoost.Instance.golds += 20000;
                }
                else if (indexValue == 3)
                {
                    shoppingStatusText.text = "+50,000 GOLD";
                    ResourceBoost.Instance.golds += 50000;
                }
                else if (indexValue == 4)
                {
                    shoppingStatusText.text = "+10 GEM";
                    ResourceBoost.Instance.gems += 10;
                }
                else if (indexValue == 5)
                {
                    shoppingStatusText.text = "+30 GEM";
                    ResourceBoost.Instance.gems += 30;
                }
                else if (indexValue == 6)
                {
                    shoppingStatusText.text = "+80 GEM";
                    ResourceBoost.Instance.gems += 80;
                }

                shoppingStatusText.text = "Purchase Completed";
                shoppingStatusText.gameObject.SetActive(true);

                totalGoldBoughtText.text = "Gold Bought: " + ResourceBoost.Instance.golds.ToString();
                totalGemBoughtText.text = "Gem Bought: " + ResourceBoost.Instance.gems.ToString();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra
                Debug.LogError($"Lỗi khi thực hiện chuyển tiền: {ex.Message}");
                shoppingStatusText.text = "Error. Please try again";
                ShowAllButton();
            }
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene("Loading");
    }
}

