using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class SkillCollectedItemUI : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField] private Image m_skillIcon;
        [SerializeField] private Image m_skillCooldownImg;
        [SerializeField] private Text m_amountTxt;
        [SerializeField] private Text m_cooldownTxt;
        [SerializeField] private Image m_timeCountingFilled;
        [SerializeField] private Button m_btnComp;
        private SkillCollectedType m_skillType;
        private SkillController m_skillController;
        private int m_currentAmount;

        #region ACTION
        private Action<object> m_OnUpdateUI;
        private Action<object> m_OnUpdateAmount;
        #endregion

        public void Init(SkillCollectedType skillType)
        {
            m_skillType = skillType;
            m_skillController = SkillCollectedManager.Ins.GetSkillController(skillType);           
            m_skillController?.ForceStop();
            m_timeCountingFilled.transform.parent.gameObject.SetActive(false);

            UpdateUI();

            if (m_btnComp)
            {
                m_btnComp.onClick.RemoveAllListeners();
                m_btnComp.onClick.AddListener(TriggerSkill);
            }

            RegisterEvents();
        }
        #region EVENTS

        public void RegisterEvents()
        {
            UnregisterEvents();

            m_OnUpdateUI = param => UpdateUI();
            m_OnUpdateAmount = param => UpdateAmountTxt();

            this.RegisterActionEvent(GameplayAction.REWARD_DIALOG_CLOSED, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.SKILL_COLLECTED_UPDATE, m_OnUpdateUI);

            if (m_skillController == null) return;

            m_skillController.OnTriggerEnter.AddListener(UpdateQuest);
            m_skillController.OnCooldown.AddListener(UpdateCooldown);
            m_skillController.OnTriggerUpdate.AddListener(UpdateTimeTriggerCounting);
            m_skillController.OnCooldownStop.AddListener(UpdateUI);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.REWARD_DIALOG_CLOSED, m_OnUpdateUI);
            this.RemoveActionEvent(GameplayAction.SKILL_COLLECTED_UPDATE, m_OnUpdateUI);

            if (m_skillController == null) return;

            m_skillController.OnTriggerEnter.RemoveListener(UpdateQuest);
            m_skillController.OnCooldown.RemoveListener(UpdateCooldown);
            m_skillController.OnTriggerUpdate.RemoveListener(UpdateTimeTriggerCounting);
            m_skillController.OnCooldownStop.RemoveListener(UpdateUI);
        }
        #endregion

        public void UpdateUI()
        {
            if (m_skillController == null) return;
            
            UpdateAmountTxt();

            if (m_skillIcon)
                m_skillIcon.sprite = m_skillController.skillStat.skillIcon;
            
            UpdateCooldown();
            UpdateTimeTriggerCounting();
            gameObject.SetActive(m_currentAmount > 0 || m_skillController.IsCooldown);
        }

        private void UpdateAmountTxt()
        {
            UpdateNewAmountAdded();

            if (m_amountTxt)
            {
                string content = "";
                if (SkillCollectedManager.Ins.IsMaxCapacity(m_skillType))
                {
                    content = "Max";
                }else
                {
                    content = $"x{m_currentAmount}";
                }

                m_amountTxt.text = content;
            }
        }

        private void UpdateCooldown()
        {
            if (m_cooldownTxt)
                m_cooldownTxt.text = m_skillController.CooldownTime.ToString("f1");

            float cooldownProgress = m_skillController.cooldownProgress;

            if (m_skillCooldownImg)
            {
                m_skillCooldownImg.gameObject.SetActive(m_skillController.IsCooldown);
                m_skillCooldownImg.fillAmount = cooldownProgress;
            }
        }

        private void UpdateTimeTriggerCounting()
        {
            if(m_skillController == null || m_timeCountingFilled == null) return;

            float triggerProgress = m_skillController.TriggerProgress;
            m_timeCountingFilled.fillAmount = triggerProgress;
            m_timeCountingFilled.transform.parent.gameObject.
                SetActive(m_skillController.IsTriggered);
        }

        private void UpdateNewAmountAdded()
        {
            var newAmount = SkillCollectedManager.Ins.GetInGameSkillAmount(m_skillType);
            var amountDifferent = newAmount - m_currentAmount;
            if (amountDifferent > 0)
            {
                gameObject.SetActive(true);
                TweenUltis.ScaleBlink(transform, 1f, 1.2f, 0.3f);
            }
            m_currentAmount = newAmount;
        }

        private void TriggerSkill()
        {
            if (m_skillController == null) return;
            m_skillController.Trigger();
            AudioBase.Ins?.PlaySound(m_skillController.skillStat.triggerSoundFx);
        }

        private void UpdateQuest()
        {
            DataGroup.Ins.UpdateQuests(QuestType.UseSkill, 1);
        }

        private void OnDisable()
        {
            TweenUltis.KillTween(transform);
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }
    }
}
