using TMPro;
using UnityEngine;

public class UsePublicKey : MonoBehaviour
{
    // Tham chiếu tới TextMeshProUGUI
    public TextMeshProUGUI PublicKeyText;

    private void Start()
    {
        // Lấy PublicKey từ PlayerPrefs
        string publicKey = PlayerPrefs.GetString("PublicKey", "Không có khóa công khai");

        // Hiển thị public key trên Text label
        PublicKeyText.text = publicKey;
    }
}
