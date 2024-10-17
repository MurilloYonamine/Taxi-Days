using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TaxiDays.Data.Save
{
    [Serializable]
    public class DSChoiceSaveData
    {
        [field: SerializeField] private string Text { get; set; }
        [field: SerializeField] private string NodeID { get; set; }
    }
}
