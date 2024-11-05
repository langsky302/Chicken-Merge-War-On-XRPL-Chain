using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UDEV.DMTool
{
    [RequireComponent(typeof(DialogAnim))]
    public class Dialog : MonoBehaviour
    {
        public Text title, message;
        public Action<Dialog> onDialogOpened;
        public Action<Dialog> onDialogClosed;
        public Action onDialogCompleteClosed;
        public Action<Dialog> onButtonCloseClicked;
        public DialogType dialogType;
        public bool enableEscape = true;

        private AnimatorStateInfo info;
        private bool isShowing;
        private DialogAnim m_anim;

        public DialogAnim Anim { get => m_anim;}

        protected virtual void Awake()
        {
            m_anim = GetComponent<DialogAnim>();
        }

        protected virtual void Start()
        {
            onDialogCompleteClosed += OnDialogCompleteClosed;
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        private void Update()
        {
            if (enableEscape && Input.GetKeyDown(KeyCode.Escape) && isShowing)
            {
                DialogDB.Ins.current.Close();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
#if USE_DOTWEEN
            if (m_anim)
            {
                m_anim.ShowInAnim(() =>
                {
                    isShowing = true;
                    
                });
            }else
            {
                isShowing = true;
            }

            onDialogOpened(this);
#else

            onDialogOpened(this);
            isShowing = true;
#endif
        }

        public virtual void Close()
        {
            if (isShowing == false) return;
            isShowing = false;
#if USE_DOTWEEN
            if (m_anim)
            {
                m_anim.ShowOutAnim(() =>
                {
                    DoClose();
                });
            }
            else
            {
                DoClose();
            }

            onDialogClosed(this);
#else
            onDialogClosed(this);
            DoClose();
#endif
        }

        private void DoClose()
        {
            if (onDialogCompleteClosed != null) onDialogCompleteClosed();
            Destroy(gameObject);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            isShowing = false;
        }

        public bool IsShowing()
        {
            return isShowing;
        }

        public virtual void OnDialogCompleteClosed()
        {
            onDialogCompleteClosed -= OnDialogCompleteClosed;
        }
    }
}
