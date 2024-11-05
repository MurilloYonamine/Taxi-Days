using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicPanelManager : MonoBehaviour
{
    public static GraphicPanelManager instance { get; private set; }

    public const float DEFAULT_TRANSITION_SPEED = 1f;

    [field: SerializeField] public GraphicPanel[] allPanels { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public GraphicPanel GetPanel(string name)
    {
        name = name.ToLower();

        foreach (var panel in allPanels)
        {
            if (panel.panelName.ToLower() == name)
                return panel;
        }

        return null;
    }

    public void HideAllPanels(float speed = 1f, bool immediate = false)
    {
        foreach (var panel in allPanels)
        {
            var canvasGroup = panel.rootPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                CanvasGroupController canvasGroupController = new CanvasGroupController(this, canvasGroup);
                canvasGroupController.Hide(speed, immediate);
            }
        }
    }

    public void ShowAllPanels(float speed = 1f, bool immediate = false)
    {
        foreach (var panel in allPanels)
        {
            var canvasGroup = panel.rootPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                CanvasGroupController canvasGroupController = new CanvasGroupController(this, canvasGroup);
                canvasGroupController.Show(speed, immediate);
            }
        }
    }
    public bool AreAllPanelsHidden()
    {
        foreach (var panel in allPanels)
        {
            var canvasGroup = panel.rootPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null && canvasGroup.alpha > 0)
            {
                return false;
            }
        }
        return true;
    }

}
