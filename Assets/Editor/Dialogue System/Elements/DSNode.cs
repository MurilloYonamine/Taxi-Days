using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using TaxiDays.Enumerations;
using TaxiDays.Utilities;
using TaxiDays.Windows;

namespace TaxiDays.Elements
{
    public class DSNode : Node // Essa classe é responsável por criar um node no grafo de diálogo
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }
        private DSGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(DSGraphView dsGraphView, Vector2 position) // Método que inicializa o node
        {
            DialogueName = "Nome do Diálogo";
            Choices = new List<string>();
            Text = "Texto do Diálogo.";

            graphView = dsGraphView;

            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }
        #region Overrided Methods
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Desconectar as Portas de Entrada", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Desconectar as Portas de Saída", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }
        #endregion
        public virtual void Draw() // Método que desenha o node
        {
            // Título do node
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, callback =>
            {
                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    DialogueName = callback.newValue;
                    graphView.AddUngroupedNode(this);
                    return;
                }
                DSGroup currentGroup = Group;
                graphView.RemoveGroupedNode(this, Group);
                DialogueName = callback.newValue;
                graphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__filename-textfield",
                "ds-node__text-field__hidden"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            // Container do input
            Port inputPort = this.CreatePort("Conexão de Diálogo", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            // Extensões do Container
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DSElementUtility.CreateFoldout("Texto do Diálogo");
            TextField textTextField = DSElementUtility.CreateTextArea(Text);

            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }
        #region Utility Methods
        public void DisconecctAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }
        private void DisconnectInputPorts() => DisconnectPorts(inputContainer);
        private void DisconnectOutputPorts() => DisconnectPorts(outputContainer);
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if(!port.connected) continue;
                graphView.DeleteElements(port.connections);
            }
        }
        public void SetErrorStyle(Color color) => mainContainer.style.backgroundColor = color;
        public void ResetStyle() => mainContainer.style.backgroundColor = defaultBackgroundColor;
        #endregion
    }
}
