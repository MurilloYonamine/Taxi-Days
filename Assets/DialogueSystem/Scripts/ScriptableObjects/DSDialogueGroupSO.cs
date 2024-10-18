using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;
using TaxiDays.Data;

namespace TaxiDays.ScriptableObjects
{
    [Serializable]
    public class DSDialogueGroupSO : ScriptableObject
    {
        [field: SerializeField] private string GroupName { get; set; }

        private void Initialize()
        {
            GroupName = GroupName;
        }
    }
}
