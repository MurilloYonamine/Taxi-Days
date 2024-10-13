using System.Collections;
using System.Collections.Generic;
using TaxiDays.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TaxiDays.Elements
{
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            DialogueType = DSDialogueType.SingleChoice;
            Choices.Add("Próximo Diálogo");
        }
        public override void Blur()
        {
            // Output Container
            foreach (string choice in Choices)
            {
                Port choicesPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicesPort.portName = choice;
                outputContainer.Add(choicesPort);
            }
            RefreshExpandedState();
        }
    }
}
