using UnityEngine;
using System;

namespace TaxiDays.ScriptableObjects
{
    [Serializable]
    public class DSDialogueGroupSO : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
