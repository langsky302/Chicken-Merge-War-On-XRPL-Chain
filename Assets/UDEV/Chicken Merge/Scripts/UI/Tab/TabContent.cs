using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV
{
    public class TabContent : MonoBehaviour
    {
        TabController m_tabController;

        public TabController TabController { get => m_tabController; set => m_tabController = value; }

        public void Show(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public virtual void LoadContent()
        {

        }
    }
}
