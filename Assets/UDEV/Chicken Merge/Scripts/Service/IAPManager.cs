using UnityEngine.Events;
#if USE_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
#endif

namespace UDEV.ChickenMerge
{
#if USE_IAP
    public class IAPManager : Singleton<IAPManager>, IDetailedStoreListener
    {
        public IAPSO data;
        private ConfigurationBuilder m_builder;

        public IStoreController controller { get; private set; }
        public IExtensionProvider extensions { get; private set; }

        public UnityEvent OnProccessing;
        public UnityEvent OnAdsBuying;
        public UnityEvent<IAPItem> OnItemBuying;
        public UnityEvent<string> OnBuyingFailed;

        private void Start()
        {
            if (data == null) return;
            m_builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            m_builder.AddProduct(data.noadsId, ProductType.NonConsumable);
            foreach (var item in data.items)
            {
                m_builder.AddProduct(item.id, item.productType);
            }

            UnityPurchasing.Initialize(this, m_builder);
        }

        public IAPManager()
        {
            
        }

        public void OnInitialized(IStoreController storeController, IExtensionProvider extensionProvider)
        {
            controller = storeController;
            extensions = extensionProvider;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            var purchasedProductId = e.purchasedProduct.definition.id;
            
            if (IsNoAdsItem(purchasedProductId))
            {
                OnAdsBuying?.Invoke();
            }
            else
            {
                var buyingItem = data.items.Find(x => x.id == purchasedProductId);
                OnItemBuying?.Invoke(buyingItem);
            }

            return PurchaseProcessingResult.Complete;
        }

        private bool IsNoAdsItem(string productId)
        {
            return string.Compare(productId, data.noadsId) == 0;
        }

        public void MakeItemBuying(string itemId)
        {
            if (controller == null) return;

            OnProccessing?.Invoke();

            controller.InitiatePurchase(itemId);
        }

        public string GetPriceString(IAPItem item, string currencySymbol = "$")
        {
            string localPriceStr = currencySymbol + item.localPrice.ToString();
            if(controller == null) return localPriceStr;
            var product = controller.products.WithID(item.id);
            if (product == null || product.metadata.localizedPrice <= new decimal(0.01))
            {
                return localPriceStr;
            }
            return product.metadata.localizedPriceString;
        }

        public void RestorePurchase()
        {
#if UNITY_IOS
            var IExtension = extensions.GetExtension<IAppleExtensions>();
            IExtension.RestoreTransactions((bool isSuccess, string str) => {
                if (!isSuccess) return;
                OnRestorePurchase?.Invoke();
            });
#endif
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            string reasonMessage = GetPurchaseFailureReasonString(reason);
            OnBuyingFailed?.Invoke(reasonMessage);
        }

        private string GetPurchaseFailureReasonString(PurchaseFailureReason reason)
        {
            switch (reason)
            {
                case PurchaseFailureReason.DuplicateTransaction:
                    return "Duplicate transaction.";

                case PurchaseFailureReason.ExistingPurchasePending:
                    return "Existing purchase pending.";

                case PurchaseFailureReason.PaymentDeclined:
                    return "Payment was declined.";

                case PurchaseFailureReason.ProductUnavailable:
                    return "Product is not available.";

                case PurchaseFailureReason.PurchasingUnavailable:
                    return "Purchasing is not available.";

                case PurchaseFailureReason.SignatureInvalid:
                    return "Invalid signature.";

                case PurchaseFailureReason.Unknown:
                    return "Unknown error.";

                case PurchaseFailureReason.UserCancelled:
                    return "User cancelled.";

            }

            return "Unknown error.";
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        {
            
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            
        }
    }
#else
    public class IAPManager : Singleton<IAPManager>
    {

    }
#endif
}
