using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV;
using UDEV.ActionEventDispatcher;

namespace UDEV.ChickenMerge
{
    public class AudioBase : Singleton<AudioBase>, IActionEventDispatcher
    {
        [Header("Main Settings:")]
        [Range(0, 1)]
        public float musicVolume = 0.3f;
        /// the sound fx volume
        [Range(0, 1)]
        public float sfxVolume = 1f;
        [SerializeField]
        private AudioSource m_musicAus;
        [SerializeField]
        private AudioSource m_sfxAus;

        public AudioSO data;

        /// <summary>
        /// Play Sound Effect
        /// </summary>
        /// <param name="clips">Array of sounds</param>
        /// <param name="aus">Audio Source</param>
        public virtual void PlaySound(AudioClip[] clips, AudioSource aus = null)
        {
            if (aus == null)
            {
                aus = m_sfxAus;
            }

            if (clips != null && clips.Length > 0 && aus)
            {
                var clip = Helper.GetRandom(clips);
                aus.PlayOneShot(clip, sfxVolume);
            }
        }

        /// <summary>
        /// Play Sound Effect
        /// </summary>
        /// <param name="clip">Sounds</param>
        /// <param name="aus">Audio Source</param>
        public virtual void PlaySound(AudioClip clip, AudioSource aus = null)
        {
            if (aus == null)
            {
                aus = m_sfxAus;
            }

            if (clip != null && aus)
            {
                aus.PlayOneShot(clip, sfxVolume);
            }
        }

        /// <summary>
        /// Play Music
        /// </summary>
        /// <param name="musics">Array of musics</param>
        /// <param name="loop">Can Loop</param>
        public virtual void PlayMusic(AudioClip[] musics, bool loop = true)
        {
            if (m_musicAus && musics != null && musics.Length > 0)
            {
                var music = Helper.GetRandom(musics);

                m_musicAus.clip = music;
                m_musicAus.loop = loop;
                m_musicAus.volume = musicVolume;
                m_musicAus.Play();
            }
        }

        /// <summary>
        /// Play Music
        /// </summary>
        /// <param name="music">music</param>
        /// <param name="canLoop">Can Loop</param>
        public virtual void PlayMusic(AudioClip music, bool canLoop)
        {
            if (m_musicAus && music != null)
            {
                m_musicAus.clip = music;
                m_musicAus.loop = canLoop;
                m_musicAus.volume = musicVolume;
                m_musicAus.Play();
            }
        }

        /// <summary>
        /// Set volume for audiosource
        /// </summary>
        /// <param name="vol">New Volume</param>
        public void SetMusicVolume(float vol)
        {
            if (m_musicAus) m_musicAus.volume = vol;
            musicVolume = vol;
        }

        public void SetSoundVolume(float vol)
        {
            if (m_sfxAus) m_sfxAus.volume = vol;
            sfxVolume = vol;
        }

        /// <summary>
        /// Stop play music or sound effect
        /// </summary>
        public void StopPlayMusic()
        {
            m_musicAus?.Stop();
        }

        public void StopPlaySound()
        {
            m_sfxAus?.Stop();
        }

        public virtual void RegisterEvents()
        {

        }

        public virtual void UnregisterEvents()
        {

        }
    }
}
