using BumblePux.Halloween2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BumblePux
{
    public class WaveGameMode : MonoBehaviour
    {
        [Header("References")]
        public GameUI UI;

        [Header("Game Mode Settings")]
        public float StartDelay = 3f;

        [Header("Audio")]
        public AudioClip WaveModeClip;

        [Header("Wave Settings")]
        public int StartingWave = 0;

        private new AudioManager audio;
        private WaveSpawner waveSpawner;
        private PlayerController player;

        private int currentWaveIndex;
        private bool allWavesComplete;
        private List<EnemyController> enemies = new List<EnemyController>();

        //--------------------------------------------------------------------------------
        private void Start()
        {
            audio = AudioManager.Instance;
            waveSpawner = GetComponent<WaveSpawner>();
            player = FindObjectOfType<PlayerController>();

            StartCoroutine(StartGameLoop());
        }

        //--------------------------------------------------
        private IEnumerator StartGameLoop()
        {
            yield return StartCoroutine(GameSetup());
            yield return StartCoroutine(GameRunning());
            yield return StartCoroutine(GameOver());
        }

        //--------------------------------------------------
        private IEnumerator GameSetup()
        {
            audio.StopMusic(false);

            StartCoroutine(UI.SetWaveText("Wave 1"));

            yield return new WaitForSeconds(StartDelay);

            audio.PlayMusic(WaveModeClip);
            StartCoroutine(waveSpawner.SpawnNextWave(currentWaveIndex));
        }

        //--------------------------------------------------
        private IEnumerator GameRunning()
        {
            while(!allWavesComplete)
            {
                if (!player)
                {
                    break;
                }

                if (waveSpawner.FinishedSpawning && enemies.Count == 0)
                {
                    if (currentWaveIndex + 1 < waveSpawner.Waves.Length)
                    {
                        currentWaveIndex++;

                        if (currentWaveIndex + 1 < waveSpawner.Waves.Length)
                        {
                            StartCoroutine(UI.SetWaveText("Wave " + (currentWaveIndex + 1).ToString()));
                        }
                        else if (currentWaveIndex == waveSpawner.Waves.Length - 1)
                        {
                            StartCoroutine(UI.SetWaveText("Final Wave!"));
                        }

                        StartCoroutine(waveSpawner.SpawnNextWave(currentWaveIndex));
                    }
                    else
                    {
                        allWavesComplete = true;
                    }
                }

                yield return null;
            }            
        }

        //--------------------------------------------------
        private IEnumerator GameOver()
        {
            if (!player)
            {
                Debug.Log("Player Died");
                UI.ShowGameOver();
            }
            else
            {
                Debug.Log("All waves complete!");
                UI.ShowWinScreen();
                Invoke("LoadMainMenu", 5f);
            }
            
            yield return null;
        }

        //--------------------------------------------------
        private void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        //--------------------------------------------------
        public void RegisterEnemy(EnemyController enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }

        //--------------------------------------------------
        public void RemoveEnemy(EnemyController enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }
    }
}