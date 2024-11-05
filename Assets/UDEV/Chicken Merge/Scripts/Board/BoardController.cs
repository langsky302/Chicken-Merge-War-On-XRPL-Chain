using System;
using System.Collections.Generic;
using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class BoardController : Singleton<BoardController>, IActionEventDispatcher
    {
        public static BoardState state;
        [SerializeField] private Node[] m_nodes;
        private GunStatSO[] m_gunStats;
        [SerializeField] private LayerMask m_rayLayer;
        private Vector3 m_mousePos;
        private NodeItem m_carryingNode;
        private Dictionary<int, Node> m_nodeDictionary;
        private Vector3 m_prevMousePos;
        private float m_dragDistance;
        private bool m_selectedToDestroy;

        #region ACTION
        private Action<object> m_OnBuyingGun;
        #endregion

        public NodeItem CarryingNode { get => m_carryingNode;}

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnBuyingGun = param => PlaceItem((int)param);

            this.RegisterActionEvent(GameplayAction.BUY_GUN, m_OnBuyingGun);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.BUY_GUN, m_OnBuyingGun);
        }
        #endregion

        protected override void Awake()
        {
            MakeSingleton(false);
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            UnregisterEvents();
            TweenUltis.KillAllTweens();
        }

        private void Init()
        {
            state = BoardState.None;
            m_nodeDictionary = new Dictionary<int, Node>();
            m_gunStats = DataGroup.Ins.gunShopData.gunStats;

            for (int i = 0; i < m_nodes.Length; i++)
            {
                var node = m_nodes[i];
                if (node == null) continue;
                node.Id = i;
                m_nodeDictionary.Add(i, node);
            }

            int defaultGunId = Mathf.RoundToInt(UserDataHandler.Ins.curLevelId / 2);
            defaultGunId = Mathf.Clamp(defaultGunId, 0, Mathf.RoundToInt(m_gunStats.Length / 2));
            for (int i = 0; i < 4; i++)
            {
                PlaceItem(defaultGunId);
            }
        }

        private void Update()
        {
            CheckMouseBehavior();

#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0) && m_selectedToDestroy)
            {
                DestroyNode();
            }
#endif
        }

        private void CheckMouseBehavior()
        {
            if (state == BoardState.Merging) return;
            m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                m_dragDistance = 0;
                m_prevMousePos = m_mousePos;
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && m_carryingNode)
            {
                OnItemSelected();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (m_selectedToDestroy) return;

                if (m_dragDistance <= 0.4f)
                {
                    OnItemCarryFail();
                }
                else
                {
                    SendRayCast();
                }
                m_dragDistance = 0;
            }

            m_dragDistance = Vector2.Distance(m_prevMousePos, m_mousePos);
        }

        public void PlaceItem(int itemId)
        {
            if (m_gunStats == null || m_gunStats.Length <= 0) return;

            if (IsAllNodeFull())
            {
                Debug.Log("No empty slot available!");
                return;
            }
            var randNodeIdx = UnityEngine.Random.Range(0, m_nodes.Length);
            var node = GetNodeById(randNodeIdx);

            while (node.state == NodeState.Full)
            {
                randNodeIdx = UnityEngine.Random.Range(0, m_nodes.Length);
                node = GetNodeById(randNodeIdx);
            }

            node.CreateItem(itemId, m_gunStats[itemId].poolKey);
            TweenUltis.ScaleAnim(node.CurNodeItem.transform, 0f, node.CurNodeItem.transform.localScale.x, 0.25f);
        }

        private void OnItemSelected()
        {
            state = BoardState.Dragging;
            m_mousePos.z = 0;
            var delta = 20 * Time.deltaTime;

            delta *= Vector3.Distance(transform.position, m_mousePos);
            m_carryingNode.ChickenCtr.StopShoot();
            m_carryingNode.SetSortingOrder(10);
            m_carryingNode.SetGunNumberSortingOrder(15);
            m_carryingNode.transform.position = Vector3.MoveTowards(m_carryingNode.transform.position, m_mousePos + m_carryingNode.dragOffset, delta);
        }

        private void SendRayCast()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.1f, m_rayLayer);

            if (hit.collider != null)
            {
                CheckRaycastHit(hit);
                return;
            }
            CheckRaycastNotHit();
        }

        private void CheckRaycastHit(RaycastHit2D hit)
        {
            var node = hit.collider.GetComponent<Node>();
            if (node == null) return;

            if (node.state == NodeState.Full && m_carryingNode == null)
            {
                GrapItem(node);
            }
            else if (node.state == NodeState.Empty && m_carryingNode != null)
            {
                DropToEmptyNode(node);
            }
            else if (node.state == NodeState.Full && m_carryingNode != null)
            {

                DropToFullNode(node);
            }
        }

        private void CheckRaycastNotHit()
        {
            if (m_carryingNode == null)
            {
                return;
            }
            OnItemCarryFail();
        }

        private void GrapItem(Node node)
        {
            m_carryingNode = node.CurNodeItem;
            node.ItemGrabbed();
        }

        private void DropToEmptyNode(Node node)
        {
            m_carryingNode.Node.ChangeStateTo(NodeState.Empty);
            m_carryingNode.ChickenCtr.BackToShoot();
            node.UpdateNode(m_carryingNode);
            m_carryingNode = null;
            state = BoardState.None;
        }

        private void DropToFullNode(Node node)
        {
            if (node.CurNodeItem.Id == m_carryingNode.Id && m_carryingNode.Id < m_gunStats.Length - 1
                && node.CurNodeItem.GetInstanceID() != m_carryingNode.GetInstanceID())
            {
                print("merged");
                OnItemMergedWithTarget(node.Id);
                return;
            }
            OnItemCarryFail(node);
        }

        private void OnItemMergedWithTarget(int targetSlotId)
        {
            state = BoardState.Merging;
            var node = GetNodeById(targetSlotId);
            node.CurNodeItem.SetSortingOrder(10);
            node.CurNodeItem.SetGunNumberSortingOrder(15);
#if USE_DOTWEEN
            m_carryingNode.transform.SetParent(node.transform);
            TweenUltis.FxMergeTwo(node.CurNodeItem.transform.localPosition, m_carryingNode.transform, node.CurNodeItem.transform, 0.25f, () =>
            {
                Merge(node);
            });
#else
            Merge(node);
#endif
        }

        private void Merge(Node node)
        {
            if(m_carryingNode == null) return;

            int nextGunId = m_carryingNode.Id + 1;
            if(nextGunId < m_gunStats.Length)
            {
                m_carryingNode.Node.ChangeStateTo(NodeState.Empty);
                node.CurNodeItem.BackToPool();
                node.CurNodeItem.ChickenCtr.StopShoot();
                m_carryingNode.ChickenCtr.StopShoot();
                m_carryingNode.BackToPool();
                node.CurNodeItem = null;
                m_carryingNode = null;

                node.CreateItem(nextGunId, m_gunStats[nextGunId].poolKey);
                this.PostActionEvent(GameplayAction.GUN_MERGED);

                bool isGunUnlocked = UserDataHandler.Ins.IsGunUnlocked(nextGunId);

                if (!isGunUnlocked)
                {
                    this.PostActionEvent(GameplayAction.GUN_UNLOCKED, node.CurNodeItem);
                    UserDataHandler.Ins?.UpdateGunUnlocked(nextGunId, true);
                }
            }
            else
            {
                Swap(node);
            }
            state = BoardState.None;
        }

        private void OnItemCarryFail(Node otherNode = null)
        {
            state = BoardState.None;
            if (otherNode == null)
            {
                BackToOldNode();
                return;
            }
            Swap(otherNode);
        }

        private void Swap(Node otherNode)
        {
            m_carryingNode.Node.UpdateNode(otherNode.CurNodeItem);
            m_carryingNode.ChickenCtr.BackToShoot();
            otherNode.UpdateNode(m_carryingNode);
            m_carryingNode = null;
        }

        private void BackToOldNode()
        {
            if (m_carryingNode == null) return;
            m_carryingNode.Node.UpdateNode(m_carryingNode);
            m_carryingNode.ChickenCtr.BackToShoot();
            m_carryingNode = null;
        }

        public void EnterToDestroyBtn()
        {
            if (m_carryingNode == null) return;
            m_selectedToDestroy = true;
            m_carryingNode.ChangeToDeleteSortingLayer();
        }

        public void ExitFromDestroyBtn()
        {
            if (m_carryingNode == null) return;
            if (m_selectedToDestroy)
            {
                if(Input.GetMouseButtonUp(0))
                {
                    DestroyNode();
                    return;
                }
            }
            m_selectedToDestroy = false;
            m_carryingNode.ChangeToNormalSortingLayer();
        }

        private void DestroyNode()
        {
            if (m_carryingNode == null) return;
            m_carryingNode.BackToPool();
            m_carryingNode = null;
            m_selectedToDestroy = false;
        }

        public bool IsAllNodeFull()
        {
            foreach (var slot in m_nodes)
            {
                if (slot.state == NodeState.Empty)
                {
                    //empty slot found
                    return false;
                }
            }
            //no slot empty 
            return true;
        }

        private Node GetNodeById(int id)
        {
            return m_nodeDictionary[id];
        }
    }
}
