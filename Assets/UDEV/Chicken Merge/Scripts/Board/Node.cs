using UnityEngine;
using UDEV.SPM;

namespace UDEV.ChickenMerge
{
    public class Node : MonoBehaviour
    {
        private int m_id;
        public NodeState state;
        public NodeType nodeType;
        public Vector3 spawnPos;

        private NodeItem m_curNodeItem;

        public NodeItem CurNodeItem { get => m_curNodeItem; set => m_curNodeItem = value; }
        public int Id { get => m_id; set => m_id = value; }

        public void CreateItem(int id, string poolId)
        {
            var nodeItemObj = PoolersManager.Ins?.Spawn(PoolerTarget.NONE, poolId, Vector3.zero, Quaternion.identity);
            if (nodeItemObj == null) return;
            var nodeItem = nodeItemObj.GetComponent<NodeItem>();
            if(nodeItem == null) return;
            Helper.AssignToRoot(transform, nodeItemObj.transform, spawnPos, Vector3.one * 0.9f);
            nodeItem.Init(this, id);
            m_curNodeItem = nodeItem;
            m_curNodeItem.ChickenCtr.Init(id);
            ChangeStateTo(NodeState.Full);
        }

        public void UpdateNode(NodeItem nodeItem)
        {
            if (nodeItem == null) return;
            Helper.AssignToRoot(transform, nodeItem.transform, spawnPos, Vector3.one * 0.9f);
            nodeItem.Init(this, nodeItem.Id);
            m_curNodeItem = nodeItem;
            ChangeStateTo(NodeState.Full);
        }

        public void ChangeStateTo(NodeState targetState)
        {
            state = targetState;
        }

        public void ItemGrabbed()
        {
            m_curNodeItem.ChickenCtr.StopShoot();
            m_curNodeItem.ChickenCtr.transform.SetParent(null);
        }
    }
}
