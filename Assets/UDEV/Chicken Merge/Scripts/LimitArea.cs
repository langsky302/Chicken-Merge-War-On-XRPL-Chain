using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LimitArea : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collisionTarget)
        {
            collisionTarget.gameObject.SetActive(false);
        }
    }
}
