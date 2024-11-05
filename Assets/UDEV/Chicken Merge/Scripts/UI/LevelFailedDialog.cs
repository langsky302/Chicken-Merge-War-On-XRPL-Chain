using UDEV.DMTool;
using UnityEngine.UI;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LevelFailedDialog : Dialog
    {
        public Button replayBtn;
        public Button homeBtn;

        public override void Show()
        {
            base.Show();
            AdmobController.Ins.ShowInterstitial();
        }

        public void BackHome()
        {
            Close();
            Helper.LoadScene(GameScene.Menu.ToString(), true);
        }

        public void Replay()
        {
            Close();
            GameController.Ins?.Replay();
        }
    }

}