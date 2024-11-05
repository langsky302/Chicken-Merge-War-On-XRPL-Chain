using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class SkillShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image m_previewIcon;
        [SerializeField] private Transform m_starGrid;
        [SerializeField] private StarItemUI m_starPrefab;
        [SerializeField] private Text m_levelCountingTxt;
        [SerializeField] private Text descriptionTxt;
        [SerializeField] private Text m_upgradePriceTxt;
        public Button upgradeBtn;

        public void UpdateUI(PassiveSkillSO skillStat)
        {
            if (skillStat == null) return;


            UpdateStars(skillStat);

            if (m_levelCountingTxt)
                m_levelCountingTxt.text = $"LEVEL {skillStat.level}";

            if (m_previewIcon)
                m_previewIcon.sprite = skillStat.thumb;

            if(descriptionTxt)
                descriptionTxt.text = skillStat.description + $" {skillStat.bonusRate * 100}%";

            if(m_upgradePriceTxt)
            {
                if (skillStat.IsMaxLevel())
                {
                    m_upgradePriceTxt.text = "Max";
                }
                else
                {
                    m_upgradePriceTxt.text = Helper.BigCurrencyFormat(skillStat.upgradePrice);
                }
            }
        }

        private void UpdateStars(PassiveSkillSO skillStat)
        {
            Helper.ClearChilds(m_starGrid);
            if (skillStat == null || !m_starPrefab) return;

            for (int i = 0; i < skillStat.maxLevel; i++)
            {
                var starClone = Instantiate(m_starPrefab);
                Helper.AssignToRoot(m_starGrid, starClone.transform, Vector3.zero, Vector3.one);
                starClone.ActiveStar(skillStat.level >= (i + 1));
            }
        }
    }

}