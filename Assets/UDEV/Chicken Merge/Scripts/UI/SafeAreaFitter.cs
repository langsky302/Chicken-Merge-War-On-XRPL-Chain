using UnityEngine;

namespace UDEV
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        private void Awake()
        {
            var rectComp = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rectComp.anchorMin = anchorMin;
            rectComp.anchorMax = anchorMax;
        }
    }

}