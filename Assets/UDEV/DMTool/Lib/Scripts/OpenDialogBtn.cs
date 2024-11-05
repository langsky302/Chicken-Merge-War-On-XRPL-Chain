using UnityEngine;
using System.Collections;

namespace UDEV.DMTool
{
    public class OpenDialogBtn : BtnBase
    {

        public DialogType dialogType;
        public ShowType dialogShow;

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            DialogDB.Ins.Show(dialogType, dialogShow);
        }
    }
}
