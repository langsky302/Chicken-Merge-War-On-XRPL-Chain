using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class AutoSpawnGunBtn : MonoBehaviour
    {
        [SerializeField] private Image m_btnBG;
        [SerializeField] private Image m_previewIcon;
        [SerializeField] private Image m_cooldownImage;
        [SerializeField] private Sprite m_activeBtnBgSp;
        [SerializeField] private Sprite m_inactiveBtnBgSp;

        public void UpdateUI(bool isActive, Sprite previewIcon)
        {
            if (m_btnBG)
            {
                m_btnBG.sprite = isActive ? m_activeBtnBgSp : m_inactiveBtnBgSp;
            }

            if(m_cooldownImage)
            {
                m_cooldownImage.gameObject.SetActive(isActive);
            }

            if(m_previewIcon != null && previewIcon != null)
            {
                m_previewIcon.sprite = previewIcon;
            }
        }

        public void UpdateCooldown(float progress)
        {
            m_cooldownImage.fillAmount = progress;
        }
    }
}
