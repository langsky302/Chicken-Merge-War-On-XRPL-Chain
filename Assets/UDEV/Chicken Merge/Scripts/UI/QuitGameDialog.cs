using UnityEngine;
using System.Collections;

namespace UDEV
{
    public class QuitGameDialog : YesNoDialog
    {
        protected override void Start()
        {
            base.Start();
            onYesClick = Quit;
            onNoClick = Close;
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}
