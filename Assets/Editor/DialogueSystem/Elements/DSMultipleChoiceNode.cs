using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TaxiDays.Enumerations;
using TaxiDays.Utilities;
using TaxiDays.Windows;
using TaxiDays.Data.Save;

namespace TaxiDays.Elements
{
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);

            DialogueType = DSDialogueType.MultipleChoice;

            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "Nova Escolha"
            };

            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            // Main Container
            Button addChoiceButton = DSElementUtility.CreateButton("Adicionar Escolha", () =>
            {
                DSChoiceSaveData choiceData = new DSChoiceSaveData()
                {
                    Text = "Nova Escolha"
                };

                Choices.Add(choiceData);

                Port choicesPort = CreateChoicePort(choiceData);

                outputContainer.Add(choicesPort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output Container
            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicesPort = CreateChoicePort(choice);

                outputContainer.Add(choicesPort);
            }
            RefreshExpandedState();
        }
        #region Elements Creation
        private Port CreateChoicePort(object userData)
        {
            Port choicesPort = this.CreatePort();

            choicesPort.userData = userData;

            DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

            Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;
                if (choicesPort.connected) graphView.DeleteElements(choicesPort.connections);

                Choices.Remove(choiceData);

                graphView.RemoveElement(choicesPort);
            });

            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

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
