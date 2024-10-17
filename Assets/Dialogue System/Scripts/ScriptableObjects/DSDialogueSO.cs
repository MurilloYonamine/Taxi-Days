using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;
using TaxiDays.Data;

namespace TaxiDays.ScriptableObjects
{
    [Serializable]
    public class DSDialogueSO : ScriptableObject
    {
        [field: SerializeField] private string DialogueName { get; set; }
        [field: SerializeField][field: TextArea()] private string Text { get; set; }
        [field: SerializeField] private List<DSDialogueChoiceData> Choices { get; set; }
        [field: SerializeField] private DSDialogueType DialogueType { get; set; }
        [field: SerializeField] private bool IsStartingDialogue { get; set; }

        private void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            DialogueType = dialogueType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
