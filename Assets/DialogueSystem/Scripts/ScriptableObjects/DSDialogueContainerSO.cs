using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;
using TaxiDays.Data;

namespace TaxiDays.ScriptableObjects
{
    [Serializable]
    public class DSDialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] private string FileName { get; set; }
        [field: SerializeField] private SerializableDictionary<DSDialogueGroupSO, List<DSDialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] private List<DSDialogueSO> UngroupedDialogues { get; set; }

        private void Initialize(string fileName)
        {
            FileName = fileName;

            DialogueGroups = new SerializableDictionary<DSDialogueGroupSO, List<DSDialogueSO>>();
            UngroupedDialogues = new List<DSDialogueSO>();
        }
    }
}
