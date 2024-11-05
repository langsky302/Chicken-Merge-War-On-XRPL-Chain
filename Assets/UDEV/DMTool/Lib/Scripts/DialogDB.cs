using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UDEV.DMTool
{
    public enum ShowType
    {
        NOT_SHOW_WHEN_OTHER_ACTIVE,
        REPLACE_CURRENT,
        STACK,
        SHOW_PREVIOUS,
        OVER_CURRENT,
    };

    public class DialogDB : Singleton<DialogDB>
    {
        public DialogTypesSO dialogTypeSO;

        [HideInInspector]
        public Dialog current;
        
        public Action onDialogsOpened;
        public Action onDialogsClosed;
        public Stack<Dialog> dialogs = new Stack<Dialog>();

        /// <summary>
        /// Show Dialog
        /// </summary>
        /// <param name="type">Type of Dialog</param>
        public void Show(int type)
        {
            Show((DialogType)type, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        /// <summary>
        /// Show Dialog
        /// </summary>
        /// <param name="type">Type of Dialog</param>
        /// <param name="option">Show Type Option</param>
        public void Show(DialogType type, ShowType option = ShowType.REPLACE_CURRENT)
        {
            Dialog dialog = GetDialog(type);
            Show(dialog, option);
        }

        /// <summary>
        /// Show Dialog
        /// </summary>
        /// <param name="dialog">Dialog</param>
        /// <param name="option">Show Type Option</param>
        public void Show(Dialog dialog, ShowType option = ShowType.REPLACE_CURRENT)
        {
            if (current != null)
            {
                if (option == ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE)
                {
                    Destroy(dialog.gameObject);
                    return;
                }
                else if (option == ShowType.REPLACE_CURRENT)
                {
                    current.Close();
                }
                else if (option == ShowType.STACK)
                {
                    current.Hide();
                }
            }

            current = dialog;
            if (option != ShowType.SHOW_PREVIOUS)
            {
                current.onDialogOpened += OnOneDialogOpened;
                current.onDialogClosed += OnOneDialogClosed;
                dialogs.Push(current);
            }

            current.Show();

            if (onDialogsOpened != null)
                onDialogsOpened();
        }

        /// <summary>
        /// Get Dialog from DB
        /// </summary>
        /// <param name="type">Type of Dialog</param>
        /// <returns></returns>
        public Dialog GetDialog(DialogType type)
        {
            Dialog dialog = dialogTypeSO.dialogs[(int)type];
            dialog.dialogType = type;
            return (Dialog)Instantiate(dialog, transform.position, transform.rotation);
        }

        /// <summary>
        /// Close activating dialog
        /// </summary>
        public void CloseCurrent()
        {
            if (current != null)
                current.Close();
        }

        /// <summary>
        /// Close a dialog
        /// </summary>
        /// <param name="type">Type of Dialog</param>
        public void Close(DialogType type)
        {
            if (current == null) return;
            if (current.dialogType == type)
            {
                current.Close();
            }
        }

        /// <summary>
        /// Check a dialog show or not
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsShowing()
        {
            return current != null;
        }

        /// <summary>
        /// Check a dialog show or not
        /// </summary>
        /// <param name="type">Type of Dialog</param>
        /// <returns>Bool</returns>
        public bool IsShowing(DialogType type)
        {
            if (current == null) return false;
            return current.dialogType == type;
        }

        /// <summary>
        /// When dialog open
        /// </summary>
        /// <param name="dialog">Dialog</param>
        private void OnOneDialogOpened(Dialog dialog)
        {

        }

        /// <summary>
        /// When dialog was closed
        /// </summary>
        /// <param name="dialog">Dialog</param>
        private void OnOneDialogClosed(Dialog dialog)
        {
            if (current)
            {
                if (current != dialog) return;

                current = null;
                dialogs.Pop();

                if (onDialogsClosed != null)
                {
                    onDialogsClosed();
                }

                if (dialogs.Count > 0)
                {
                    Show(dialogs.Peek(), ShowType.SHOW_PREVIOUS);
                }
            }
            else
            {
                onDialogsClosed?.Invoke();
            }
        }
    }
}
