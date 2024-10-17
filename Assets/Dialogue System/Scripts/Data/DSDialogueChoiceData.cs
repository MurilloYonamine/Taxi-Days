using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;
using TaxiDays.ScriptableObjects;

namespace TaxiDays.Data
{
    [Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] private string Text { get; set; }
        [field: SerializeField] private DSDialogueSO NextDialogue { get; set; }
    }
}
