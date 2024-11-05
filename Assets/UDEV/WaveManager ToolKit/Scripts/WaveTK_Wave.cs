using System.Collections.Generic;
using UnityEngine;

namespace UDEV.WaveManagerToolkit
{
    public class WaveTK_Wave : ScriptableObject
    {
        public List<WaveTK_Spawner> spawners = new List<WaveTK_Spawner>();
        [HideInInspector]
        public int totalEnemy;
        [HideInInspector]
        public int enemyKilled;
    }
}
