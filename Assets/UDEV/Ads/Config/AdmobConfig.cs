#if USE_ADMOB
using GoogleMobileAds.Api;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV {
    [CreateAssetMenu(fileName = "Admob Config", menuName = "UDEV/Ads/Create Admob Config")]
    public class AdmobConfig : AdConfig
    {
        public AdmobId androidIds;
        public AdmobId iosIds;
#if USE_ADMOB
        public AdPosition bannerPosition = AdPosition.Bottom;
#endif
    }
}
