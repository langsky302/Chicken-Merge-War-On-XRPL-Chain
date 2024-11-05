using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV
{
    public class TabController : Singleton<TabController>
    {
        [SerializeField] private List<TabButton> m_tabButtons;
        [SerializeField] private TabButton m_selectedTab;
        [SerializeField] private Transform m_tabHolder;
        [SerializeField] private Sprite m_idleTabSprite;
        [SerializeField] private Color m_idleTabColor;
        [SerializeField] private Sprite m_hoverTabSprite;
        [SerializeField] private Color m_hoverTabColor;
        [SerializeField] private Sprite m_selectedTabSprite;
        [SerializeField] private Color m_selectedTabColor;
        [SerializeField] private Color m_activeBtnTxtColor;
        [SerializeField] private Color m_inactiveBtnTxtColor;

        public UnityEvent onInit;
        public UnityEvent onEnter;
        public UnityEvent onSelected;
        public UnityEvent onExit;

        private List<Vector3> m_startPosArr;

        protected override void Awake()
        {
            MakeSingleton(false);
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            GetTabPositions();
            AssignTabController();
            ResetTabButtonsState();

            if (onInit != null)
                onInit.Invoke();

            m_selectedTab?.content?.Show(true);
            m_selectedTab?.content?.LoadContent();
        }

        private void GetTabPositions()
        {
            if (m_tabButtons == null || m_tabButtons.Count <= 0) return;

            m_startPosArr = new List<Vector3>();

            for (int i = 0; i < m_tabButtons.Count; i++)
            {
                if (m_tabButtons[i])
                    m_startPosArr.Add(m_tabButtons[i].transform.localPosition);
            }
        }

        private void AssignTabController()
        {
            if (m_tabButtons == null || m_tabButtons.Count <= 0) return;
            for (int i = 0; i < m_tabButtons.Count; i++)
            {
                var tabButton = m_tabButtons[i];
                if (tabButton == null) continue;
                tabButton.Controller = this;
                if (tabButton.content == null) continue;
                tabButton.content.Show(false);
                tabButton.content.TabController = this;
            }
        }

        public void SetActiveTab(int index)
        {
            if (m_tabButtons == null || m_tabButtons.Count <= 0 || index >= m_tabButtons.Count) return;

            m_selectedTab = m_tabButtons[index];
            ResetTabButtons();
        }

        public void OnTabEnter(TabButton button)
        {
            if(button != m_selectedTab)
            {
                ResetTabButtons();

                button.tabImage.sprite = m_hoverTabSprite;
                button.tabImage.color = m_hoverTabColor;
            }

            if (onEnter != null)
                onEnter.Invoke();
        }

        public void OnTabSelected(TabButton button)
        {
            button.tabImage.sprite = m_selectedTabSprite;
            button.tabImage.color = m_selectedTabColor;

            if (m_selectedTab == null || m_selectedTab.content == null) return;

            m_selectedTab.content.Show(false);

            if (m_selectedTab != button)
            {
                m_selectedTab = button;
                m_selectedTab.content.LoadContent();
            }

            if (onSelected != null)
                onSelected.Invoke();

            ResetTabButtons();
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabButtons();

            if (onExit != null)
                onExit.Invoke();
        }

        public void ResetTabButtons()
        {
            ResetTabButtonsState();

            ReDrawTabButtons();
        }

        private void ResetTabButtonsState()
        {
            if (m_tabButtons == null || m_tabButtons.Count <= 0) return;

            for (int i = 0; i < m_tabButtons.Count; i++)
            {
                var tabButton = m_tabButtons[i];
                if (tabButton == null) return;

                if (m_selectedTab != null && tabButton == m_selectedTab)
                {
                    ActiveButtonState(tabButton);
                    continue;
                }
                else
                {
                    IdleButtonState(tabButton);
                }
            }
        }

        private void ActiveButtonState(TabButton tabButton)
        {
            tabButton.tabSelected?.SetActive(true);
            tabButton.tabInactive?.SetActive(false);

            if (tabButton.tabImage == null) return;

            tabButton.tabImage.color = m_selectedTabColor;
            tabButton.tabImage.sprite = m_selectedTabSprite;
            tabButton.btnText.color = m_activeBtnTxtColor;
        }

        private void IdleButtonState(TabButton tabButton)
        {
            tabButton.tabSelected?.SetActive(false);
            tabButton.tabInactive?.SetActive(true);

            if (tabButton.tabImage == null) return;

            tabButton.tabImage.sprite = m_idleTabSprite;
            tabButton.tabImage.color = m_idleTabColor;
            tabButton.btnText.color = m_inactiveBtnTxtColor;
        }

        private void ReDrawTabButtons()
        {
            if (m_tabButtons == null || m_tabButtons.Count <= 0 || m_tabHolder == null) return;

            for (int i = 0; i < m_tabButtons.Count; i++)
            {
                if (m_tabButtons[i])
                    m_tabButtons[i].transform.SetParent(null);
            }

            PushSelectedTabToBottom();

            for (int i = 0; i < m_tabButtons.Count; i++)
            {
                if (m_tabButtons[i])
                {
                    m_tabButtons[i].transform.SetParent(m_tabHolder);
                    m_tabButtons[i].transform.localPosition = m_startPosArr[i];
                }
            }
        }

        private void PushSelectedTabToBottom()
        {
            List<TabButton> newTabs = new List<TabButton>();
            var seletectedTabCopy = m_selectedTab;
            var tabsCopy = m_tabButtons;
            int selectedTabIdx = tabsCopy.IndexOf(seletectedTabCopy);
            tabsCopy.Remove(seletectedTabCopy);
            newTabs.AddRange(tabsCopy);
            newTabs.Add(seletectedTabCopy);
            m_tabButtons = newTabs;

            ResortTabPositions(selectedTabIdx);
        }

        private void ResortTabPositions(int selectedTabIdx)
        {
            List<Vector3> tabPositions = new List<Vector3>();
            var selectedTabPos = m_startPosArr[selectedTabIdx];
            m_startPosArr.Remove(selectedTabPos);
            tabPositions.AddRange(m_startPosArr);
            tabPositions.Add(selectedTabPos);
            m_startPosArr = tabPositions;
        }
    }
}
