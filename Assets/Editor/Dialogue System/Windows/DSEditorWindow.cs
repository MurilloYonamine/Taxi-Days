using UnityEditor;
using UnityEngine.UIElements;
using TaxiDays.Utilities;
using UnityEditor.UIElements;

namespace TaxiDays.Windows
{
    public class DSEditorWindow : EditorWindow // Essa classe é responsável por criar a janela de edição do diálogo
    {
        private readonly string defaultFileName = "Nome do Arquivo de Diálogo";
        private TextField fileNameTextField;
        private Button saveButton;
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void ShowExample() // Método que cria a janela de edição do diálogo
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable() // Método que é chamado quando a janela é aberta
        {
            AddGraphView();
            AddStyle();
            AddToolbar();
        }
        #region Adição de Elementos
        private void AddGraphView() // Método que adiciona a view do grafo de diálogo na janela
        {
            DSGraphView graphView = new DSGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, "Nome do Arquivo:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });
            saveButton = DSElementUtility.CreateButton("Salvar");

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.AddStyleSheets("Dialogue System/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }
        private void AddStyle() => rootVisualElement.AddStyleSheets("Dialogue System/DSVariables.uss"); // Método que adiciona o estilo da janela
        #endregion
        #region Utility Methods
        public void EnableSaving() => saveButton.SetEnabled(true);
        public void DisableSaving() => saveButton.SetEnabled(false);
        #endregion
    }
}