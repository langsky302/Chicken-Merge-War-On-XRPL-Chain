using UDEV.SPM;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class GunVisual : MonoBehaviour
    {
        [Header("Kickback Visual Settings:")]
        [Range(0.1f, 1f)]
        [SerializeField] private float m_kickBackForce = 0.15f;
        [Range(5f, 100f)]
        [SerializeField] private float m_kickBackForceMulti = 10f;
        [SerializeField] private bool m_yKickback;
        [PoolerKeys(target = PoolerTarget.NONE)]
        [SerializeField] private string m_impackVfxPoolKey;

        private GunController m_gunCtr;
        private Vector3 m_startPos;

        private UnityAction<Transform> m_OnShootAction;

        #region EVENTS
        private void AddEventsToGun()
        {
            if (m_gunCtr == null) return;

            m_OnShootAction += SpawnImpactEffect;
            m_OnShootAction += (trans) => Kickback();

            m_gunCtr.OnShoot.AddListener(m_OnShootAction);
        }

        private void RemoveEventsFromGun()
        {
            if (m_gunCtr == null) return;

            m_gunCtr.OnShoot.RemoveListener(m_OnShootAction);
        }
        #endregion

        private void Awake()
        {
            m_gunCtr = GetComponent<GunController>();
            m_startPos = transform.localPosition;
        }

        private void OnEnable()
        {
            AddEventsToGun();
        }

        private void OnDisable()
        {
            RemoveEventsFromGun();
        }

        private void Update()
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_startPos, m_kickBackForce * m_kickBackForceMulti * Time.deltaTime);
        }

        private void SpawnImpactEffect(Transform shootingPoint)
        {
            var impacClone = PoolersManager.Ins?.Spawn(PoolerTarget.NONE, m_impackVfxPoolKey, shootingPoint.position, Quaternion.identity);
            if (impacClone)
            {
                impacClone.transform.SetParent(null);
                impacClone.transform.SetParent(shootingPoint);
                impacClone.transform.localPosition = Vector3.zero;
                impacClone.transform.localScale = Vector3.one * 0.4f;
            }
        }

        private void Kickback()
        {
            transform.localPosition = m_startPos;
            if (m_yKickback)
            {
                transform.localPosition += new Vector3(m_kickBackForce, Random.Range(-0.035f, 0.025f), 0f);
            }else
            {
                transform.localPosition += Vector3.right * m_kickBackForce;
            }
        }
    }
}
