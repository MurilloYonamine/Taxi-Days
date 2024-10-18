using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TaxiDays.Data.Save
{
    [Serializable]
    public class DSChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}
