using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class VFXController : MonoBehaviour
    {
        [SerializeField] private bool m_useCamShake;
        [SerializeField] private float m_shakeDur = 0f;
        [SerializeField] private float m_shakeFreq = 0f;
        [SerializeField] private float m_shakeAmp = 1f;

        private AudioSource m_aus;
        [SerializeField] private AudioClip[] m_sounds;

        private void Awake()
        {
            m_aus = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            AudioController.Ins?.PlaySound(m_sounds, m_aus);
            CameraShakeVfx();
        }

        private void CameraShakeVfx()
        {
            if (!m_useCamShake) return;
            CamShake.Ins?.ShakeTrigger(m_shakeDur, m_shakeFreq, m_shakeAmp);
        }
    }
}
