using TaxiDays.Enumerations;
using TaxiDays.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TaxiDays.Elements
{
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = DSDialogueType.SingleChoice;
            Choices.Add("Próximo Diálogo");
        }
        public override void Draw()
        {
            base.Draw();

            // Output Container
            foreach (string choice in Choices)
            {
                Port choicesPort = this.CreatePort(choice);
                choicesPort.portName = choice;
                outputContainer.Add(choicesPort);
            }
            RefreshExpandedState();
        }
    }
}
