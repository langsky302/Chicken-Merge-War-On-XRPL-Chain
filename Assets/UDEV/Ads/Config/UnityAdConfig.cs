using UnityEngine;
#if USE_UNITY_AD
using UnityEngine.Advertisements;
#endif

namespace UDEV
{
    [CreateAssetMenu(fileName = "UnityAds Config", menuName = "UDEV/Ads/Create Unity Ads Config")]
    public class UnityAdConfig : AdConfig
    {
        public bool isTestMode;
        public AdId adId;
#if USE_UNITY_AD
        public BannerPosition bannerPosition;
#endif
    }
}
