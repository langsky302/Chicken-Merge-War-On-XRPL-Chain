using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class FollowCamOffset : MonoBehaviour
    {
        public Vector3 offset;
        private Camera m_cam;

        private void Awake()
        {
            m_cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(transform.position.x, m_cam.transform.position.y, 0f) + offset;
        }
    }
}
