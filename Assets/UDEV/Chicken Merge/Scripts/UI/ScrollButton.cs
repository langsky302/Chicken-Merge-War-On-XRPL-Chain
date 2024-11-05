using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UDEV.ChickenMerge
{
    public class ScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool m_isPointerDown;

        public bool IsPointerDown { get => m_isPointerDown;}

        public void OnPointerDown(PointerEventData eventData)
        {
            m_isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_isPointerDown = false;
        }
    }
}
