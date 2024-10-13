using UnityEditor;
using UnityEngine.UIElements;

namespace TaxiDays.Windows
{
    public class DSEditorWindow : EditorWindow // Essa classe é responsável por criar a janela de edição do diálogo
    {
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void ShowExample() // Método que cria a janela de edição do diálogo
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable() // Método que é chamado quando a janela é aberta
        {
            AddGraphView();
            AddStyle();
        }
        #region Adição de Elementos
        private void AddGraphView() // Método que adiciona a view do grafo de diálogo na janela
        {
            DSGraphView graphView = new DSGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        private void AddStyle() // Método que adiciona o estilo da janela
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Dialogue System/DSVariables.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }
        #endregion
    }
}