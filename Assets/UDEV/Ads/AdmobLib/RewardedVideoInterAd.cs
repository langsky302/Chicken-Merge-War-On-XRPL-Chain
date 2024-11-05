#if USE_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV
{
    public class RewardedVideoInterAd : AdBase
    {
        protected AdmobConfig m_config;
#if USE_ADMOB
        private string _adUnitId = "ca-app-pub-3940256099942544/5354046379";

        private RewardedInterstitialAd _rewardedInterstitialAd;

        public UnityEvent OnUserReward;

        /// <summary>
        /// Loads the ad.
        /// </summary>
        public override void LoadAd(AdsController adCtr = null)
        {
            base.LoadAd(adCtr);

            m_config = (AdmobConfig)m_adConfig;

#if UNITY_ANDROID
            _adUnitId = m_config.androidIds.rewardedInterstitialAdId.Trim();
#elif UNITY_IPHONE
        _adUnitId = m_config.iosIds.rewardedInterstitialAdId.Trim();
#else
        _adUnitId = "unused";
#endif
            // Clean up the old ad before loading a new one.
            if (_rewardedInterstitialAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading rewarded interstitial ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            RewardedInterstitialAd.Load(_adUnitId, adRequest,
                (RewardedInterstitialAd ad, LoadAdError error) =>
                {
                    // If the operation failed with a reason.
                    if (error != null)
                    {
                        Debug.LogError("Rewarded interstitial ad failed to load an ad with error : "
                                        + error);
                        return;
                    }
                    // If the operation failed for unknown reasons.
                    // This is an unexpexted error, please report this bug if it happens.
                    if (ad == null)
                    {
                        Debug.LogError("Unexpected error: Rewarded interstitial load event fired with null ad and null error.");
                        return;
                    }

                    // The operation completed successfully.
                    Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());
                    _rewardedInterstitialAd = ad;

                    // Register to ad events to extend functionality.
                    RegisterEventHandlers(ad);
                });
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public override void ShowAd()
        {
            if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
            {
                _rewardedInterstitialAd.Show((Reward reward) =>
                {
                    OnUserReward?.Invoke();
                    LoadAd(m_adCtr);
                });
            }
            else
            {
                Debug.LogError("Rewarded interstitial ad is not ready yet.");
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public override void DestroyAd()
        {
            if (_rewardedInterstitialAd != null)
            {
                Debug.Log("Destroying rewarded interstitial ad.");
                _rewardedInterstitialAd.Destroy();
                _rewardedInterstitialAd = null;
            }
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public override void LogResponseInfo()
        {
            if (_rewardedInterstitialAd != null)
            {
                var responseInfo = _rewardedInterstitialAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        protected void RegisterEventHandlers(RewardedInterstitialAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded interstitial ad failed to open full screen content" +
                               " with error : " + error);
            };
        }
#endif
    }
}
