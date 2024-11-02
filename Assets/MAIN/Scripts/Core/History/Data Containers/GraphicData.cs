using System.Collections;
using System.Collections.Generic;
using GRAPHICS;
using UnityEngine;

namespace History
{
    /// <summary>
    /// All data for the images and videos being displayed on teh graphic panels
    /// </summary>
    [System.Serializable]
    public class GraphicData
    {
        public string panelName;
        public List<LayerData> layers;

        [System.Serializable]
        public class LayerData
        {
            public int depth = 0;
            public string graphicName;
            public string graphicPath;
            public bool isVideo;
            public bool useAudio;

            public LayerData(GraphicLayer layer)
            {
                depth = layer.layerDepth;

                if (layer.currentGraphic == null) return;

                GraphicObject graphic = layer.currentGraphic;

                graphicName = graphic.graphicName;
                graphicPath = graphic.graphicPath;

                isVideo = graphic.isVideo;
                useAudio = graphic.useAudio;
            }
            public static List<GraphicData> Capture()
            {
                List<GraphicData> graphicLayers = new List<GraphicData>();

                foreach (GraphicPanel panel in GraphicPanelManager.instance.allPanels)
                {
                    if (panel.isClear) continue;

                    GraphicData data = new GraphicData();
                    data.panelName = panel.panelName;
                    data.layers = new List<LayerData>();

                    foreach (GraphicLayer layer in panel.layers)
                    {
                        LayerData layerData = new LayerData(layer);
                        data.layers.Add(layerData);
                    }
                    graphicPanels
                }

                return graphicLayers;
            }
        }
    }
}
