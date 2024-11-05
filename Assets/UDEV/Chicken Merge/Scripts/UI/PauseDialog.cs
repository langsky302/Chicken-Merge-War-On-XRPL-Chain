using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class PauseDialog : SettingBaseDialog
    {
        public override void Show()
        {
            base.Show();
            Time.timeScale = 0f;
            GameController.ChangeState(GameState.Playing);
        }

        public void BackHomeBtn()
        {
            Close();
            Helper.LoadScene(GameScene.Menu.ToString(), true);
        }

        public void ReplayBtn()
        {
            Close();
            GameController.Ins.Replay();
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1f;
            GameController.RevertState();
        }
    }
}
