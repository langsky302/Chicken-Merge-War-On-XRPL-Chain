using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LimitAreaSceneFitter : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        private Vector2 m_camSize;

        private void Start()
        {
            m_camSize = Helper.Get2DCamSize();
            Vector2 positionFitter = new Vector2(-m_camSize.x / 2, 0f) + offset;
            transform.position = positionFitter;
        }
    }
}
