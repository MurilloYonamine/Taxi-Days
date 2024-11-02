using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace History
{
    /// <summary>
    /// Represents a snapshot of data at a given time in the visual novel.
    /// </summary>
    [System.Serializable]
    public class DialogueData
    {
        /*
        Contains all data for the dialogue
        at this point in time.
        */
        public string currentDialogue = "";
        public string currentSpeaker = "";

        public string dialogueFont;
        public Color dialogueColor;
        public float dialogueScale;

        public string speakerFont;
        public Color speakerNameColor;
        public float speakerScale;

        /// <summary>
        /// Captures the current state of the dialogue system and returns a new instance of <see cref="DialogueData"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="DialogueData"/> containing the captured dialogue data.</returns>
        public static DialogueData Capture()
        {
            DialogueData data = new DialogueData();

            DialogueSystem dialogueSystem = DialogueSystem.instance;
            TextMeshProUGUI dialogueText = dialogueSystem.dialogueContainer.dialogueText;
            TextMeshProUGUI nameText = dialogueSystem.dialogueContainer.nameContainer.nameText;

            data.currentDialogue = dialogueText.text;
            data.dialogueFont = FilePaths.resources_font + dialogueText.font.name;
            data.dialogueColor = dialogueText.color;
            data.dialogueScale = dialogueText.fontSize;

            data.currentSpeaker = nameText.text;
            data.speakerFont = FilePaths.resources_font + nameText.font.name;
            data.speakerNameColor = nameText.color;
            data.speakerScale = nameText.fontSize;

            return data;
        }
    }
}