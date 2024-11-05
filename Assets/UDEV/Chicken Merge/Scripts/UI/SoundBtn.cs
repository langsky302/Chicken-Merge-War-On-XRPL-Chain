using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class SoundBtn : MonoBehaviour
    {
        private Button m_btn;

        private void Awake()
        {
            m_btn = GetComponent<Button>();
        }

        private void Start()
        {
            if (m_btn == null) return;
            m_btn.onClick.AddListener(() => PlaySound());
        }

        private void PlaySound()
        {
            AudioController.Ins.PlaySound(AudioBase.Ins.data.btnClick);
        }
    }
}
