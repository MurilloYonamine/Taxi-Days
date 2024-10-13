using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TaxiDays.Enumerations;

namespace TaxiDays.Elements
{
    public class DSNode : Node
    {
        [SerializeField] private string DialogueName { get; set; }
        [SerializeField] private List<string> Choices { get; set; }
        [SerializeField] private string Text { get; set; }
        [SerializeField] private DSDialogueType Type { get; set; }

        private void Initialize()
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue Text.";
        }
    }
}
