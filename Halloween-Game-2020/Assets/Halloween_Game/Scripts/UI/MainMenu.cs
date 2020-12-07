using UnityEngine;
using UnityEngine.SceneManagement;

namespace BumblePux.Halloween2020
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Audio")]
        public AudioClip MainMenuClip;

        private new AudioManager audio;

        //--------------------------------------------------------------------------------
        private void Start()
        {
            audio = AudioManager.Instance;

            if (MainMenuClip)
                audio.PlayMusic(MainMenuClip);
        }

        //--------------------------------------------------
        public void PlayGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}