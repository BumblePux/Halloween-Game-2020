using System.Collections;
using UnityEngine;

namespace BumblePux.Halloween2020
{
    public class WaveSpawner : MonoBehaviour
    {
		//--------------------------------------------------------------------------------
        [System.Serializable]
        public class Wave
        {
            public EnemyController[] Enemies;
            public int NumEnemies;
            public float TimeBetweenSpawns;
        }

        //--------------------------------------------------------------------------------
        public Wave[] Waves;
        public Transform[] SpawnPoints;
        public float TimeBetweenWaves;
        public bool FinishedSpawning;

        private Wave currentWave;

        //--------------------------------------------------------------------------------
        public IEnumerator SpawnNextWave(int waveIndex)
        {
            FinishedSpawning = false;
            yield return new WaitForSeconds(TimeBetweenWaves);
            StartCoroutine(SpawnWave(waveIndex));
        }

        //--------------------------------------------------
        private IEnumerator SpawnWave(int index)
        {
            currentWave = Waves[index];

            for (int i = 0; i < currentWave.NumEnemies; i++)
            {
                EnemyController randomEnemy = currentWave.Enemies[Random.Range(0, currentWave.Enemies.Length)];
                Transform randomSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                Instantiate(randomEnemy, randomSpawnPoint.position, randomSpawnPoint.rotation);

                if (i == currentWave.NumEnemies - 1)
                    FinishedSpawning = true;
                else
                    FinishedSpawning = false;

                yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);
            }
        }
    }
}