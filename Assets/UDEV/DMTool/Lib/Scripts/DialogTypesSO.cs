using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.DMTool
{
    [CreateAssetMenu(fileName = "New Dialog Types", menuName = "UDEV/DMTool/Create Types")]
    public class DialogTypesSO : ScriptableObject
    {
        [HideInInspector]
        public Dialog[] dialogs;
    }
}
