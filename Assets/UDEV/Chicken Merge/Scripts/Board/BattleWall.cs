using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class BattleWall : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collisionTarget)
        {
            if (collisionTarget.gameObject.CompareTag(GameTag.Enemy.ToString()))
            {
                this.PostActionEvent(GameState.Gameover);
            }
        }
    }
}
