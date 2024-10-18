using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;

namespace TaxiDays.Data.Save
{
    [Serializable]
    public class DSNodeSaveData
    {
        [field: SerializeField] private string ID { get; set; }
        [field: SerializeField] private string Name { get; set; }
        [field: SerializeField] private string Text { get; set; }
        [field: SerializeField] private List<DSChoiceSaveData> Choices { get; set; }
        [field: SerializeField] private string GroupID { get; set; }
        [field: SerializeField] private DSDialogueType DialogueType { get; set; }
        [field: SerializeField] private Vector2 Position { get; set; }
    }
}
