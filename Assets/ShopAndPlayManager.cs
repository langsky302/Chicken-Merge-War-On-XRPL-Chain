//using Solana.Unity.Metaplex.NFT.Library;
//using Solana.Unity.Metaplex.Utilities;
//using Solana.Unity.Programs;
//using Solana.Unity.Rpc.Builders;
//using Solana.Unity.Rpc.Core.Http;
//using Solana.Unity.Rpc.Models;
//using Solana.Unity.Rpc.Types;
//using Solana.Unity.SDK;
//using Solana.Unity.SDK.Nft;
//using Solana.Unity.Wallet;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class ShopAndPlayManager : MonoBehaviour
//{
//    //Game Shop
//    public Button gold5kButton;
//    public Button gem10Button;
//    public Button gold20kButton;
//    public Button gem30Button;
//    public Button gold50kButton;
//    public Button gem80Button;
//    public Button backBtn;

//    // To-do Declare all NFT Buttons
//    public Button rookieNFTBtn;
//    public Button challengerNFTBtn;
//    public Button eliteNFTBtn;
//    public Button legendNFTBtn;


//    public TextMeshProUGUI coinBoughtText;
//    public TextMeshProUGUI gemBoughtText;
//    public TextMeshProUGUI buyingStatusText;

//    private const long SolLamports = 1000000000;

//    private void Start()
//    {
//        ShowAndHideNFTButton();
//    }

//    private void ShowAndHideNFTButton() {
//        int nextLevelId = PlayerPrefs.GetInt("NextLevelId", 1); // 1 là giá trị mặc định nếu không có giá trị nào được lưu
//        Debug.Log("nextLevelId: " + nextLevelId);

//        if (nextLevelId < 3) {
//            rookieNFTBtn.gameObject.SetActive(true);
//            challengerNFTBtn.gameObject.SetActive(false);
//            eliteNFTBtn.gameObject.SetActive(false);
//            legendNFTBtn.gameObject.SetActive(false);
//        }
//        else if (nextLevelId >= 3 && nextLevelId < 10) {
//            rookieNFTBtn.gameObject.SetActive(true);
//            challengerNFTBtn.gameObject.SetActive(true);
//            eliteNFTBtn.gameObject.SetActive(false);
//            legendNFTBtn.gameObject.SetActive(false);
//        }
//        else if (nextLevelId >= 10 && nextLevelId < 20)
//        {
//            rookieNFTBtn.gameObject.SetActive(true);
//            challengerNFTBtn.gameObject.SetActive(true);
//            eliteNFTBtn.gameObject.SetActive(true);
//            legendNFTBtn.gameObject.SetActive(false);
//        }
//        else if (nextLevelId >=20)
//        {
//            rookieNFTBtn.gameObject.SetActive(true);
//            challengerNFTBtn.gameObject.SetActive(true);
//            eliteNFTBtn.gameObject.SetActive(true);
//            legendNFTBtn.gameObject.SetActive(true);
//        }
//    }

//    public void PlayGame()
//    {
//        // Chuyển sang scene mới (ShopAndPlay)
//        SceneManager.LoadScene("Loading");
//    }

//    private void HandleResponse(RequestResult<string> result)
//    {
//        Debug.Log(result.Result == null ? result.Reason : "");
//    }

//    public async void SpendTokenToBuyCoins(int indexValue)
//    {

//        Double _ownedSolAmount = await Web3.Instance.WalletBase.GetBalance();

//        if (_ownedSolAmount <= 0)
//        {
//            buyingStatusText.text = "Not Enough SOL";
//            return;
//        }

//        TurnOffButtons();

//        float costValue = 0f;
//        buyingStatusText.text = "Buying...";
//        buyingStatusText.gameObject.SetActive(true);
//        if (indexValue == 0 || indexValue == 3)
//        {
//            costValue = 0.01f;
//        }
//        else if (indexValue == 1 || indexValue == 4)
//        {
//            costValue = 0.02f;
//        }
//        else if (indexValue == 2 || indexValue == 5)
//        {
//            costValue = 0.04f;
//        }
//        try
//        {
//            // Thực hiện chuyển tiền, nếu thành công thì tiếp tục xử lý giao diện
//            RequestResult<string> result = await Web3.Instance.WalletBase.Transfer(
//               new PublicKey("Hw1VoYsnB7kX5h4nZiczEndj6mMF3i7DZR5Q2Ng1JiM4"),
//               Convert.ToUInt64(costValue * SolLamports));
//            HandleResponse(result);

//            // Chỉ thực hiện các thay đổi giao diện nếu chuyển tiền thành công

//            TurnOnButtons();


