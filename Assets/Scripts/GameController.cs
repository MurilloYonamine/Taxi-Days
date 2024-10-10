using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public StoryScene currentScene;
    public DialogueBox dialogueBox;

    void Start()
    {
        dialogueBox = FindObjectOfType<DialogueBox>();
        dialogueBox.PlayScene(currentScene);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (dialogueBox.IsCompleted())
            {
                if (dialogueBox.IsLastSentence())
                {
                    currentScene = currentScene.nextScene;
                    dialogueBox.PlayScene(currentScene);
                }
                else
                {
                    dialogueBox.PlayNextSentence();
                }
            }
        }
    }
}