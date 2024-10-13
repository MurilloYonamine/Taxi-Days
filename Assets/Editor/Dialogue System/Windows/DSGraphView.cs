using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TaxiDays.Elements;
using TaxiDays.Enumerations;
using System;
using System.Collections.Generic;

namespace TaxiDays.Windows
{
    public class DSGraphView : GraphView // Essa classe é responsável por criar a view do grafo de diálogo
    {
        public DSGraphView() // Construtor da classe
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if(startPort == port) return;
                if(startPort.node == port.node) return;
                if(startPort.direction == port.direction) return;

                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }
        private void AddManipulators() // Método que adiciona os manipuladores na view do grafo
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Adicionar Diálogo (Escolha Única)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Adicionar Diálogo (Escolhas Múltiplas)", DSDialogueType.MultipleChoice));
        }
        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType) // Método que cria o menu contextual do node
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition))) // Adiciona um node na view do grafo
            );
            return contextualMenuManipulator;
        }
        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position) // Método que cria um node na view do grafo
        {
            Type nodeType = Type.GetType($"TaxiDays.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode)Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }
        private void AddGridBackground() // Método que adiciona o fundo quadriculado na view do grafo
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
        private void AddStyles() // Método que adiciona o estilo da view do grafo
        {
            StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("Dialogue System/DSGraphViewStyles.uss");
            StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("Dialogue System/DSNodeStyles.uss");

            styleSheets.Add(graphViewStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }
    }
}
