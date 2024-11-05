using UnityEngine;
using System.Collections;
using System;

namespace UDEV
{
    public class Timer
    {
        private static MonoBehaviour behaviour;

        public static void Schedule(MonoBehaviour _behaviour, float delay, Action task, Action beforeTask = null, bool unscaleTime = false)
        {
            behaviour = _behaviour;
            if(unscaleTime)
                behaviour.StartCoroutine(DoTaskUnscale(task, beforeTask, delay));
            else
                behaviour.StartCoroutine(DoTask(task, beforeTask, delay));
        }

        private static IEnumerator DoTask(Action task, Action beforeTask, float delay)
        {
            if (beforeTask != null)
                beforeTask.Invoke();

            yield return new WaitForSeconds(delay);

            if (task != null)
                task.Invoke();
        }

        private static IEnumerator DoTaskUnscale(Action task, Action beforeTask, float delay)
        {
            if (beforeTask != null)
                beforeTask.Invoke();

            yield return new WaitForSecondsRealtime(delay);

            if (task != null)
                task.Invoke();
        }
    }
}
