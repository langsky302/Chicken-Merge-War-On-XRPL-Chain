using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UDEV.DMTool
{
    public class DialogOverlay : MonoBehaviour
    {
        private Image overlay;

        private void Awake()
        {
            overlay = GetComponent<Image>();
        }

        private void Start()
        {
            DialogDB.Ins.onDialogsOpened += OnDialogOpened;
            DialogDB.Ins.onDialogsClosed += OnDialogClosed;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            overlay.enabled = false;
            DialogDB.Ins.current = null;
            DialogDB.Ins.dialogs.Clear();
        }

        private void OnDialogOpened()
        {
            overlay.enabled = true;
        }

        private void OnDialogClosed()
        {
            overlay.enabled = false;
        }
    }
}
