using System;
using UDEV.ActionEventDispatcher;
using UDEV.SPM;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ShieldVisual : MonoBehaviour, IActionEventDispatcher
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_getHitVfx;
        #region ACTION
        private Action<object> m_OnTakeDamage;
        #endregion

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        #region RegisterEvents
        public void RegisterEvents()
        {
            m_OnTakeDamage = (param) => OnTakeDamage();

            this.RegisterActionEvent(GameplayAction.SHIELD_HITTED, m_OnTakeDamage);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.SHIELD_HITTED, m_OnTakeDamage);
        }
        #endregion

        private void OnTakeDamage()
        {
            PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_getHitVfx, transform.position, Quaternion.identity);
        }
    }
}
