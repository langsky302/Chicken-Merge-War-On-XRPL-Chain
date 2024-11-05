#if USE_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump.Api;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV
{
    public class AdmobController : AdsController
    {
        [SerializeField] private OpenAdCtr m_openAd;
        [SerializeField] private BannerAd m_bannerAd;
        [SerializeField] private RewaredVideoAd m_rewardedAd;
        [SerializeField] private RewardedVideoInterAd m_rewardedInterAd;
        [SerializeField] private InterAd m_interstitialAd;
#if USE_ADMOB
        // Always use test ads.
        // https://developers.google.com/admob/unity/test-ads
        internal static List<string> TestDeviceIds = new List<string>()
        {
            AdRequest.TestDeviceSimulator,
#if UNITY_IPHONE
            "96e23e80653bb28980d3f40beb58915c",
#elif UNITY_ANDROID
            "702815ACFC14FF222DA1DC767672A573"
#endif
        };

        // The Google Mobile Ads Unity plugin needs to be run only once.
        private static bool? _isInitialized;

        // Helper class that implements consent using the
        // Google User Messaging Platform (UMP) Unity plugin.
        [SerializeField, Tooltip("Controller for the Google User Messaging Platform (UMP) Unity plugin.")]
        private AdmobConsentCtr _consentController;

        public override void Init()
        {
            base.Init();
            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            MobileAds.SetiOSAppPauseOnBackground(true);

            // When true all events raised by GoogleMobileAds will be raised
            // on the Unity main thread. The default value is false.
            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            // Configure your RequestConfiguration with Child Directed Treatment
            // and the Test Device Ids.
            MobileAds.SetRequestConfiguration(new RequestConfiguration
            {
                TestDeviceIds = TestDeviceIds
            });

            // If we can request ads, we should initialize the Google Mobile Ads Unity plugin.
            if (_consentController.CanRequestAds)
            {
                InitializeGoogleMobileAds();
            }

            // Ensures that privacy and consent information is up to date.
            InitializeGoogleMobileAdsConsent();

            m_rewardedAd.OnUserReward = OnUserReward;
            m_rewardedInterAd.OnUserReward = OnUserReward;          
        }

        protected override void Awake()
        {
            base.Awake();
            m_openAd?.LoadAd(this);
            m_bannerAd?.LoadAd(this);
            m_interstitialAd?.LoadAd(this);
            m_rewardedAd?.LoadAd(this);
            m_rewardedInterAd?.LoadAd(this);
            HideBanner();
        }

        /// <summary>
        /// Ensures that privacy and consent information is up to date.
        /// </summary>
        private void InitializeGoogleMobileAdsConsent()
        {
            Debug.Log("Google Mobile Ads gathering consent.");

            _consentController.GatherConsent((string error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Failed to gather consent with error: " +
                        error);
                }
                else
                {
                    Debug.Log("Google Mobile Ads consent updated: "
                        + ConsentInformation.ConsentStatus);
                }

                if (_consentController.CanRequestAds)
                {
                    InitializeGoogleMobileAds();
                }
            });
        }

        /// <summary>
        /// Initializes the Google Mobile Ads Unity plugin.
        /// </summary>
        private void InitializeGoogleMobileAds()
        {
            // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
            if (_isInitialized.HasValue)
            {
                return;
            }

            _isInitialized = false;

            // Initialize the Google Mobile Ads Unity plugin.
            Debug.Log("Google Mobile Ads Initializing.");
            MobileAds.Initialize((InitializationStatus initstatus) =>
            {
                if (initstatus == null)
                {
                    Debug.LogError("Google Mobile Ads initialization failed.");
                    _isInitialized = null;
                    return;
                }

                // If you use mediation, you can check the status of each adapter.
                var adapterStatusMap = initstatus.getAdapterStatusMap();
                if (adapterStatusMap != null)
                {
                    foreach (var item in adapterStatusMap)
                    {
                        Debug.Log(string.Format("Adapter {0} is {1}",
                            item.Key,
                            item.Value.InitializationState));
                    }
                }

                Debug.Log("Google Mobile Ads initialization complete.");
                _isInitialized = true;
            });
        }

        /// <summary>
        /// Opens the AdInspector.
        /// </summary>
        public void OpenAdInspector()
        {
            Debug.Log("Opening ad Inspector.");
            MobileAds.OpenAdInspector((AdInspectorError error) =>
            {
                // If the operation failed, an error is returned.
                if (error != null)
                {
                    Debug.Log("Ad Inspector failed to open with error: " + error);
                    return;
                }

                Debug.Log("Ad Inspector opened successfully.");
            });
        }

        /// <summary>
        /// Opens the privacy options form for the user.
        /// </summary>
        /// <remarks>
        /// Your app needs to allow the user to change their consent status at any time.
        /// </remarks>
        public void OpenPrivacyOptions()
        {
            _consentController.ShowPrivacyOptionsForm((string error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Failed to show consent privacy form with error: " +
                        error);
                }
                else
                {
                    Debug.Log("Privacy form opened successfully.");
                }
            });
        }

        public override void ShowBanner()
        {
            if (m_isNoAds) return;
            m_bannerAd?.ShowAd();
        }

        public override void ShowInterstitial()
        {
            if (m_isNoAds) return;
            m_interRate++;
            if (!CanShowInterAds) return;            
            m_interstitialAd?.ShowAd();
        }

        public override void HideBanner()
        {
            m_bannerAd?.HideAd();
        }

        public override void ShowRewardedVideo()
        {
            m_rewardedAd?.ShowAd();
        }

        public override void ShowRewardedInterstitialAd()
        {
            m_rewardedInterAd?.ShowAd();
        }
#endif
    }
}
