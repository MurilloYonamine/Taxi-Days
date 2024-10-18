using TaxiDays.Enumerations;
using TaxiDays.Utilities;
using TaxiDays.Data.Save;
using TaxiDays.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TaxiDays.Elements
{
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);

            DialogueType = DSDialogueType.SingleChoice;

            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "Próximo Diálogo"
            };

            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            // Output Container
            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicesPort = this.CreatePort(choice.Text);

                choicesPort.userData = choice;

                outputContainer.Add(choicesPort);
            }
            RefreshExpandedState();
        }
    }
}
