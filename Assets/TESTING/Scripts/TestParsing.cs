using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace TESTING
{
    public class TestParsing : MonoBehaviour
    {
        void Start()
        {
            SendFileToParse();
        }
        private void SendFileToParse()
        {
            List<string> lines = FileManager.ReadTextAsset("testfile");

            foreach (string line in lines)
            {
                if (line == string.Empty) continue;
                DIALOGUE_LINE dIALOGUE_LINE = DialogueParser.Parse(line);
            }
        }
    }
}