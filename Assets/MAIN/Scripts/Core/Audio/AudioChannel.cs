using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AUDIO
{
    public class AudioChannel
    {
        /*
        A virtual channel for playing and
        managing audio tracks.
        */
        private const string TRACK_CONTAINER_NAME_FORMAT = "Channel - [{0}]";
        public int channelIndex { get; private set; }

        public Transform trackContainer { get; private set; } = null;
        private List<AudioTrack> tracks = new List<AudioTrack>();
        public AudioChannel(int channel)
        {
            channelIndex = channel;

            trackContainer = new GameObject(String.Format(TRACK_CONTAINER_NAME_FORMAT, channel)).transform;
            trackContainer.SetParent(AudioManager.instance.transform);
        }
        public AudioTrack PlayTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap, string filePath)
        {
            if (TryGetTrack(clip.name, out AudioTrack existingTrack))
            {
                if(!existingTrack.isPlaying) existingTrack.Play();
                return existingTrack;
            }

            AudioTrack track = new AudioTrack(clip, loop, startingVolume, volumeCap, this, AudioManager.instance.musicMixer);
            track.Play();
            return track;
        }
        public bool TryGetTrack(string trackName, out AudioTrack value)
        {
            trackName = trackName.ToLower();
            foreach (AudioTrack track in tracks)
            {
                if (track.name.ToLower() == trackName)
                {
                    value = track;
                    return true;
                }
            }
            value = null;
            return false;
        }
    }
}