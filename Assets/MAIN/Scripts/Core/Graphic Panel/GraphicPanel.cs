using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GRAPHICS
{
    [System.Serializable]
    public class GraphicPanel
    {
        /*
        Contains layers that can be assigned
        images and videos on the UI.
        */
        public string panelName;
        public GameObject rootPanel;
        private List<GraphicLayer> layers = new List<GraphicLayer>();

        public GraphicLayer GetLayer(int layerDepth, bool createIfDoesNotExist = false)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].layerDepth == layerDepth) return layers[i];
            }
            if (createIfDoesNotExist) return CreateLayer(layerDepth);

            return null;
        }
        private GraphicLayer CreateLayer(int layerDepth)
        {
            GraphicLayer layer = new GraphicLayer();
            GameObject panel = new GameObject(string.Format(GraphicLayer.LAYER_OBJECT_NAME_FORMAT, layerDepth));
            RectTransform rect = panel.AddComponent<RectTransform>();
            panel.AddComponent<CanvasGroup>();
            panel.transform.SetParent(rootPanel.transform, false);

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            layer.panel = panel.transform;
            layer.layerDepth = layerDepth;

            int index = layers.FindIndex(l => l.layerDepth > layerDepth);
            if (index == -1) layers.Add(layer);
            else layers.Insert(index, layer);

            for (int i = 0; i < layers.Count; i++) layers[i].panel.SetSiblingIndex(layers[i].layerDepth);

            return layer;
        }
        public void Clear(float transitionSpeed = 1f, Texture blendTexture = null, bool immediate = false)
        {
            foreach(GraphicLayer layer in layers) layer.Clear(transitionSpeed, blendTexture, immediate);
        }
    }
}