#if USE_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using UnityEngine;

namespace UDEV
{
    public class BannerAd : AdBase
    {
        protected AdmobConfig m_config;
#if USE_ADMOB
        private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";

        private BannerView _bannerView;

        /// <summary>
        /// Creates a 320x50 banner at top of the screen.
        /// </summary>
        public void CreateBannerView()
        {
            Debug.Log("Creating banner view.");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyAd();
            }

            // Create a 320x50 banner at top of the screen.
            _bannerView = new BannerView(_adUnitId, AdSize.Banner, m_config.bannerPosition);

            // Listen to events the banner may raise.
            ListenToAdEvents();

            Debug.Log("Banner view created.");
        }

        /// <summary>
        /// Creates the banner view and loads a banner ad.
        /// </summary>
        public override void LoadAd(AdsController adCtr = null)
        {
            base.LoadAd(adCtr);

            m_config = (AdmobConfig)m_adConfig;

#if UNITY_ANDROID
            _adUnitId = m_config.androidIds.bannerAdId.Trim();
#elif UNITY_IPHONE
        _adUnitId = m_config.iosIds.bannerAdId.Trim();
#else
        _adUnitId = "unused";
#endif

            // Create an instance of a banner view first.
            if (_bannerView == null)
            {
                CreateBannerView();
            }

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public override void ShowAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Showing banner view.");
                _bannerView.Show();
            }
        }

        /// <summary>
        /// Hides the ad.
        /// </summary>
        public override void HideAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Hiding banner view.");
                _bannerView.Hide();
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// When you are finished with a BannerView, make sure to call
        /// the Destroy() method before dropping your reference to it.
        /// </summary>
        public override void DestroyAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Destroying banner view.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public override void LogResponseInfo()
        {
            if (_bannerView != null)
            {
                var responseInfo = _bannerView.GetResponseInfo();
                if (responseInfo != null)
                {
                    UnityEngine.Debug.Log(responseInfo);
                }
            }
        }

        /// <summary>
        /// Listen to events the banner may raise.
        /// </summary>
        private void ListenToAdEvents()
        {
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += () =>
            {
                Debug.Log("Banner view loaded an ad with response : "
                    + _bannerView.GetResponseInfo());
            };
            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                Debug.LogError("Banner view failed to load an ad with error : " + error);
            };
            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Banner view paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Banner view recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            _bannerView.OnAdClicked += () =>
            {
                Debug.Log("Banner view was clicked.");
            };
            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Banner view full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Banner view full screen content closed.");
            };
        }
#endif
    }
}
