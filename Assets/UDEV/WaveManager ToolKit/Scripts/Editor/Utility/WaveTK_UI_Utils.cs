using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit
{
    public class WaveTK_UI_Utils
    {
        public static void ShowElement(VisualElement element, bool isShow)
        {
            if (isShow)
            {
                element.RemoveFromClassList("hide");
                element.AddToClassList("show");
            }
            else {
                element.RemoveFromClassList("show");
                element.AddToClassList("hide");
            }
            
        }
    }
}
