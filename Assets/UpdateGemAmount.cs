using UDEV.ChickenMerge;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGemAmount : MonoBehaviour
{

    public Text gemAmountValueText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gemAmountValueText.text = UserDataHandler.Ins.gems.ToString();
    }
}
