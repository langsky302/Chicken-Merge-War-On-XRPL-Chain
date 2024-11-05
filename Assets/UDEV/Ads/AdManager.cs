using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;

namespace UDEV
{
    public class AdManager : Singleton<AdManager>
    {
        [Header("Ads Settings:")]
        public AdsPlatform adsPlatform;
        public AdsController adsController;

        protected override void Awake()
        {
            base.Awake();
            var adController = Instantiate(adsController, Vector3.zero, Quaternion.identity);
            adController?.Init();
        }
    }
}
