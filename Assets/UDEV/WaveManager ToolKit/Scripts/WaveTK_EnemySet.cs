using System.Collections.Generic;
using UnityEngine;

namespace UDEV.WaveManagerToolkit
{
    public class WaveTK_EnemySet : ScriptableObject
    {
        public List<WaveTK_EnemyWeighted> enemies = new List<WaveTK_EnemyWeighted>();
        [HideInInspector]
        public int totalWeight;

        public void UpdateTotalWeight()
        {
            int sum = 0;
            foreach (var i in enemies)
            {
                sum += i.weight;
            }
            totalWeight = sum;
        }

        public string GetEnemy()
        {
            List<WaveTK_EnemyWeighted> s = new List<WaveTK_EnemyWeighted>(enemies);
            s.Sort(
                delegate (WaveTK_EnemyWeighted x, WaveTK_EnemyWeighted y) 
                { return x.weight.CompareTo(y.weight); }
                );

            int ran = Random.Range(0, totalWeight);

            string selected = s[enemies.Count - 1].enemyPoolKey;
            foreach (var i in s)
            {
                if (ran < i.weight)
                {
                    selected = i.enemyPoolKey;
                    break;
                }
                ran -= i.weight;
            }
            return selected;
        }
    }
}
