using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class OpenChestButton : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField] private Text m_chestRemainingCountingTxt;

        private Action<object> m_OnEnemyDie;

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnEnemyDie = param => UpdateUI();

            this.RegisterActionEvent(EnemyAction.DIE, m_OnEnemyDie);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(EnemyAction.DIE, m_OnEnemyDie);
        }
        #endregion

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            StopAllCoroutines();

            int chestRemaining = ChestController.Ins.ChestRemaining;
            if (m_chestRemainingCountingTxt != null)
                m_chestRemainingCountingTxt.text = "x" + chestRemaining.ToString("n0");

            if(chestRemaining > 0)
            {
                StartCoroutine(
                    TweenUltis.ScaleBlinkLoop(
                        m_chestRemainingCountingTxt.transform, 1f, 1.15f, 0.2f, 0.1f)
                    );
            }else
            {
                StopAllCoroutines();
            }
        }

        private void OnDestroy()
        {
            TweenUltis.KillAllTweens();
        }
    }
}
