using UnityEngine;
using UDEV.DMTool;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class IAPDialog : Dialog
    {
        [SerializeField] private Transform m_layoutRoot;
        [SerializeField] private IAPItemUI m_itemPrefab;
        [SerializeField] private Button m_restorePurchasesBtn;
#if USE_IAP
        List<IAPItem> m_items;
#endif

        public override void Show()
        {
            GameController.ChangeState(GameState.Pausing);
            base.Show();
#if USE_IAP
            if (IAPManager.Ins == null || IAPManager.Ins.data == null) return;

            m_items = IAPManager.Ins.data.items;
#endif
            UpdateUI();
        }

        public void UpdateUI()
        {
            //Helper.ClearChilds(layoutRoot);
#if USE_IAP
            if (m_items == null || m_items.Count <= 0) return;
            for (int i = 0; i < m_items.Count; i++)
            {
                int idx = i;

                IAPItem item = m_items[idx];

                if (item != null && m_layoutRoot && m_itemPrefab)
                {
                    IAPItemUI itemUIClone = Instantiate(m_itemPrefab, Vector3.zero, Quaternion.identity);

                    itemUIClone.transform.SetParent(m_layoutRoot);
                    itemUIClone.transform.localPosition = Vector3.zero;
                    itemUIClone.transform.localScale = Vector3.one;

                    itemUIClone.UpdateUI(item);

                    if (itemUIClone.buyBtn)
                    {
                        itemUIClone.buyBtn.onClick.RemoveAllListeners();
                        itemUIClone.buyBtn.onClick.AddListener(() => Purchase(item));
                    }
                }
            }
            m_restorePurchasesBtn.onClick.RemoveAllListeners();
            m_restorePurchasesBtn.onClick.AddListener(() => RestorePurchase());

#if UNITY_IOS
            m_restorePurchasesBtn.gameObject.SetActive(true);
#endif

#endif
        }
#if USE_IAP
        void Purchase(IAPItem item)
        {
            IAPManager.Ins?.MakeItemBuying(item.id);
        }

        public void RestorePurchase()
        {
            IAPManager.Ins?.RestorePurchase();
        }
#endif

        public override void Close()
        {
            base.Close();
            GameController.RevertState();
        }
    }
}
