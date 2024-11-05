using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UDEV {
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        public TabContent content;
        public Image tabImage;
        public Text btnText;
        public GameObject tabSelected;
        public GameObject tabInactive;

        TabController m_controller;

        public TabController Controller { get => m_controller; set => m_controller = value; }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            m_controller?.OnTabSelected(this);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            m_controller?.OnTabEnter(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            m_controller?.OnTabExit(this);
        }
    }
}
