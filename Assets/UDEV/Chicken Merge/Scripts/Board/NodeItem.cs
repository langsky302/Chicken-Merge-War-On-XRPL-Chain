using UnityEngine;

namespace UDEV.ChickenMerge
{
    [RequireComponent(typeof(ChickenController))]
    public class NodeItem : MonoBehaviour
    {
        public Vector3 dragOffset;
        public GunNumberUI numberGUI;
        [SerializeField] private string m_delsortingLayer = "Default";
        [SerializeField] private string m_normalSortingLayer = "Gameplay";

        private int m_id;
        private Node m_node;
        private ChickenController m_chickenCtr;
        private int[] m_originalSortingOrders;
        [SerializeField] private SpriteRenderer[] m_spriteRenderers;

        public Node Node { get => m_node; }
        public int Id { get => m_id; }
        public ChickenController ChickenCtr { get => m_chickenCtr;}

        private void Awake()
        {
            m_chickenCtr = GetComponent<ChickenController>();
            m_originalSortingOrders = new int[m_spriteRenderers.Length];
            if (m_spriteRenderers == null || m_spriteRenderers.Length <= 0) return;
            for (int i = 0; i < m_spriteRenderers.Length; i++)
            {
                var spriteRenderer = m_spriteRenderers[i];
                if (spriteRenderer == null) continue;
                m_originalSortingOrders[i] = spriteRenderer.sortingOrder;
            }
            if (numberGUI == null) return;
            numberGUI.canvas.sortingOrder = m_originalSortingOrders[m_originalSortingOrders.Length - 1] + 1;
        }

        public void Init(Node node, int id)
        {
            m_node = node;
            m_id = id;
            m_chickenCtr.CurNode = node;
            ResetSorting();
            numberGUI?.SetNumber(id + 1);
        }

        public void ResetSorting()
        {
            SetSortingLayer(m_normalSortingLayer);
            ResetSortingOrder();
            if (numberGUI == null) return;
            numberGUI.canvas.sortingOrder = m_originalSortingOrders[m_originalSortingOrders.Length - 1] + 1;
        }

        public void ChangeToDeleteSortingLayer()
        {
            SetSortingLayer(m_delsortingLayer);
        }

        public void ChangeToNormalSortingLayer()
        {
            SetSortingLayer(m_normalSortingLayer);
        }

        public void SetGunNumberSortingOrder(int order)
        {
            if (numberGUI == null || numberGUI.canvas == null) return;
            numberGUI.canvas.sortingOrder = order;
        }

        private void SetSortingLayer(string layerName)
        {
            if (m_spriteRenderers == null || m_spriteRenderers.Length <= 0) return;
            for (int i = 0; i < m_spriteRenderers.Length; i++)
            {
                var spriteRenderer = m_spriteRenderers[i];
                if (spriteRenderer == null) continue;
                spriteRenderer.sortingLayerName = layerName;
            }
            numberGUI.canvas.sortingLayerName = layerName;
        }

        public void SetSortingOrder(int order)
        {
            if (m_spriteRenderers == null || m_spriteRenderers.Length <= 0) return;
            for (int i = 0; i < m_spriteRenderers.Length; i++)
            {
                var spriteRenderer = m_spriteRenderers[i];
                if (spriteRenderer == null) continue;
                spriteRenderer.sortingOrder = i + order;
            }
        }

        private void ResetSortingOrder()
        {
            if (m_spriteRenderers == null || m_spriteRenderers.Length <= 0) return;
            for (int i = 0; i < m_spriteRenderers.Length; i++)
            {
                var spriteRenderer = m_spriteRenderers[i];
                if (spriteRenderer == null) continue;
                spriteRenderer.sortingOrder = m_originalSortingOrders[i];
            }
        }

        public void BackToPool()
        {
            ResetSorting();
            gameObject.SetActive(false);
        }
    }
}
