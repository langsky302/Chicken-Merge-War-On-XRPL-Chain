using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ActionEventDispatcher
{
    public static class ActionManagerExtension
    {
        public static ActionEventDispatcher RegisterActionEvent<T>(this MonoBehaviour mono, T actionID, System.Action<object> callback)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.LogError(actionID + "Must be Enum Type !!!");
                return null;
            }

            if (ReferenceEquals(ActionEventDispatcher.Ins, null))
            {

                Debug.LogError("[Action Event] Action Event Dispatcher Is Null!");
                return null;
            }

            return ActionEventDispatcher.Ins.RegisterEvent(actionID, callback); ;
        }

        public static ActionEventDispatcher RemoveActionEvent<T>(this MonoBehaviour mono, T actionID, System.Action<object> callback)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.LogError(actionID + "Must be Enum Type !!!");
                return null;
            }

            if (ReferenceEquals(ActionEventDispatcher.Ins, null))
            {

                Debug.LogError("[Action Event] Action Event Dispatcher Is Null!");
                return null;
            }

            return ActionEventDispatcher.Ins.RemoveEvent(actionID, callback); ;
        }

        public static ActionEventDispatcher PostActionEvent<T>(this MonoBehaviour mono, T actionID, object param = null)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.LogError(actionID + "Must be Enum Type !!!");
                return null;
            }

            if (ReferenceEquals(ActionEventDispatcher.Ins, null))
            {
                Debug.LogError("[Action Event] Action Event Dispatcher Is Null!");
                return null;
            }

            return ActionEventDispatcher.Ins.PostEvent(actionID, param); ;
        }

        public static void Clear(this MonoBehaviour mono)
        {
            ActionEventDispatcher.Ins.Clear();
        }
    }
}
