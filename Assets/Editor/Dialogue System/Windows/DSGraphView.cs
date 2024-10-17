using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using TaxiDays.Elements;
using TaxiDays.Enumerations;
using TaxiDays.Utilities;
using TaxiDays.Data.Error;
using TaxiDays.Data.Save;

namespace TaxiDays.Windows
{
    public class DSGraphView : GraphView // Essa classe é responsável por criar a view do grafo de diálogo
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;

        private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, DSGroupErrorData> groups;
        private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

        private int repeatedNameAmount;
        public int RepeatedNameAmount
        {
            get { return repeatedNameAmount; }
            set
            {
                repeatedNameAmount = value;
                if (repeatedNameAmount == 0) editorWindow.EnableSaving();
                if (repeatedNameAmount == 1) editorWindow.DisableSaving();
            }
        }

        public DSGraphView(DSEditorWindow dSEditorWindow) // Construtor da classe
        {
            editorWindow = dSEditorWindow;

            ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
            groups = new SerializableDictionary<string, DSGroupErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();

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
            menuEvent => menuEvent.menu.AppendAction("Adicionar Grupo", actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
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
        public DSGroup CreateGroup(string title, Vector2 localMousePosition) // Método que cria um grupo na view do grafo
        {
            DSGroup group = new DSGroup(title, localMousePosition);

            AddGroup(group);

            AddElement(group);

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is DSNode)) continue;
                DSNode node = (DSNode)selectedElement;
                group.AddElement(node);
            }

            //group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }
        public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position) // Método que cria um node na view do grafo
        {
            Type nodeType = Type.GetType($"TaxiDays.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode)Activator.CreateInstance(nodeType);

            node.Initialize(this, position);
            node.Draw();

            AddUngroupedNode(node);

            return node;
        }
        #endregion
        #region Callbacks
        public void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type groupType = typeof(DSGroup);
                Type edgeType = typeof(Edge);

                List<DSGroup> groupsToDelete = new List<DSGroup>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<DSNode> nodesToDelete = new List<DSNode>();

                foreach (GraphElement element in selection)
                {
                    if (element is DSNode)
                    {
                        nodesToDelete.Add((DSNode)element);
                        continue;
                    }
                    if (element.GetType() == edgeType)
                    {
                        Edge edge = (Edge)element;

                        edgesToDelete.Add(edge);

                        continue;
                    }
                    if (element.GetType() != groupType) continue;

                    DSGroup group = (DSGroup)element;

                    groupsToDelete.Add(group);
                }

                foreach (DSGroup group in groupsToDelete)
                {
                    List<DSNode> groupNodes = new List<DSNode>();
                    foreach (GraphElement groupElements in group.containedElements)
                    {
                        if (!(groupElements is DSNode)) continue;
                        DSNode node = (DSNode)groupElements;
                        groupNodes.Add(node);
                    }
                    group.RemoveElements(groupNodes);
                    RemoveGroup(group);
                    RemoveElement(group);
                }
                DeleteElements(edgesToDelete);

                foreach (DSNode node in nodesToDelete)
                {
                    if (node.Group != null) node.Group.RemoveElement(node);
                    RemoveUngroupedNode(node);
                    node.DisconnectAllPorts();
                    RemoveElement(node);
                }
            };
        }
        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode)) continue;

                    DSGroup nodeGroup = (DSGroup)group;
                    DSNode node = (DSNode)element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
                }
            };
        }
        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode)) continue;

                    DSNode node = (DSNode)element;

                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }
        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DSGroup dSGroup = (DSGroup)group;
                dSGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                RemoveGroup(dSGroup);
                dSGroup.oldTitle = dSGroup.title;
                AddGroup(dSGroup);
            };
        }
        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DSNode nextNode = (DSNode)edge.input.node;

                        DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }
                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType) continue;

                        Edge edge = (Edge)element;

                        DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;

                        choiceData.NodeID = "";
                    }
                }
                return changes;
            };
        }
        #endregion
        #region Repeated Elements
        public void AddUngroupedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                DSNodeErrorData nodeErrorData = new DSNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }

            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++RepeatedNameAmount;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveUngroupedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                --RepeatedNameAmount;
                ungroupedNodesList[0].ResetStyle();
                return;
            }

            if (ungroupedNodesList.Count == 0) ungroupedNodes.Remove(nodeName);
        }
        public void AddGroup(DSGroup group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                DSGroupErrorData groupErrorData = new DSGroupErrorData();
                groupErrorData.Groups.Add(group);
                groups.Add(groupName, groupErrorData);
                return;
            }
            List<DSGroup> groupsList = groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = groups[groupName].ErrorData.Color;

            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++RepeatedNameAmount;
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveGroup(DSGroup group)
        {
            string oldGroupName = group.oldTitle.ToLower();

            List<DSGroup> groupsList = groups[oldGroupName].Groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --RepeatedNameAmount;
                groupsList[0].ResetStyle();
                return;
            }

            if (groupsList.Count == 0) groups.Remove(oldGroupName);
        }
        public void AddGroupedNode(DSNode node, DSGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = group;

            if (!groupedNodes.ContainsKey(group)) groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                DSNodeErrorData nodeErrorData = new DSNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                groupedNodes[group].Add(nodeName, nodeErrorData);
                return;
            }
            List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++RepeatedNameAmount;
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveGroupedNode(DSNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;

            List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --RepeatedNameAmount;
                groupedNodesList[0].ResetStyle();
                return;
            }

            if (groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);
                if (groupedNodes[group].Count == 0) groupedNodes.Remove(group);
            }
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