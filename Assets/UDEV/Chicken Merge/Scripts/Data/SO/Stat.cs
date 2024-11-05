using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public abstract class Stat : ScriptableObject
    {
        [UniqueId]
        public string id;
        [HideInInspector]
        public Sprite thumb;

        public virtual void Save()
        {

        }

        public virtual void Save(int id)
        {

        }

        public virtual void Load()
        {

        }

        public virtual void Load(int id)
        {

        }

        public virtual void Upgrade(int id, UnityAction Success = null, UnityAction Failed = null)
        {
            if (IsMaxLevel())
            {
                Failed?.Invoke();
            }
            else
            {
                UpgradeCore();
                Save(id);

                Success?.Invoke();
            }
        }
        public virtual void Upgrade(UnityAction Success = null, UnityAction Failed = null)
        {
            if (IsMaxLevel())
            {
                Failed?.Invoke();
            }
            else
            {
                UpgradeCore();
                Save();

                Success?.Invoke();
            }
        }

        protected abstract void UpgradeCore();

        public virtual void UpgradeToMax(UnityAction OnUpgrade = null)
        {

        }
        public abstract bool IsMaxLevel();

        public abstract string ToJson();
    }
}
