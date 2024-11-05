using UDEV.DMTool;
using UnityEngine.UI;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LevelCompleteDialog : Dialog
    {

        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Completed);
            title.SetText($"LEVEL {UserDataHandler.Ins.curLevelId} CLEARED");
            AdmobController.Ins.ShowInterstitial();
        }

        public void NextLevel()
        {           
            Close();
            if (DataGroup.Ins.IsMaxGameLevel)
            {
                Helper.LoadScene(GameScene.Menu.ToString(), true);
            }
            else
            {
                Helper.ReloadScene();
            }
        }

        public void BackHome()
        {
            Close();
            Helper.LoadScene(GameScene.Menu.ToString(), true);
        }

        public void Replay()
        {
            Close();
            Helper.ReloadScene();
        }        
    }

}