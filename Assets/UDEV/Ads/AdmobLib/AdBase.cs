using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV
{
    public class AdBase : MonoBehaviour
    {
        protected AdsController m_adCtr;
        protected AdConfig m_adConfig;

        public virtual void LoadAd(AdsController adCtr = null)
        {
            m_adCtr = adCtr;
            m_adConfig = adCtr.config;
        }
        public virtual void ShowAd()
        {

        }
        public virtual void HideAd()
        {

        }
        public virtual void DestroyAd()
        {

        }

        public virtual void LogResponseInfo()
        {

        }
    }
}
