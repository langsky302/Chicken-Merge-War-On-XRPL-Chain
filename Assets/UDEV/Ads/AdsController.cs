using UnityEngine;
using UnityEngine.Events;

namespace UDEV
{
    public enum AdsPlatform
    {
        Admob,
        UnityAds
    }

    public class AdsController : Singleton<AdsController>
    {
        public AdConfig config;

        protected int m_interRate;
        protected bool m_isNoAds;
        public UnityEvent OnUserReward;

        public bool IsNoAds
        {
            get => m_isNoAds;
        }

        public bool CanShowInterAds
        {
            get {
                bool canShow = config && m_interRate >= config.interstitialAdShowRate;
                if (canShow)
                {
                    m_interRate = 0;
                }
                return canShow;
            }
        }

        public virtual void Init()
        {

        }

        public void SetIsNoAds(bool isNoAds)
        {
            m_isNoAds = isNoAds;
        }

        public virtual void ShowBanner()
        {

        }

        public virtual void HideBanner()
        {

        }

        public virtual bool IsRewardedVideoAvaiable()
        {
            return false;
        }

        public virtual void ShowInterstitial()
        {
            
        }

        public virtual void ShowRewardedVideo()
        {

        }

        public virtual void ShowRewardedInterstitialAd()
        {

        }

        protected virtual void UserRewarded()
        {

        }
    }
}
