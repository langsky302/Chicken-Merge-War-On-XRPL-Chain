using UnityEngine.UIElements;

namespace UDEV.GameSettings.Editor
{
    public static class GameSettings_UI_Utils
    {
        public static void ShowElement(VisualElement element, bool isShow)
        {
            if (isShow)
            {
                element.RemoveFromClassList("hide");
                element.AddToClassList("show");
            }
            else
            {
                element.RemoveFromClassList("show");
                element.AddToClassList("hide");
            }

        }
    }
}
