using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TaxiDays.Enumerations;
using TaxiDays.Elements;

namespace TaxiDays.Windows
{
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DSGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(DSGraphView dSGraphView)
        {
            graphView = dSGraphView;
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Criar Elemento")),
                new SearchTreeGroupEntry(new GUIContent("Bloco de Diálogo"), 1),
                new SearchTreeEntry(new GUIContent("Escolha Simples", indentationIcon))
                {
                    level = 2,
                    userData = DSDialogueType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Escolha Múltipla", indentationIcon))
                {
                    level = 2,
                    userData = DSDialogueType.SingleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Grupos de Diálogo"), 1),
                new SearchTreeEntry(new GUIContent("Grupo Simples", indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case DSDialogueType.SingleChoice:
                    {
                        DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode)graphView.CreateNode(DSDialogueType.SingleChoice, localMousePosition);
                        graphView.AddElement(singleChoiceNode);
                        return true;
                    }
                case DSDialogueType.MultipleChoice:
                    {
                        DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode)graphView.CreateNode(DSDialogueType.MultipleChoice, localMousePosition);
                        graphView.AddElement(multipleChoiceNode);
                        return true;
                    }
                case DSGroup _group:
                    {
                        graphView.CreateGroup("Grupo de Diálogo", localMousePosition);
                        return true;
                    }
                default: { return false; }
            }
        }
    }
}
