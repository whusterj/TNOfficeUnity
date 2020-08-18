using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TNOffice.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        public Sound[] sounds;
        public GameObject[] speakers = null;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                if (s.name == "Boost")
                {
                    Debug.Log("Set up Sound Source for Boost: " + s.source.ToString());
                }
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                if (s.playOnAwake)
                {
                    Play2D(s.name);
                }
            }

            // CollectSpeakers();
        }

        public void Start()
        {
            SceneManager.activeSceneChanged += SceneManager_OnActiveSceneChanged;
        }

        private void SceneManager_OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            Debug.Log("AudioManager.OnActiveSceneChanged");
            CollectSpeakers();
        }

        private void CollectSpeakers()
        {
            speakers = GameObject.FindGameObjectsWithTag("Speakers");
        }

        public Sound GetSoundByName(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                throw new Exception("Sound: " + name + " was not found.");
            }
            return s;
        }

        public void Play(string name, float volume = 1.0f)
        {
            Sound s = GetSoundByName(name);
            s.source.PlayOneShot(s.clip, volume);
        }

        public void Play2D(string name, float volume = 1.0f)
        {
            Sound s = GetSoundByName(name);
            s.source.spatialBlend = 0f;
            s.source.Play();
        }

        public void Stop(string name)
        {
            Sound s = GetSoundByName(name);
            StartCoroutine("FadeOutList", new Sound[] { s });
        }

        public void StopAll()
        {
            // Fade out all sources that are playing
            StartCoroutine("FadeOutList", sounds);
        }

        IEnumerator FadeOutList(Sound[] sounds)
        {
            bool atLeastOnePlaying = true;
            while (atLeastOnePlaying)
            {
                atLeastOnePlaying = false;
                foreach (Sound sound in sounds)
                {
                    if (sound.source.isPlaying && sound.source.volume > 0)
                    {
                        atLeastOnePlaying = true;
                        sound.source.volume -= 0.001f;
                        Debug.Log("AudioManager: Fading Out Sound: " + sound.name + sound.source.volume);
                    }
                    if (sound.source.isPlaying && sound.source.volume == 0)
                    {
                        sound.source.Stop();
                        sound.source.volume = sound.volume;
                    }
                }

                if (atLeastOnePlaying)
                {
                    // Wait until next frame
                    yield return null;
                }
            }
        }

        public void PlayOnSonos(string name)
        {
            Debug.Log("AudioManager.PlayOnSonos");
            StopAll();
            Sound s = GetSoundByName(name);
            foreach (GameObject speaker in speakers)
            {
                Debug.Log("AudioManager: Playing on Speaker: " + speaker.ToString());
                AudioSource source = speaker.GetComponent<AudioSource>();
                source.clip = s.clip;
                source.volume = s.volume;
                source.spatialBlend = 1f;
                source.Play();
            }
        }
    }
}