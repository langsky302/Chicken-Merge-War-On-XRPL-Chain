namespace UDEV
{
    [System.Serializable]
    public class AdmobId : AdId
    {
        public string openAdId;
        public string rewardedInterstitialAdId;
    }

    [System.Serializable]
    public class AdId
    {
        public string appId;
        public string bannerAdId;
        public string interstitialAdId;
        public string rewardedAdId;
    }
}
