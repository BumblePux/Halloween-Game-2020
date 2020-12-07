using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BumblePux.Halloween2020
{
    public class GameUI : MonoBehaviour
    {
        [Header("References")]
        public GameObject GameOverUI;
        public GameObject WinScreen;
        public TMP_Text WaveText;

		//--------------------------------------------------------------------------------
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        //--------------------------------------------------
        public void Retry()
        {
            SceneManager.LoadScene("Game");
        }

        //--------------------------------------------------
        public void ShowGameOver()
        {
            GameOverUI.SetActive(true);
        }

        //--------------------------------------------------
        public IEnumerator SetWaveText(string wave)
        {
            WaveText.enabled = true;
            WaveText.SetText(wave);
            yield return new WaitForSeconds(2f);
            WaveText.enabled = false;
        }

        //--------------------------------------------------
        public void ShowWinScreen()
        {
            WinScreen.SetActive(true);
        }
    }
}