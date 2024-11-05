using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UDEV.DMTool
{
    public class BtnBase : MonoBehaviour
    {
        protected Button button;

        protected virtual void Start()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }

        public virtual void OnButtonClick()
        {

        }
    }
}
