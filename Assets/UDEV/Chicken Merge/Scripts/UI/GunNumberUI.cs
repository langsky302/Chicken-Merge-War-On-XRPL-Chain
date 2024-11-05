using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class GunNumberUI : MonoBehaviour
    {
        public Canvas canvas;
        [SerializeField] private Text m_numberTxt;

        public void SetNumber(int number)
        {
            if(m_numberTxt)
                m_numberTxt.text = number.ToString("00");
        }
    }
}
