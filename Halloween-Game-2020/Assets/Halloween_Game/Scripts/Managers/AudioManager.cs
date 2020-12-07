using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace BumblePux.Halloween2020
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Sources")]
        public AudioSource MusicSource;
        public AudioSource SfxSource;
        public AudioMixer Mixer;

        [Header("Volume Settings")]
        [Range(0f, 1f)]
        public float MaxMusicVolume = 1f;
        [Range(0f, 1f)]
        public float MaxSfxVolume = 1f;

        [Header("Fade Settings")]
        public float FadeOutDuration = 0.5f;


        //---------------------------------------------------------------------------
        // Control Methods
        //---------------------------------------------------------------------------
        public void PlayMusic(AudioClip musicClip, bool loop = true)
        {
            if (MusicSource.isPlaying)
                MusicSource.Stop();

            MusicSource.clip = musicClip;
            MusicSource.loop = loop;
            MusicSource.Play();
        }

        //--------------------------------------------------
        public void PlaySfx(AudioClip sfxClip)
        {
            SfxSource.PlayOneShot(sfxClip);
        }

        //--------------------------------------------------
        public void StopMusic(bool fadeOut = true)
        {
            Debug.Log("[AudioManager] StopMusic() - Coroutine needs work. Doesn't fade correctly.");
            if (fadeOut)
                StartCoroutine(FadeOutMusic());

            MusicSource.Stop();
        }

        //---------------------------------------------------------------------------
        // Volume Setter Methods
        //---------------------------------------------------------------------------
        public void SetMasterVolume(float volume)
        {
            float volumeInDb = GetVolumeInDb(volume);
            Mixer.SetFloat("Master", volumeInDb);
        }

        //--------------------------------------------------
        public void SetMusicVolume(float volume)
        {
            MaxMusicVolume = volume;
            float volumeInDb = GetVolumeInDb(volume);
            Mixer.SetFloat("Music", volumeInDb);
        }

        //--------------------------------------------------
        public void SetSfxVolume(float volume)
        {
            MaxSfxVolume = volume;
            float volumeInDb = GetVolumeInDb(volume);
            Mixer.SetFloat("SFX", volumeInDb);
        }

        //---------------------------------------------------------------------------
        // Volume Getter Methods
        //---------------------------------------------------------------------------
        public float GetMasterVolume()
        {
            Mixer.GetFloat("Master", out var volumeInDb);
            return Mathf.Pow(10f, volumeInDb / 20f);
        }

        //---------------------------------------------------------------------------
        // Private Methods
        //---------------------------------------------------------------------------
        private float GetVolumeInDb(float volume)
        {
            if (volume <= 0f)
                volume = 0.0001f;

            return Mathf.Log10(volume) * 20f;
        }

        //--------------------------------------------------
        private IEnumerator FadeOutMusic()
        {
            for (float t = 0f; t < FadeOutDuration; t += Time.unscaledDeltaTime)
            {
                float volume = Mathf.Clamp(1 - (t / FadeOutDuration), 0f, MaxMusicVolume);
                SetMusicVolume(volume);
                yield return null;
            }
        }
    }
}