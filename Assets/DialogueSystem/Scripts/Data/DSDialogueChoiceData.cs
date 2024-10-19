using UnityEngine;
using System;
using TaxiDays.ScriptableObjects;

namespace TaxiDays.Data
{
    [Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }
    }
}
