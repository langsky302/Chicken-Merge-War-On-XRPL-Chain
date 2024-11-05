using UDEV.DMTool;
using UDEV.WaveManagerToolkit;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LevelSelectionDialog : Dialog
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private LevelItemUI m_itemUIPrefab;

        private WaveTK_WaveController[] m_levelWaves;

        public override void Show()
        {
            base.Show();

            if (DataGroup.Ins == null || DataGroup.Ins.levelManagerData == null) return;

            m_levelWaves = DataGroup.Ins.levelManagerData.waveControllers;

            UpdateUI();
        }

        public void UpdateUI()
        {
            Helper.ClearChilds(m_gridRoot);
            if(m_levelWaves == null || m_levelWaves.Length <= 0) return;

            for(int i = 0; i < m_levelWaves.Length; i++)
            {
                int levelId = i;
                var levelUserData = UserDataHandler.Ins.GetLevelData(levelId);
                var itemUIClone = Instantiate(m_itemUIPrefab);
                itemUIClone.UpdateUI(levelId);
                Helper.AssignToRoot(m_gridRoot, itemUIClone.transform, Vector3.zero, Vector3.one);

                if(itemUIClone.levelBtn)
                {
                    itemUIClone.levelBtn.onClick.RemoveAllListeners();
                    itemUIClone.levelBtn.onClick.AddListener(() => LevelBtnEvent(levelUserData, levelId));
                }
            }
        }

        private void LevelBtnEvent(ItemStateUserData levelUserData, int levelId)
        {
            DataGroup.Ins?.SelectLevel(levelUserData, levelId, () =>
            {
                DialogDB.Ins.current.onDialogCompleteClosed += () =>
                {
                    Helper.LoadScene(GameScene.Gameplay.ToString(), true);
                };
                DialogDB.Ins.current.Close();
            });
        }
    }
}
