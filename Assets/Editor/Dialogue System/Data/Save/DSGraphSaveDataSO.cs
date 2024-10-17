using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;

namespace TaxiDays.Data.Save
{
    [Serializable]
    public class DSGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] private string FileName { get; set; }
        [field: SerializeField] private List<DSNodeSaveData> Groups { get; set; }
        [field: SerializeField] private List<DSNodeSaveData> Nodes { get; set; }
        [field: SerializeField] private List<string> OldGroupNames { get; set; }
        [field: SerializeField] private List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] private SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        private void Initialize()
        {
            FileName = FileName;

            Groups = new List<DSNodeSaveData>();
            Nodes = new List<DSNodeSaveData>();
        }
    }
}
