using UnityEditor;
using UnityEngine.UIElements;

namespace TaxiDays.Windows
{
    public class DSEditorWindow : EditorWindow // Essa classe é responsável por criar a janela de edição do diálogo
    {
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void ShowExample()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable() => AddGraphView();
        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
    }
}