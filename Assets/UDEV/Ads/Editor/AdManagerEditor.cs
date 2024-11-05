using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UDEV
{
    [CustomEditor(typeof(AdManager))]
    public class AdManagerEditor : Editor
    {
        private AdManager m_target;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            m_target = (AdManager)target;

            switch (m_target.adsPlatform) {
                case AdsPlatform.Admob:

                    AdmobController admobController = Resources.Load<AdmobController>("AdsControllers/AdmobController");
                    if (admobController)
                    {
                        m_target.adsController = admobController;
                    }
                    break;
                case AdsPlatform.UnityAds:
                    UnityAdsController unityAdsController = Resources.Load<UnityAdsController>("AdsControllers/UnityAdController");
                    if (unityAdsController)
                    {
                        m_target.adsController = unityAdsController;
                    }
                    break;
            }
        }
    }
}
