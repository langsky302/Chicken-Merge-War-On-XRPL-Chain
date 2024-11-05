using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UDEV.ChickenMerge
{
    public static class Helper
    {
        #region Animation
        public static void PlayAnim(Animator anim, string stateName, int layerIndex = 0)
        {
            if (IsAnimCanPlayState(anim, stateName, layerIndex))
            {
                anim.Play(stateName);
            }
        }

        public static bool IsAnimStateActive(Animator animator, string stateName, int layerIndex = 0)
        {
            if (animator)
                return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);

            return true;
        }

        public static bool IsAnimCanPlayState(Animator animator, string stateName, int layerIndex = 0)
        {
            if (animator)
                return !IsAnimStateActive(animator, stateName, layerIndex)
                && animator.HasState(layerIndex, Animator.StringToHash(stateName));

            return false;
        }

        public static AnimationClip GetClip(Animator anim, string stateName)
        {
            if (anim)
            {
                int maxState = anim.runtimeAnimatorController.animationClips.Length;

                var states = anim.runtimeAnimatorController.animationClips;

                for (int i = 0; i < maxState; i++)
                {
                    if (string.Compare(states[i].name, stateName) == 0)
                    {
                        return states[i];
                    }
                }
            }

            return null;
        }

        #endregion

        public static string GenerateUID()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            int z1 = UnityEngine.Random.Range(0, 1000000);
            int z2 = UnityEngine.Random.Range(0, 1000000);
            string uid = currentEpochTime + "" + z1 + "" + z2;
            return uid;
        }

        public static string TimeConvert(double time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);

            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        public static void ClearChilds(Transform root)
        {
            if (root)
            {
                int childs = root.childCount;

                if (childs > 0)
                {
                    for (int i = 0; i < childs; i++)
                    {
                        var child = root.GetChild(i);

                        if (child)
                            MonoBehaviour.Destroy(child.gameObject);
                    }
                }
            }
        }

        public static void AssignToRoot(Transform root, Transform obj, Vector3 pos, Vector3 scale, bool isChange = false)
        {
            obj.SetParent(root, isChange);

            obj.localPosition = pos;

            if (!isChange)
                obj.localScale = scale;

            if (!isChange)
                obj.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        public static void LoadScene(string sceneName, bool useScreenFader = false)
        {
            if (useScreenFader)
            {
                ScreenFader.Ins?.GotoScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public static void ReloadScene(bool useScreenFader = true)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            LoadScene(currentScene, useScreenFader);
        }

        public static void OpenStore()
        {

        }

        public static Vector2 Get2DCamSize()
        {
            return new Vector2(2f * Camera.main.aspect * Camera.main.orthographicSize, 2f * Camera.main.orthographicSize);
        }

        public static Color ChangAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static void FitSpriteToScreen(SpriteRenderer sp, bool resetScale = true, bool fitX = true, bool fixY = true, float offsetX = 0, float offsetY = 0)
        {
            if (resetScale)
                sp.transform.localScale = Vector3.one;

            var width = sp.sprite.bounds.size.x;
            var height = sp.sprite.bounds.size.y;

            var worldScreenHeight = Camera.main.orthographicSize * 2.0;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            double scaleX = worldScreenWidth / width;
            double scaleY = worldScreenHeight / height;

            if (fitX)
                sp.transform.localScale = new Vector3((float)scaleX + offsetX, sp.transform.localScale.y + offsetY, sp.transform.localScale.z);

            if (fixY)
                sp.transform.localScale = new Vector3(sp.transform.localScale.x + offsetX, (float)scaleY + offsetY, sp.transform.localScale.z);
        }

        public static float UpgradeForm(int level, float factor)
        {
            return (level / factor) * 0.5f;
        }

        public static float MaxUpgradeValue(float factor, float oldValue, float upValueRate, int level, bool isPercent = false)
        {
            float maxValue = 0;

            if (isPercent)
            {
                for (int i = 0; i < level; i++)
                {
                    maxValue += (UpgradeForm(i, factor) * upValueRate) / 100;
                }
            }
            else
            {
                for (int i = 0; i < level; i++)
                {
                    maxValue += (UpgradeForm(i, factor) * upValueRate);
                }
            }

            maxValue += oldValue;

            return maxValue;
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

        public static T GetRandomEnum<T>(T except)
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));

            while (V.Equals(except))
            {
                V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            }
            return V;
        }

        public static T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        public static int GetEnumCounting<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            return A.Length;
        }

        public static T GetRandom<T>(params T[] arr)
        {
            if (arr == null || arr.Length <= 0) return default;

            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }

        public static Vector3 GetUIWorldPoint(RectTransform rect)
        {
            if (rect == null) return Vector3.zero;

            Vector3[] v = new Vector3[4];
            rect.GetWorldCorners(v);
            return v[0];
        }

        public static void RotateFollowDirection(Transform start, Transform end, Transform transform)
        {
            var dir = end.position - start.position;
            dir.Normalize();
            var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public static string BigCurrencyFormat(double amount)
        {
            int length = amount.ToString().Length;
            if (length > 6)
            {
                return amount.ToString("0,,.##M");
            }
            else if (length > 4)
            {
                return amount.ToString("0,.##K");
            }

            return amount.ToString();
        }
    }
}
