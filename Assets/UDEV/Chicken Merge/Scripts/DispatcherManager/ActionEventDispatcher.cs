using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ActionEventDispatcher
{
    public class ActionEventDispatcher : Singleton<ActionEventDispatcher>
    {
        private readonly Dictionary<string, System.Action<object>> _PoolEvent = new Dictionary<string, System.Action<object>>();

        private event System.Action<object> paramOut;

        public ActionEventDispatcher RegisterEvent<T>(T actionID, System.Action<object> action)
        {
            if(!typeof(T).IsEnum)
            {
                Debug.LogError(actionID + "Must be Enum Type !!!");
                return null;
            }

            if (_PoolEvent.TryGetValue(actionID.ToString(), out paramOut))
            {
                paramOut += action;

                _PoolEvent[actionID.ToString()] = paramOut;
            }
            else
            {
                paramOut += action;

                _PoolEvent.Add(actionID.ToString(), paramOut);
            }

            Debug.Log(string.Format("[Action Manager] Register the new event with ID: {0}",
                                        actionID.ToString()));

            return this;
        }

        public ActionEventDispatcher RemoveEvent<T>(T actionID, System.Action<object> action)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.LogError(actionID + "Must be Enum Type !!!");
                return null;
            }

            if (_PoolEvent.TryGetValue(actionID.ToString(), out paramOut))
            {
                paramOut -= action;

                _PoolEvent[actionID.ToString()] = paramOut;

                Debug.Log(string.Format("[Action Manager] Remove the event with ID: {0}",
                                            actionID.ToString()));
            }
            else
            {
                Debug.Log(string.Format("[Action Manager] Not Found the event with ID: {0}",
                                            actionID.ToString()));
            }

            return this;
        }

        public ActionEventDispatcher PostEvent<T>(T actionID, object param)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.LogError(actionID + "Must be Enum Type !!!");
                return null;
            }

            if (!_PoolEvent.TryGetValue(actionID.ToString(), out paramOut)) return this;

            if (ReferenceEquals(paramOut, null))
            {
                _PoolEvent.Remove(actionID.ToString());
                return this;
            }

            paramOut(param);

            Debug.Log(string.Format("[Action Manager] Post the event with ID: {0}",
                                        actionID.ToString()));
            return this;
        }

        public void Clear()
        {
            _PoolEvent.Clear();
        }
    }
}