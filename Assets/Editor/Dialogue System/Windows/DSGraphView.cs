using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TaxiDays.Elements;
using UnityEngine;

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
        private DSNode CreateNode(Vector2 position) // Método que cria um node na view do grafo
        {
            DSNode node = new DSNode();

            node.Initialize(position);
            node.Draw();

            return node;
        }
        private void AddManipulators() // Método que adiciona os manipuladores na view do grafo
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu());
        }
        private IManipulator CreateNodeContextualMenu() // Método que cria o menu contextual do node
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Adicionar Node", actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition))) // Adiciona um node na view do grafo
            );
            return contextualMenuManipulator;
        }
        private void AddGridBackground() // Método que adiciona o fundo quadriculado na view do grafo
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
        private void AddStyles() // Método que adiciona o estilo da view do grafo
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Dialogue System/DSGraphViewStyles.uss");
            styleSheets.Add(styleSheet);
        }
    }
}
