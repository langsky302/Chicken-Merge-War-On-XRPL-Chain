using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class UnFollowCam : MonoBehaviour
    {
        Vector3 m_startingPos;

        private void Awake()
        {
            m_startingPos = transform.position;
        }

        private void Update()
        {
            transform.position = m_startingPos;
        }
    }
}
