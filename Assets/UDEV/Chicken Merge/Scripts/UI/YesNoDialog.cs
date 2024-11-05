using System;
using UDEV.DMTool;
using UDEV.ChickenMerge;

namespace UDEV
{
    public class YesNoDialog : Dialog
    {
        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Pausing);
        }

        public Action onYesClick;
        public Action onNoClick;
        public virtual void OnYesClick()
        {
            if (onYesClick != null) onYesClick();
            Close();
        }

        public virtual void OnNoClick()
        {
            if (onNoClick != null) onNoClick();
            Close();
        }

        public override void Close()
        {
            base.Close();
            GameController.RevertState();
        }
    }
}
