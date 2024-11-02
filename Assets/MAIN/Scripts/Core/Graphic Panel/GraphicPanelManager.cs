using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRAPHICS
{
    /// <summary>
    /// The manager that controls all graphic panels such as background, cinematic, and foreground.
    /// </summary>  
    public class GraphicPanelManager : MonoBehaviour
    {
        public static GraphicPanelManager instance { get; private set; }
        public const float DEFAULT_TRANSITION_SPEED = 3f;
        [field: SerializeField] public GraphicPanel[] allPanels { get; private set; }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public GraphicPanel GetPanel(string name)
        {
            name = name.ToLower();
            foreach (GraphicPanel panel in allPanels)
            {
                if (panel.panelName.ToLower() == name) return panel;
            }
            return null;
        }
    }
}