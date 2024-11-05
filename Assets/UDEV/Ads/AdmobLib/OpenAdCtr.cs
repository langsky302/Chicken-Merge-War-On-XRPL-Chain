#if USE_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif
using System;
using UnityEngine;

namespace UDEV
{
    public class OpenAdCtr : AdBase
    {
        protected AdmobConfig m_config;

#if USE_ADMOB
        private string _adUnitId = "ca-app-pub-3940256099942544/3419835294";

        // App open ads can be preloaded for up to 4 hours.
        private readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
        private DateTime _expireTime;
        private AppOpenAd _appOpenAd;

        private void Awake()
        {
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        private void OnDestroy()
        {
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
        }

        private void OnAppStateChanged(AppState state)
        {
            Debug.Log("App State changed to : " + state);

            // If the app is Foregrounded and the ad is available, show it.
            if (state == AppState.Foreground)
            {
                ShowAd();
            }
        }

        /// <summary>
        /// Loads the ad.
        /// </summary>
        override public void LoadAd(AdsController adCtr = null)
        {
            base.LoadAd(adCtr);

            m_config = (AdmobConfig)m_adConfig;

#if UNITY_ANDROID
            _adUnitId = m_config.androidIds.openAdId.Trim();
#elif UNITY_IPHONE
        _adUnitId = m_config.iosIds.openAdId.Trim();
#else
        _adUnitId = "unused";
#endif

            // Clean up the old ad before loading a new one.
            if (_appOpenAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading app open ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            AppOpenAd.Load(_adUnitId, adRequest, (AppOpenAd ad, LoadAdError error) =>
            {
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("App open ad failed to load an ad with error : "
                                    + error);
                    return;
                }

                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: App open ad load event fired with " +
                                   " null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());
                _appOpenAd = ad;

                // App open ads can be preloaded for up to 4 hours.
                _expireTime = DateTime.Now + TIMEOUT;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);
            });
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public override void ShowAd()
        {
            if (m_adCtr && m_adCtr.IsNoAds) return;

            // App open ads can be preloaded for up to 4 hours.
            if (_appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime)
            {
                Debug.Log("Showing app open ad.");
                _appOpenAd.Show();
            }
            else
            {
                Debug.LogError("App open ad is not ready yet.");
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public override void DestroyAd()
        {
            if (_appOpenAd != null)
            {
                Debug.Log("Destroying app open ad.");
                _appOpenAd.Destroy();
                _appOpenAd = null;
            }
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public override void LogResponseInfo()
        {
            if (_appOpenAd != null)
            {
                var responseInfo = _appOpenAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        private void RegisterEventHandlers(AppOpenAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("App open ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("App open ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("App open ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("App open ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("App open ad full screen content closed.");

                // It may be useful to load a new ad when the current one is complete.
                LoadAd(m_adCtr);
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("App open ad failed to open full screen content with error : "
                                + error);
            };
        }
#endif
    }
}
