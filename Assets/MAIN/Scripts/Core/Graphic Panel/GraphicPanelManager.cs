using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRAPHICS
{
    public class GraphicPanelManager : MonoBehaviour
    {
        /*
        The manager that controls all graphic panels
        such as background, cinematic, and foreground
        */

        public static GraphicPanelManager instance { get; private set; }
        public const float DEFAULT_TRANSITION_SPEED = 3f;
        [SerializeField] private GraphicPanel[] allPanels;
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