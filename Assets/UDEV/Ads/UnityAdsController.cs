using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USE_UNITY_AD
using UnityEngine.Advertisements;
using UnityEngine.Events;
#endif

namespace UDEV
{
    public class UnityAdsController : AdsController
#if USE_UNITY_AD
        , IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
#endif
    {
        protected UnityAdConfig m_config;
#if USE_UNITY_AD
        public override void Init(UnityAction _OnUserReward = null)
        {
            base.Init(_OnUserReward);
            m_config = (UnityAdConfig) config;
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(m_config.adId.appId.Trim(), m_config.isTestMode, this);
                Advertisement.Load(m_config.adId.bannerAdId.Trim(), this);
                Advertisement.Load(m_config.adId.interstitialAdId.Trim(), this);
                Advertisement.Load(m_config.adId.rewardedAdId.Trim(), this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public override void ShowBanner()
        {
            if (m_isNoAds) return;

            StartCoroutine(ShowBannerWhenInitialized());
        }

        public override void HideBanner()
        {
            if (Advertisement.isInitialized)
            {
                Advertisement.Banner.Hide(true);
            }
        }

        IEnumerator ShowBannerWhenInitialized()
        {
            while (!Advertisement.isInitialized)
            {
                yield return new WaitForSeconds(0.5f);
            }

            Advertisement.Banner.SetPosition(m_config.bannerPosition);
            Advertisement.Banner.Load(m_config.adId.bannerAdId.Trim());
            yield return new WaitForSeconds(0.1f);
            Advertisement.Banner.Show(m_config.adId.bannerAdId.Trim());
        }

        public override void ShowRewardedVideo()
        {
            Advertisement.Show(m_config.adId.rewardedAdId.Trim(), this);
        }
        public override void ShowInterstitial()
        {
            m_interRate++;

            if (!CanShowInterAds || m_isNoAds) return;

            Advertisement.Show(m_config.adId.interstitialAdId.Trim(), this);
        }

        public void OnUnityAdsReady(string placementId)
        {

        }

        public void OnUnityAdsDidError(string message)
        {

        }

        public void OnUnityAdsDidStart(string placementId)
        {

        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {

        }

        public void OnUnityAdsAdLoaded(string placementId)
        {

        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {

        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {

        }

        public void OnUnityAdsShowStart(string placementId)
        {

        }

        public void OnUnityAdsShowClick(string placementId)
        {

        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (string.Compare(placementId, m_config.adId.rewardedAdId.Trim()) == 0)
            {
                m_OnUserReward?.Invoke();
                Advertisement.Load(m_config.adId.rewardedAdId.Trim(), this);
            }
            else if (string.Compare(placementId, m_config.adId.interstitialAdId.Trim()) == 0)
            {
                Advertisement.Load(m_config.adId.interstitialAdId.Trim(), this);
            }
        }
#endif
    }
}
