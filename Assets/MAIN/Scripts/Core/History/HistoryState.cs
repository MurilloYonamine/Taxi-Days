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

        public DialogueData dialogue;
        public List<CharacterData> characters;
        public List<AudioData> audio;
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
            DialogueData.Apply(dialogue);
            CharacterData.Apply(characters);
            AudioData.Apply(audio);
            GraphicData.Apply(graphics);
        }
    }
}