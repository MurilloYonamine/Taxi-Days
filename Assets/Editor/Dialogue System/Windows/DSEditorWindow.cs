using UnityEditor;
using UnityEngine.UIElements;
using TaxiDays.Utilities;

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
            DSGraphView graphView = new DSGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        private void AddStyle() => rootVisualElement.AddStyleSheets("Dialogue System/DSVariables.uss"); // Método que adiciona o estilo da janela
        #endregion
    }
}