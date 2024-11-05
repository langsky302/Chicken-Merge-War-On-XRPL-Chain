using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ActiveBoosterDrawer : MonoBehaviour
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private ActiveBoosterItem m_itemUIPrefab;

        private BoosterController[] m_boosterCtrs;

        public void Draw()
        {
            Helper.ClearChilds(m_gridRoot);
            m_boosterCtrs = BoosterManager.Ins.GetActiveBoosterCtrs();
            if (m_boosterCtrs == null || m_boosterCtrs.Length <= 0) return;

            for (int i = 0; i < m_boosterCtrs.Length; i++)
            {
                var boosterCtr = m_boosterCtrs[i];
                if(boosterCtr == null) continue;
                var itemUIClone = Instantiate(m_itemUIPrefab);
                itemUIClone.UpdateUI(boosterCtr);
                Helper.AssignToRoot(m_gridRoot, itemUIClone.transform, Vector3.zero, Vector3.one);
            }
        }
    }
}
