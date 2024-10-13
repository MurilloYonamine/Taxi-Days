using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using TaxiDays.Enumerations;
using TaxiDays.Utilities;

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
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);

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
    }
}
