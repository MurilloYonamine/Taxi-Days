using UnityEngine;
using TaxiDays.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace TaxiDays.Elements
{
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = DSDialogueType.MultipleChoice;
            Choices.Add("Nova Escolha");
        }
        public override void Draw()
        {
            base.Draw();

            // Main Container
            Button addChoiceButton = new Button() { text = "Adicionar Escolha" };
            mainContainer.Insert(1, addChoiceButton);

            // Output Container
            foreach (string choice in Choices)
            {
                Port choicesPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicesPort.portName = "";

                Button deleteChoiceButton = new Button() { text = "X" };
                TextField choiceTextField = new TextField() { value = choice };
                choiceTextField.style.flexDirection = FlexDirection.Column;

                choicesPort.contentContainer.Add(choiceTextField);
                choicesPort.contentContainer.Add(deleteChoiceButton);

                outputContainer.Add(choicesPort);
            }
            RefreshExpandedState();
        }
    }
}
