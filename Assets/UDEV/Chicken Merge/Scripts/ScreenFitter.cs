using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ScreenFitter : MonoBehaviour
    {
        public SpriteRenderer sp;
        public bool resetScale;
        public bool fitX;
        public bool fitY;
        public float offsetX;
        public float offsetY;
        public bool isOverride;

        private void Awake()
        {
            if (!isOverride)
                Helper.FitSpriteToScreen(sp, resetScale, fitX, fitY, offsetX, offsetY);
        }

        public void Fit()
        {
            Helper.FitSpriteToScreen(sp, resetScale, fitX, fitY, offsetX, offsetY);
        }
    }
}