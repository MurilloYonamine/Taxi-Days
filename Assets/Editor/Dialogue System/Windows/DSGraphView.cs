using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using TaxiDays.Elements;
using TaxiDays.Enumerations;
using TaxiDays.Utilities;

namespace TaxiDays.Windows
{
    public class DSGraphView : GraphView // Essa classe é responsável por criar a view do grafo de diálogo
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;
        public DSGraphView(DSEditorWindow dSEditorWindow) // Construtor da classe
        {
            editorWindow = dSEditorWindow;

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            AddStyles();
        }
        #region Override Methods
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;

                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }
        #endregion
        #region Manipulators
        private void AddManipulators() // Método que adiciona os manipuladores na view do grafo
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Adicionar Diálogo (Escolha Única)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Adicionar Diálogo (Escolhas Múltiplas)", DSDialogueType.MultipleChoice));

            this.AddManipulator(CreateGroupContextualMenu());
        }
        private IManipulator CreateGroupContextualMenu() // Método que cria o menu contextual do grupo
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
    menuEvent => menuEvent.menu.AppendAction("Adicionar Grupo", actionEvent => AddElement(
        CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
        )
);
            return contextualMenuManipulator;
        }
        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType) // Método que cria o menu contextual do node
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                // Adiciona um node na view do grafo
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );
            return contextualMenuManipulator;
        }
        #endregion
        #region Elements Creation
        public Group CreateGroup(string title, Vector2 localMousePosition) // Método que cria um grupo na view do grafo
        {
            Group group = new Group
            {
                title = title,
            };
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }
        public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position) // Método que cria um node na view do grafo
        {
            Type nodeType = Type.GetType($"TaxiDays.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode)Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }
        #endregion
        #region Elements Addition
        private void AddSearchWindow() // Método que adiciona a janela de busca na view do grafo
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                searchWindow.Initialize(this);
            }
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }
        private void AddGridBackground() // Método que adiciona o fundo quadriculado na view do grafo
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
        private void AddStyles() // Método que adiciona o estilo da view do grafo
        {
            this.AddStyleSheets(
                "Dialogue System/DSGraphViewStyles.uss",
                "Dialogue System/DSNodeStyles.uss"
                );
        }
        #endregion
        #region Utilities
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }
        #endregion
    }
}
