using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem dialogueSystem;
        TextArchitect textArchitect;

        public TextArchitect.BuildMethod buildMethod = TextArchitect.BuildMethod.instant;

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
            textArchitect.buildMethod = TextArchitect.BuildMethod.fade;
            textArchitect.speed = 0.5f;
        }

        void Update()
        {

            if (buildMethod != textArchitect.buildMethod)
            {
                textArchitect.buildMethod = buildMethod;
                textArchitect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                textArchitect.Stop();
            }

            string longLine = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur laoreet porttitor quam, eget eleifend dolor fermentum at. Proin ipsum felis.";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (textArchitect.isBuilding)
                {
                    if (!textArchitect.hurryUp)
                    {
                        textArchitect.hurryUp = true;
                    }
                    else
                    {
                        textArchitect.ForceComplete();
                    }
                }
                else
                {
                    textArchitect.Build(longLine);
                }
                //textArchitect.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                textArchitect.Append(longLine);
                // textArchitect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}