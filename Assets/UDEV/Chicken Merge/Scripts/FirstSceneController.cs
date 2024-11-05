using UnityEngine;
using UDEV.DMTool;
using UDEV.ActionEventDispatcher;

namespace UDEV.ChickenMerge
{
    public class FirstSceneController : MonoBehaviour
    {
        private void Update()
        {
#if !UNITY_WSA
            if (Input.GetKeyDown(KeyCode.Escape) && !DialogDB.Ins.IsShowing())
            {
                DialogDB.Ins.Show(DialogType.QuitGame);
            }
#endif
        }
    }
}