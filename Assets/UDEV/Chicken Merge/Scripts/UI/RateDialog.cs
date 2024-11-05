namespace UDEV.ChickenMerge
{
    public class RateDialog : YesNoDialog
    {
        public override void Show()
        {
            base.Show();
        }

        public override void OnYesClick()
        {
            base.OnYesClick();
            UserDataHandler.Ins.setting.isUserRated = true;
            Helper.OpenStore();
        }

        public override void OnNoClick()
        {
            base.OnNoClick();
        }
    }
}
