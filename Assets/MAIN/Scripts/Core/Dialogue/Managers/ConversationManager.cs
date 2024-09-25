using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // Event for when the user prompts the next line
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
        // Run the conversation
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
        // Run the dialogue
        private IEnumerator Line_RunDialogue(DIALOGUE_LINE dIALOGUE_LINE)
        {
            if (dIALOGUE_LINE.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(dIALOGUE_LINE.speaker);
            }
            yield return BuildLineSegments(dIALOGUE_LINE.dialogue);

            // Wait for user input
            yield return WaitForUserInput();
        }
        // Run any commands
        private IEnumerator Line_RunCommands(DIALOGUE_LINE dIALOGUE_LINE)
        {
            Debug.Log(dIALOGUE_LINE.commands);
            yield return null;
        }
        // Build the line segments
        private IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];
                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }
        private IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            // Wait for the signal to be triggered
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }
        private IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            // build the dialogue
            if (!append) textArchitect.Build(dialogue);
            else textArchitect.Append(dialogue);

            // wait for the dialogue to complete
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