using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UDEV
{
    public static class Helper
    {
        public static void AssignToRoot(Transform root, Transform obj, Vector3 scale)
        {
            obj.SetParent(root);

            obj.localPosition = Vector3.zero;

            obj.localScale = scale;

            obj.localRotation = Quaternion.Euler(0f, 0f, 0f);
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

        public static float UpgradeForm(int level)
        {
            return ((level) / 2 - 0.5f) * 0.5f;
        }

        public static Vector2 Get2DCamSize()
        {
            return new Vector2(2f * Camera.main.aspect * Camera.main.orthographicSize, 2f * Camera.main.orthographicSize);
        }
    }
}