//            if (indexValue == 0)
//            {
//                buyingStatusText.text = "+5,000 Golds";
//                gold5kButton.gameObject.SetActive(false);
//                ResourceBoost.Instance.golds += 5000;
//            }
//            else if (indexValue == 1)
//            {
//                buyingStatusText.text = "+20,000 Golds";
//                gold20kButton.gameObject.SetActive(false);
//                ResourceBoost.Instance.golds += 20000;
//            }
//            else if (indexValue == 2)
//            {
//                buyingStatusText.text = "+50,000 Golds";
//                gold50kButton.gameObject.SetActive(false);
//                ResourceBoost.Instance.golds += 50000;
//            }
//            else if (indexValue == 3)
//            {
//                buyingStatusText.text = "+10 Gems";
//                gem10Button.gameObject.SetActive(false);
//                ResourceBoost.Instance.gems += 10;
//            }
//            else if (indexValue == 4)
//            {
//                buyingStatusText.text = "+30 Gems";
//                gem30Button.gameObject.SetActive(false);
//                ResourceBoost.Instance.gems += 30;
//            }
//            else if (indexValue == 5)
//            {
//                buyingStatusText.text = "+80 Gems";
//                gem80Button.gameObject.SetActive(false);
//                ResourceBoost.Instance.gems += 80;
//            }

//            coinBoughtText.text = "Gold Bought: " + ResourceBoost.Instance.golds.ToString();
//            gemBoughtText.text = "Gem Bought: " + ResourceBoost.Instance.gems.ToString();

//        }
//        catch (Exception ex)
//        {
//            // Xử lý ngoại lệ nếu có lỗi xảy ra
//            Debug.LogError($"Lỗi khi thực hiện chuyển tiền: {ex.Message}");
//        }
//    }

//    private void TurnOffButtons() {
//        // Gold & Gem Buttons disable
//        gold5kButton.interactable = false;
//        gem10Button.interactable = false;
//        gold20kButton.interactable = false;
//        gem30Button.interactable = false;
//        gold50kButton.interactable = false;
//        gem80Button.interactable = false;

//        // Back Buttons disable
//        backBtn.interactable = true;

//        // NFT Buttons disable
//        rookieNFTBtn.interactable = false;
//        challengerNFTBtn.interactable = false;
//        eliteNFTBtn.interactable = false;
//        legendNFTBtn.interactable = false;
//}

//    private void TurnOnButtons()
//    {
//        //Gold & Gem Buttons enable
//        gold5kButton.interactable = true;
//        gem10Button.interactable = true;
//        gold20kButton.interactable = true;
//        gem30Button.interactable = true;
//        gold50kButton.interactable = true;
//        gem80Button.interactable = true;

//        //Back Buttons enable
//        backBtn.interactable = true;

//        // NFT Buttons enable
//        rookieNFTBtn.interactable = true;
//        challengerNFTBtn.interactable = true;
//        eliteNFTBtn.interactable = true;
//        legendNFTBtn.interactable = true;
//    }

//    public async void ClaimGameNFT(int indexValue)
//    {
//        // Mint and ATA
//        var mint = new Account();
//        var associatedTokenAccount = AssociatedTokenAccountProgram
//            .DeriveAssociatedTokenAccount(Web3.Account, mint.PublicKey);

//        // Define the metadata
//        // 4 NFTs here
//        var metadata = new Metadata() { };

//        if (indexValue == 0)
//        {
//            metadata = new Metadata()
//            {
//                name = "Rookie",
//                symbol = "R",

//                // Deployed to Pinata
//                uri = "https://gateway.pinata.cloud/ipfs/QmQoU9ezuvPKtDTzQ7m7EpRFaxCCtdiYRqh1P8wYAMzzaM",
//                sellerFeeBasisPoints = 0,
//                creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
//            };
//        }
//        else if (indexValue == 1)
//        {
//            metadata = new Metadata()
//            {
//                name = "Challenger",
//                symbol = "C",

//                // Deployed to Pinata
//                uri = "https://gateway.pinata.cloud/ipfs/QmSEX5uq27nAXpF7FNG9bwTKQ8pQiWRb67c9QNgtojbrBc",
//                sellerFeeBasisPoints = 0,
//                creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
//            };
//        }
//        else if (indexValue == 2)
//        {
//            metadata = new Metadata()
//            {
//                name = "Elite",
//                symbol = "E",

//                // Deployed to Pinata
//                uri = "https://gateway.pinata.cloud/ipfs/QmfAe3ZkWwscKXEfgBye8XWWc5a919xcRe5fE31tnUTPux",
//                sellerFeeBasisPoints = 0,
//                creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
//            };
//        }
//        else
//        {
//            metadata = new Metadata()
//            {
//                name = "Legend",
//                symbol = "L",

//                // Deployed to Pinata
//                uri = "https://gateway.pinata.cloud/ipfs/QmYsYAptXkjQnmgMhWv5s8VCk8RJyMzsmrphV3TUE7taFc",
//                sellerFeeBasisPoints = 0,
//                creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
//            };
//        }

//        // Check if Player already Owned the NFT
//        var tokenAccounts = await Web3.Wallet.GetTokenAccounts(Commitment.Processed);
//        int matchingNftCount = 0;
//        string checkTextValue = "";

//        if (indexValue == 0)
//        {
//            checkTextValue = "Rookie";
//        }
//        else if (indexValue == 1)
//        {
//            checkTextValue = "Challenger";
//        }
//        else if (indexValue == 2)
//        {
//            checkTextValue = "Elite";
//        }
//        else if (indexValue == 3)
//        {
//            checkTextValue = "Legend";
//        }

