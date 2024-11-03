using System.Collections;
using System.Collections.Generic;
using GRAPHICS;
using UnityEngine;
using UnityEngine.Video;

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
        }
        public static List<GraphicData> Capture()
        {
            List<GraphicData> graphicPanels = new List<GraphicData>();

            foreach (var panel in GraphicPanelManager.instance.allPanels)
            {
                if (panel.isClear) continue;

                GraphicData data = new GraphicData();
                data.panelName = panel.panelName;
                data.layers = new List<LayerData>();

                foreach (GraphicLayer layer in panel.layers)
                {
                    LayerData entry = new LayerData(layer);
                    data.layers.Add(entry);
                }
                graphicPanels.Add(data);
            }
            return graphicPanels;
        }
        public static void Apply(List<GraphicData> data)
        {
            List<string> cache = new List<string>();

            foreach (var panelData in data)
            {
                var panel = GraphicPanelManager.instance.GetPanel(panelData.panelName);

                foreach (var layerData in panelData.layers)
                {
                    var layer = panel.GetLayer(layerData.depth, createIfDoesNotExist: true);

                    if (layer.currentGraphic == null || layer.currentGraphic.graphicName != layerData.graphicName)
                    {
                        if (!layerData.isVideo)
                        {
                            Texture tex = HistoryCache.LoadImage(layerData.graphicPath);

                            if (tex != null) layer.SetTexture(tex, filePath: layerData.graphicPath, immediate: true);
                            else Debug.LogWarning($"History State: Não pode carregar a imagem do caminho: {layerData.graphicPath}");
                        }
                        else
                        {
                            VideoClip clip = HistoryCache.LoadVideo(layerData.graphicPath);

                            if (clip != null) layer.SetVideo(clip, filePath: layerData.graphicPath, immediate: true);
                            else Debug.LogWarning($"History State: Não pode carregar o video do caminho: {layerData.graphicPath}");
                        }
                    }
                }
                cache.Add(panel.panelName);
            }
            foreach (var panel in GraphicPanelManager.instance.allPanels)
            {
                if (!cache.Contains(panel.panelName)) panel.Clear(immediate: true);
            }
        }
    }
}
