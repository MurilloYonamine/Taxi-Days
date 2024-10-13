using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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
        private void AddManipulators() // Método que adiciona os manipuladores na view do grafo
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
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
