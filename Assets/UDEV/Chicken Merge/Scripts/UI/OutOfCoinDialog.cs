using UDEV.DMTool;

namespace UDEV.ChickenMerge
{
    public class OutOfCoinDialog : Dialog
    {
        public override void Show()
        {
            GameController.ChangeState(GameState.Pausing);
            base.Show();
        }

        public void GotoShop()
        {
            DialogDB.Ins.Show(DialogType.IAPShop);
        }

        public override void Close()
        {
            base.Close();
            GameController.ChangeState(GameState.Playing);
        }
    }
}
