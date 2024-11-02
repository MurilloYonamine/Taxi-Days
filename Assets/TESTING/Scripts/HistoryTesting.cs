using System.Collections.Generic;
using History;
using UnityEngine;

namespace TESTING
{
    /// <summary>
    /// A simple script to test the history system.
    /// </summary>
    public class HistoryTesting : MonoBehaviour
    {
        public DialogueData data;
        public List<AudioData> audioData;
        public List<GraphicData> graphicData;
        public List<CharacterData> characterData;

        void Update()
        {
            data = DialogueData.Capture();
            audioData = AudioData.Capture();
            graphicData = GraphicData.Capture();
            characterData = CharacterData.Capture();
        }
    }
}