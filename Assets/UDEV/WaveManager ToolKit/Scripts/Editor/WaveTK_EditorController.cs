using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_EditorController
    {
        private WaveTK_WaveController m_waveController;
        private VisualElement m_uiRootElement;

        private string m_waveIdFilePath = Application.dataPath + WaveTK_Const.EDITOR_DATA_PATH;
        private int m_lastWaveId;

        public WaveTK_WaveController WaveController { get => m_waveController;}
        public VisualElement UiRootElement { get => m_uiRootElement;}

        public WaveTK_EditorController()
        {
        }

        public WaveTK_EditorController(WaveTK_WaveController waveController, VisualElement uiRootElement)
        {
            m_waveController = waveController;
            m_uiRootElement = uiRootElement;
        }

        public int GetIdFromFilename(string fileName, string searchStr)
        {
            string id = fileName.Replace(searchStr, "").Trim();
            return int.Parse(id);
        }

        public void DeleteNullWave()
        {
            var waves = m_waveController.waves;
            int waveCounting = waves.Count;
            for (int i = 0; i < waveCounting; i++)
            {
                var wave = waves[i];
                if (wave != null) continue;
                waves.RemoveAt(i);
                waveCounting = waves.Count;
            }
            EditorUtility.SetDirty(m_waveController);
        }

        public void AddNewWave(UnityAction OnCompleted = null)
        {
            if (m_waveController == null) return;
            m_lastWaveId = Utils.LoadDataFromFile(m_waveIdFilePath + "WaveIds.dat", m_lastWaveId);
            m_lastWaveId++;
            var newWave = WMN_ToolKit_SO_Utils<WaveTK_Wave>.CreateSO($"Wave {m_lastWaveId}", WaveTK_Utils.holdingAssetPath);
            m_waveController.waves.Add(newWave);
            EditorUtility.SetDirty(m_waveController);
            OnCompleted?.Invoke();

            Utils.SaveDataToFile(m_waveIdFilePath, "WaveIds.dat", m_lastWaveId);
        }

        public void DeleteWave(int waveIndex, UnityAction OnCompleted = null)
        {
            if (m_waveController == null) return;
            var wave = m_waveController.waves[waveIndex];
            WMN_ToolKit_SO_Utils<WaveTK_Wave>.DeleteSO(wave);
            m_waveController.waves.RemoveAt(waveIndex);
            EditorUtility.SetDirty(m_waveController);
            OnCompleted?.Invoke();
        }

        public void AddNewSpawner(WaveTK_Wave wave, UnityAction OnCompleted = null)
        {
            if (wave == null) return;
            wave.spawners.Add(new WaveTK_Spawner());
            EditorUtility.SetDirty(wave);
            OnCompleted?.Invoke();
        }

        public void DeleteSpawner(WaveTK_Wave wave, int spawnerIndex, UnityAction OnCompleted = null)
        {
            if (wave == null) return;
            wave.spawners.RemoveAt(spawnerIndex);
            EditorUtility.SetDirty(wave);
            OnCompleted?.Invoke();
        }

        public void AddNewSpawnerEnemy(WaveTK_EnemySet enemySet, UnityAction OnCompleted = null)
        {
            if (enemySet == null) return;
            var enemies = enemySet.enemies;
            enemies.Add(new WaveTK_EnemyWeighted());
            EditorUtility.SetDirty(enemySet);
            OnCompleted?.Invoke();            
        }

        public void DeleteSpawnerEnemy(WaveTK_EnemySet enemySet, int enemyIndex, UnityAction OnCompleted = null)
        {
            if (enemySet == null) return;
            var enemies = enemySet.enemies;
            enemies.RemoveAt(enemyIndex);
            EditorUtility.SetDirty(enemySet);
            OnCompleted?.Invoke();
        }
    }
}
