using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] protected CollectableSO m_stat;
        protected int m_bonus;
        protected int m_bonusMultier;

        public virtual void Init(int bonusMultier = 1)
        {
            m_bonusMultier = bonusMultier;
            if (m_stat)
            {
                m_bonus = Random.Range(m_stat.minBonus, m_stat.maxBonus);
            }
        }

        protected virtual void TriggerHandle()
        {

        }

        public virtual void Trigger()
        {
            if (m_stat == null) return;

            TriggerHandle();

            gameObject.SetActive(false);
        }
    }
}
