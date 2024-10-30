using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AUDIO
{
    public class AudioManager : MonoBehaviour
    {
        /*
        Handles sound effects, voices,
        ambience, and music.
        */
        public static AudioManager instance { get; private set; }
        public AudioMixerGroup musicMixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup voicesMixer;
        private void Awake()
        {
            if (instance == null)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
        }
    }
}