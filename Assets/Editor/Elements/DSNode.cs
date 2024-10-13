using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TaxiDays.Enumerations;
using UnityEngine.UIElements;

namespace TaxiDays.Elements
{
    public class DSNode : Node // Essa classe é responsável por criar um node no grafo de diálogo
    {
        [SerializeField] protected string DialogueName { get; set; }
        [SerializeField] protected List<string> Choices { get; set; }
        [SerializeField] protected string Text { get; set; }
        [SerializeField] protected DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position) // Método que inicializa o node
        {
            DialogueName = "Nome do Diálogo";
            Choices = new List<string>();
            Text = "Texto do Diálogo.";

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }
        public virtual void Draw() // Método que desenha o node
        {
            // Título do node
            TextField dialogueNameTextField = new TextField() { value = DialogueName };

            dialogueNameTextField.AddToClassList("ds-node__textfield");
            dialogueNameTextField.AddToClassList("ds-node__filename-textfield");
            dialogueNameTextField.AddToClassList("ds-node__textfield__hidden");

            titleContainer.Insert(0, dialogueNameTextField);

            // Container do input
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Conexão do Diálogo";
            inputContainer.Add(inputPort);

            // Extensões do Container
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = new Foldout() { text = "Texto do Diálogo" };
            TextField textTextField = new TextField() { value = Text };

            textTextField.AddToClassList("ds-node__textfield");
            textTextField.AddToClassList("ds-node__quote-textfield");

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

        }
    }
}
