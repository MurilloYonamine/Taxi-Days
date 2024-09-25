using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class TestDialogueFiles : MonoBehaviour
{
    void Start()
    {
        StartConversation();
    }
    private void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("testfile");
        
        DialogueSystem.instance.Say(lines);
    }
}
