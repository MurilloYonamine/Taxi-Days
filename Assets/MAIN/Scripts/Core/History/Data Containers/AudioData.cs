using System.Collections;
using System.Collections.Generic;
using AUDIO;
using UnityEngine;

namespace History
{
    /// <summary>
    ///  All the data for each audio channel active in the visual novel.
    /// </summary>
    [System.Serializable]
    public class AudioData
    {
        public int channel = 0;
        public string trackName;
        public string trackPath;

        public float trackVolume;
        public float trackPitch;
        public bool trackLoop;

        public AudioData(AudioChannel channel)
        {
            this.channel = channel.channelIndex;

            if (channel.activeTrack == null) return;

            AudioTrack track = channel.activeTrack;
            trackName = track.name;
            trackPath = track.path;
            trackVolume = track.volumeCap;
            trackPitch = track.pitch;
            trackLoop = track.loop;
        }
        public static List<AudioData> Capture()
        {
            List<AudioData> audioChannels = new List<AudioData>();

            foreach (var channel in AudioManager.instance.channels)
            {
                if (channel.Value.activeTrack == null) continue;

                AudioData data = new AudioData(channel.Value);
                audioChannels.Add(data);
            }

            return audioChannels;
        }

    }
}