using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaxiDays.Enumerations;
using System;

namespace TaxiDays.Data.Save
{
    [Serializable]
    public class DSGroupSaveData
    {
        [field: SerializeField] private string ID { get; set; }
        [field: SerializeField] private string Name { get; set; }
        [field: SerializeField] private Vector2 position { get; set; }
    }
}
