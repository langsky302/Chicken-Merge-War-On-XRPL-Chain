using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ShieldDetection : MonoBehaviour
    {
        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private float m_detectDistance;
        [SerializeField] private Vector3 m_offset;
        private bool m_isDetected;

        public bool IsDetected { get => m_isDetected;}

        private void FixedUpdate()
        {
            Detect();
        }

        private void Detect()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + m_offset, Vector2.left, m_detectDistance, m_targetLayer);
            m_isDetected = hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 endPos = new Vector3(transform.position.x - m_detectDistance, transform.position.y, transform.position.z) + m_offset;
            Gizmos.DrawLine(transform.position, endPos);
        }
    }
}
