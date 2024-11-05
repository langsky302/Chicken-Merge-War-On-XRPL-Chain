using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UDEV.ChickenMerge
{
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] private GameScene m_finalScene;
        [SerializeField] private GameScene m_dataScene;
        private AsyncOperation m_async;

        public UnityEvent<float> OnLoading;

        private void Start()
        {
            Application.targetFrameRate = 60;
            m_async = SceneManager.LoadSceneAsync(m_dataScene.ToString(), LoadSceneMode.Additive);
        }

        private void Update()
        {
            OnLoading?.Invoke(m_async.progress);

            if (m_async.isDone)
            {
                Helper.LoadScene(m_finalScene.ToString());
            }
        }
    }
}
