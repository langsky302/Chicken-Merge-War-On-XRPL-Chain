using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class LevelItemUI : MonoBehaviour
    {
        [SerializeField] Text m_levelCountingTxt;
        [SerializeField] Image m_lockIcon;
        public Button levelBtn;

        public void UpdateUI(int levelId)
        {
            var levelUserData = UserDataHandler.Ins.GetLevelData(levelId);
            if (levelUserData == null) return;

            if (m_levelCountingTxt)
            {
                m_levelCountingTxt.text = (levelId + 1).ToString("00");
                m_levelCountingTxt.gameObject.SetActive(levelUserData.isUnlocked);
            }

            if(m_lockIcon)
                m_lockIcon.gameObject.SetActive(!levelUserData.isUnlocked);
        }
    }
}
