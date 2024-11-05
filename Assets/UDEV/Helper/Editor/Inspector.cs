using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UDEV.UI.Editor
{
    public class Inspector : InspectorElement
    {
        public new class UxmlFactory : UxmlFactory<Inspector, UxmlTraits> {}
    }
}