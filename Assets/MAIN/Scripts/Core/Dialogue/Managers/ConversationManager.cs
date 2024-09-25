using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;
        private TextArchitect textArchitect = null;
        private bool userPrompt = false;
        public ConversationManager(TextArchitect textArchitect)
        {
            this.textArchitect = textArchitect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }
        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }
        public void StopConversation()
        {
            if (!isRunning) return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                // Don't show any blank lines or try to run any logic of them
                if (string.IsNullOrWhiteSpace(conversation[i])) continue;
                DIALOGUE_LINE dIALOGUE_LINE = DialogueParser.Parse(conversation[i]);

                // Show dialogue
                if (dIALOGUE_LINE.hasDialogue) yield return Line_RunDialogue(dIALOGUE_LINE);

                // Run any commands
                if (dIALOGUE_LINE.hasCommands) yield return Line_RunCommands(dIALOGUE_LINE);
            }
        }

        private IEnumerator Line_RunDialogue(DIALOGUE_LINE dIALOGUE_LINE)
        {
            if (dIALOGUE_LINE.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(dIALOGUE_LINE.speaker);
            }
            else
            {
                dialogueSystem.HideSpeakerName();
            }
            yield return BuildDialogue(dIALOGUE_LINE.dialogue);

            // Wait for user input
            yield return WaitForUserInput();
        }

        private IEnumerator Line_RunCommands(DIALOGUE_LINE dIALOGUE_LINE)
        {
            Debug.Log(dIALOGUE_LINE.commands);
            yield return null;
        }
        private IEnumerator BuildDialogue(string dialogue)
        {
            textArchitect.Build(dialogue);

            while (textArchitect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!textArchitect.hurryUp)
                    {
                        textArchitect.hurryUp = true;
                    }
                    else
                    {
                        textArchitect.ForceComplete();
                    }
                    userPrompt = false;
                }
                yield return null;
            }
        }
        private IEnumerator WaitForUserInput()
        {
            while (!userPrompt) yield return null;
            userPrompt = false;
        }
    }
}