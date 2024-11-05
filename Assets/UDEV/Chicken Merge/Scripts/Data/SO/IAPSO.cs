using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USE_IAP
using UnityEngine.Purchasing;
#endif

namespace UDEV.ChickenMerge
{
    [CreateAssetMenu(fileName = "New IAP Data", menuName = "UDEV/Create IAP Data")]
    public class IAPSO : ScriptableObject
    {
        public string noadsId;
        public float noadsLocalPrice;
        public List<IAPItem> items;
    }

    [System.Serializable]
    public class IAPItem
    {
#if USE_IAP
        public ProductType productType;
#endif
        public string id;
        public int amount;
        public float localPrice;
    }
}
