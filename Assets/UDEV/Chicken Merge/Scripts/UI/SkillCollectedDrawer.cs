using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class SkillCollectedDrawer : MonoBehaviour
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private SkillCollectedItemUI m_itemUIPrefab;

        private Dictionary<SkillCollectedType, int> m_skillCollecteds;
        public void Draw()
        {
            m_skillCollecteds = SkillCollectedManager.Ins.SkillCollecteds;
            Helper.ClearChilds(m_gridRoot);

            foreach (var skill in m_skillCollecteds)
            {
                var skillItemUIClone = Instantiate(m_itemUIPrefab);
                Helper.AssignToRoot(m_gridRoot, skillItemUIClone.transform, Vector3.zero, Vector3.one);
                skillItemUIClone.Init(skill.Key);
            }
        }
    }
}
