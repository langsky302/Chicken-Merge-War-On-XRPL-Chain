#if USE_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV
{
    public class RewaredVideoAd : AdBase
    {
        protected AdmobConfig m_config;
#if USE_ADMOB
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";

        private RewardedAd _rewardedAd;

        public UnityEvent OnUserReward;

        /// <summary>
        /// Loads the ad.
        /// </summary>
        public override void LoadAd(AdsController adCtr = null)
        {
            base.LoadAd(adCtr);

            m_config = (AdmobConfig)m_adConfig;

#if UNITY_ANDROID
            _adUnitId = m_config.androidIds.rewardedAdId.Trim();
#elif UNITY_IPHONE
        _adUnitId = m_config.iosIds.rewardedAdId.Trim();
#else
        _adUnitId = "unused";
#endif

            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading rewarded ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);
            });
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public override void ShowAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                Debug.Log("Showing rewarded ad.");
                _rewardedAd.Show((Reward reward) =>
                {
                    OnUserReward?.Invoke();
                    LoadAd(m_adCtr);
                });
            }
            else
            {
                Debug.LogError("Rewarded ad is not ready yet.");
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public override void DestroyAd()
        {
            if (_rewardedAd != null)
            {
                Debug.Log("Destroying rewarded ad.");
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public override void LogResponseInfo()
        {
            if (_rewardedAd != null)
            {
                var responseInfo = _rewardedAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when the ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content with error : "
                    + error);
            };
        }
#endif
    }
}
