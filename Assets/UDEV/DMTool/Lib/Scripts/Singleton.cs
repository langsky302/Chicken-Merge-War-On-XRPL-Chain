using UnityEngine;

// a Generic Singleton class

namespace UDEV
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public string sceneName;
        protected int numofEnterScene;

        // private static instance
        static T m_ins;

        // public static instance used to refer to Singleton (e.g. MyClass.Instance)
        public static T Ins
        {
            get
            {
                // return the singleton instance
                return m_ins;
            }

            set => m_ins = value;
        }

        protected virtual void Awake()
        {
            MakeSingleton(true);
        }

        protected void MakeSingleton(bool destroyOnload)
        {
            if (m_ins == null)
            {
                m_ins = this as T;
                if (destroyOnload)
                {
                    var root = transform.root;

                    if (root != transform)
                    {
                        DontDestroyOnLoad(root);
                    }
                    else
                    {
                        DontDestroyOnLoad(this.gameObject);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}