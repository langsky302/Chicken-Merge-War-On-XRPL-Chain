using System;
using System.Collections.Generic;
using System.Linq;

namespace UDEV.ChickenMerge
{
    public static class TimeActionHelper
    {
        public static TimeActionUserData GetAction(List<TimeActionUserData> timeActions, string key)
        {
            string actName = key + "_time";

            if (timeActions != null && timeActions.Count > 0)
            {
                var result = timeActions.Where(a => string.Compare(a.key, actName) == 0).ToArray();

                if (result != null && result.Length > 0)
                    return result[0];
            }

            return null;
        }

        public static void UpdateAction(ref List<TimeActionUserData> timeActions, string key, string value)
        {
            string action = key + "_time";

            TimeActionUserData act = GetAction(timeActions, key);

            if (act != null)
            {
                act.value = value;
            }
            else
            {
                timeActions.Add(new TimeActionUserData(action, value));
            }
        }

        public static bool IsActionAvailable(List<TimeActionUserData> timeActions, String action, int time, bool availableFirstTime = true)
        {
            if (GetAction(timeActions, action) == null)
            {
                if (availableFirstTime == false)
                {
                    SetActionTime(ref timeActions, action);
                }
                return availableFirstTime;
            }

            int delta = (int)(TimeActionHelper.GetCurrentTime() - GetActionTime(timeActions, action));
            return delta >= time;
        }

        public static double GetActionDeltaTime(List<TimeActionUserData> timeActions, String action)
        {
            if (GetActionTime(timeActions, action) == 0)
                return 0;
            return TimeActionHelper.GetCurrentTime() - GetActionTime(timeActions, action);
        }

        public static void SetActionTime(ref List<TimeActionUserData> timeActions, String action)
        {
            UpdateAction(ref timeActions, action, TimeActionHelper.GetCurrentTime().ToString());
        }

        public static void SetActionTime(ref List<TimeActionUserData> timeActions, String action, double time)
        {
            UpdateAction(ref timeActions, action, time.ToString());
        }

        public static double GetActionTime(List<TimeActionUserData> timeActions, String action)
        {
            var act = GetAction(timeActions, action);

            if (act != null)
            {
                return Double.Parse(act.value);
            }

            return 0;
        }

        public static double GetCurrentTime()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalSeconds;
        }

        public static double GetCurrentTimeInDays()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalDays;
        }

        public static double GetCurrentTimeInMills()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalMilliseconds;
        }

    }
}
