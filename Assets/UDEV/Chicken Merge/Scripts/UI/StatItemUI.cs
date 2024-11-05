using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class StatItemUI : MonoBehaviour
    {
        [SerializeField] private Text m_oldStatTxt;
        [SerializeField] private Text m_upValueTxt;

        public void UpdateStatUI(string oldStat, string upValue)
        {
            if (m_oldStatTxt)
            {
                m_oldStatTxt.text = oldStat;
            }

            if (m_upValueTxt)
            {
                m_upValueTxt.text = upValue;
            }
        }
    }
}
