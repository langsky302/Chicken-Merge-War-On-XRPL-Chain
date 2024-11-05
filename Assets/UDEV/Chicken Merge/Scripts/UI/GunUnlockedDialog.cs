using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class GunUnlockedDialog : Dialog
    {
        [SerializeField] private Image m_gunIcon;
        [SerializeField] private Text m_numberTxt;

        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Pausing);
            Invoke("Close", 1.5f);
        }

        public void UpdateUI(Sprite icon, int number)
        {
            if(m_gunIcon)
                m_gunIcon.sprite = icon;

            if(m_numberTxt)
                m_numberTxt.text = number.ToString("00");
        }

        public override void Close()
        {
            base.Close();
            GameController.RevertState();
        }
    }
}
