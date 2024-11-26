using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace History
{
    [System.Serializable]
    public class GraphicData
    {
        public string panelName;
        public List<LayerData> layers;
        public List<int> backgroundStates; // 0 para invisível, 1 para visível.

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

                if (layer.currentGraphic == null)
                    return;

                var graphic = layer.currentGraphic;

                graphicName = graphic.graphicName;
                graphicPath = graphic.graphicPath;
                isVideo = graphic.isVideo;
                useAudio = graphic.useAudio;
            }
        }

        public static List<GraphicData> Capture()
        {
            List<GraphicData> graphicPanels = new List<GraphicData>();

            // Captura os dados dos painéis gráficos
            foreach (var panel in GraphicPanelManager.instance.allPanels)
            {
                if (panel.isClear)
                    continue;

                GraphicData data = new GraphicData();
                data.panelName = panel.panelName;
                data.layers = new List<LayerData>();

                foreach (var layer in panel.layers)
                {
                    LayerData entry = new LayerData(layer);
                    data.layers.Add(entry);
                }

                graphicPanels.Add(data);
            }

            // Captura os estados dos backgrounds do BackgroundManager
            var bgManager = Object.FindObjectOfType<BackgroundManager>();
            if (bgManager != null)
            {
                GraphicData backgroundData = new GraphicData
                {
                    panelName = "BackgroundManager",
                    backgroundStates = new List<int>()
                };

                foreach (var canvasGroup in bgManager.GetCanvasGroups())
                {
                    backgroundData.backgroundStates.Add(canvasGroup.alpha > 0.5f ? 1 : 0);
                }

                graphicPanels.Add(backgroundData);
            }

            return graphicPanels;
        }

        public static void Apply(List<GraphicData> data)
        {
            List<string> cache = new List<string>();

            foreach (var panelData in data)
            {
                // Verifica se é um painel gráfico ou o BackgroundManager
                if (panelData.panelName == "BackgroundManager")
                {
                    var bgManager = Object.FindObjectOfType<BackgroundManager>();
                    if (bgManager != null && panelData.backgroundStates != null)
                    {
                        var canvasGroups = bgManager.GetCanvasGroups();
                        for (int i = 0; i < canvasGroups.Count; i++)
                        {
                            if (i < panelData.backgroundStates.Count)
                            {
                                float targetAlpha = panelData.backgroundStates[i] == 1 ? 1f : 0f;
                                bgManager.SetAlpha(canvasGroups[i], targetAlpha);
                            }
                        }
                    }
                }
                else
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
                                if (tex != null)
                                    layer.SetTexture(tex, filePath: layerData.graphicPath, immediate: true);
                                else
                                    Debug.LogWarning($"History State: Could not load image from path '{layerData.graphicPath}'");
                            }
                            else
                            {
                                VideoClip clip = HistoryCache.LoadVideo(layerData.graphicPath);
                                if (clip != null)
                                    layer.SetVideo(clip, filePath: layerData.graphicPath, immediate: true);
                                else
                                    Debug.LogWarning($"History State: Could not load video from path '{layerData.graphicPath}'");
                            }
                        }
                    }

                    cache.Add(panel.panelName);
                }
            }

            foreach (var panel in GraphicPanelManager.instance.allPanels)
            {
                if (!cache.Contains(panel.panelName))
                    panel.Clear(immediate: true);
            }
        }
    }
}
