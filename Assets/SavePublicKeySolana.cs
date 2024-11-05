//using Solana.Unity.SDK;
//using Solana.Unity.Wallet;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class SavePublicKeySolana : MonoBehaviour
//{
//    private void OnEnable()
//    {
//        Web3.OnLogin += OnLogin;
//    }

//    private void OnDisable()
//    {
//        Web3.OnLogin -= OnLogin;
//    }

//    private void OnLogin(Account account)
//    {
//        // Lưu PublicKey vào PlayerPrefs

//        PlayerPrefs.SetString("PublicKey", account.PublicKey);
//        PlayerPrefs.Save();

//        // Chuyển sang scene mới (ShopAndPlay)
//        SceneManager.LoadScene("ShopAndPlay");
//    }
//}
