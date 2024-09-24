using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem dialogueSystem;
        TextArchitect textArchitect;

        string[] lines = new string[5]
        {
            "Hello World!",
            "This is a test.",
            "Testing the Text Architect.",
            "Testing the Dialogue System.",
            "Testing the Text Architect and Dialogue System."
        };
        void Start()
        {
            dialogueSystem = DialogueSystem.instance;
            textArchitect = new TextArchitect(dialogueSystem.dialogueContainer.dialogueText);
            textArchitect.buildMethod = TextArchitect.BuildMethod.typewriter;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textArchitect.Build(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}