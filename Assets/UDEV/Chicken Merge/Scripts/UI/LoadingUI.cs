using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField] private Image m_slider;
        [SerializeField] private Text m_progressTxt;

        public void UpdateUI(float loadingProgress)
        {
            if (m_slider)
            {
                m_slider.fillAmount = loadingProgress;
            }

            if (m_progressTxt)
            {
                m_progressTxt.text = $"Loading...{(loadingProgress * 100).ToString("f0")}%";
            }
        }
    }
}
