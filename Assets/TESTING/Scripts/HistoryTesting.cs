using System.Collections.Generic;
using History;
using UnityEngine;

namespace TESTING
{
    public class HistoryTesting : MonoBehaviour
    {
        public DialogueData data;
        public List<AudioData> audioData;

        void Update()
        {
            data = DialogueData.Capture();
            audioData = AudioData.Capture();
        }
    }
}