//        foreach (var item in tokenAccounts)
//        {
//            var loadTask = Nft.TryGetNftData(item.Account.Data.Parsed.Info.Mint,
//            Web3.Instance.WalletBase.ActiveRpcClient, commitment: Commitment.Processed);
//            var nftData = await loadTask;
//            if (nftData != null)
//            {
//                Debug.Log($"NFT Mint: {nftData}");
//                string textValue = nftData.metaplexData?.data?.offchainData?.name;
//                Debug.Log("textValue: " + textValue);
//                if (textValue == checkTextValue)
//                {
//                    matchingNftCount += 1;
//                    break;
//                }
//            }
//            else
//            {
//                Debug.Log($"Can not find NFT Data: {item.Account.Data.Parsed.Info.Mint}");
//            }
//        }


//        // If already have then notify the Player
//        if (matchingNftCount >= 1)
//        {
//            buyingStatusText.text = "You already own this NFT.";
//            buyingStatusText.gameObject.SetActive(true);

//            TurnOnButtons();

//            // Hide the NFT Buttons.
//            if (indexValue == 0) {
//                rookieNFTBtn.gameObject.SetActive(false);
//            }
//            else if (indexValue == 1) {
//                challengerNFTBtn.gameObject.SetActive(false);
//            }
//            else if (indexValue == 2)
//            {
//                eliteNFTBtn.gameObject.SetActive(false);
//            }
//            else if (indexValue == 3)
//            {
//                legendNFTBtn.gameObject.SetActive(false);
//            }

//            return;
//        }

//        TurnOffButtons();

//        buyingStatusText.text = "Claiming...";
//        buyingStatusText.gameObject.SetActive(true);

//        // Prepare the transaction
//        var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();
//        var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize);
//        var transaction = new TransactionBuilder()
//            .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
//            .SetFeePayer(Web3.Account)
//            .AddInstruction(
//                SystemProgram.CreateAccount(
//                    Web3.Account,
//                    mint.PublicKey,
//                    minimumRent.Result,
//                    TokenProgram.MintAccountDataSize,
//                    TokenProgram.ProgramIdKey))
//            .AddInstruction(
//                TokenProgram.InitializeMint(
//                    mint.PublicKey,
//                    0,
//                    Web3.Account,
//                    Web3.Account))
//            .AddInstruction(
//                AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
//                    Web3.Account,
//                    Web3.Account,
//                    mint.PublicKey))
//            .AddInstruction(
//                TokenProgram.MintTo(
//                    mint.PublicKey,
//                    associatedTokenAccount,
//                    1,
//                    Web3.Account))
//            .AddInstruction(MetadataProgram.CreateMetadataAccount(
//                PDALookup.FindMetadataPDA(mint),
//                mint.PublicKey,
//                Web3.Account,
//                Web3.Account,
//                Web3.Account.PublicKey,
//                metadata,
//                TokenStandard.NonFungible,
//                true,
//                true,
//                null,
//                metadataVersion: MetadataVersion.V3))
//            .AddInstruction(MetadataProgram.CreateMasterEdition(
//                    maxSupply: null,
//                    masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
//                    mintKey: mint,
//                    updateAuthorityKey: Web3.Account,
//                    mintAuthority: Web3.Account,
//                    payer: Web3.Account,
//                    metadataKey: PDALookup.FindMetadataPDA(mint),
//                    version: CreateMasterEditionVersion.V3
//                )
//            );
//        var tx = Transaction.Deserialize(transaction.Build(new List<Account> { Web3.Account, mint }));

//        // Sign and Send the transaction
//        try
//        {
//            var res = await Web3.Wallet.SignAndSendTransaction(tx);
//            // Show Confirmation
//            if (res?.Result != null)
//            {
//                await Web3.Rpc.ConfirmTransaction(res.Result, Commitment.Confirmed);
//                Debug.Log("Minting succeeded, see transaction at https://explorer.solana.com/tx/"
//                          + res.Result + "?cluster=" + Web3.Wallet.RpcCluster.ToString().ToLower());
//            }


//            buyingStatusText.text = "NFT Claimed";
//            buyingStatusText.gameObject.SetActive(true);

//            TurnOnButtons();

//            // Hide the NFT Buttons.
//            if (indexValue == 0)
//            {
//                rookieNFTBtn.gameObject.SetActive(false);
//            }
//            else if (indexValue == 1)
//            {
//                challengerNFTBtn.gameObject.SetActive(false);
//            }
//            else if (indexValue == 2)
//            {
//                eliteNFTBtn.gameObject.SetActive(false);
//            }
//            else if (indexValue == 3)
//            {
//                legendNFTBtn.gameObject.SetActive(false);
//            }

//        }
//        catch (Exception ex)
//        {
//            buyingStatusText.text = $"Failed to claim NFT: {ex.Message}";
//            buyingStatusText.gameObject.SetActive(true);
//            Debug.LogError("Error while claiming NFT: " + ex);

//            TurnOnButtons();

//            // Not Hide NFT Buttons

//        }
//    }
//}
