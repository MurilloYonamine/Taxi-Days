using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    /// <summary>
    /// Represents a snapshot of data at a given time in the visual novel.
    /// </summary>
    [System.Serializable]
    public class HistoryState
    {
        /// <summary>
        /// The dialogue data at the current state.
        /// </summary>
        public DialogueData dialogue;

        /// <summary>
        /// The list of character data at the current state.
        /// </summary>
        public List<CharacterData> characters;

        /// <summary>
        /// The list of audio data at the current state.
        /// </summary>
        public List<AudioData> audio;

        /// <summary>
        /// The list of graphic data at the current state.
        /// </summary>
        public List<GraphicData> graphics;

        /// <summary>
        /// Captures the current state of the visual novel and returns a new instance of <see cref="HistoryState"/>.
        /// </summary>
        public static HistoryState Capture()
        {
            HistoryState state = new HistoryState();

            state.dialogue = DialogueData.Capture();
            state.characters = CharacterData.Capture();
            state.audio = AudioData.Capture();
            state.graphics = GraphicData.Capture();

            return state;
        }

        /// <summary>
        /// Loads the history state.
        /// </summary>
        public void Load()
        {

        }
    }
}