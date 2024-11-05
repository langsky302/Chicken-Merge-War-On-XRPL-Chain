using UDEV.WaveManagerToolkit;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LevelManagerSO : ScriptableObject
    {
        [Header("Level Waves:")]
        public WaveTK_WaveController[] waveControllers; 
    }
}
