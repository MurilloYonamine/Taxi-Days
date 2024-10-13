using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TaxiDays.Enumerations;
using TaxiDays.Utilities;

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
            Button addChoiceButton = DSElementUtility.CreateButton("Adicionar Escolha", () =>
            {
                Port choicesPort = CreateChoicePort("Nova Escolha");
                Choices.Add("Nova Escolha");
                outputContainer.Add(choicesPort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output Container
            foreach (string choice in Choices)
            {
                Port choicesPort = CreateChoicePort(choice);

                outputContainer.Add(choicesPort);
            }
            RefreshExpandedState();
        }
        #region Elements Creation
        private Port CreateChoicePort(string choice)
        {
            Port choicesPort = this.CreatePort();

            Button deleteChoiceButton = DSElementUtility.CreateButton("X");

            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choice);

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choiceTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__choice-textfield",
                "ds-node__textfield__hidden"
            );

            choicesPort.contentContainer.Add(choiceTextField);
            choicesPort.contentContainer.Add(deleteChoiceButton);

            outputContainer.Add(choicesPort);
            return choicesPort;
        }
        #endregion
    }
}
