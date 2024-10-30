using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRAPHICS
{
    public class GraphicLayer
    {
        /*
        A single layer on a graphic panel that can be
        assigned an image or video and stacked on
        other layers.
        */
        public const string LAYER_OBJECT_NAME_FORMAT = "Layer: {0}";
        public int layerDepth = 0;
        public Transform panel;

        public GraphicObject currentGraphic { get; private set; } = null;

        public void SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null)
        {
            Texture texture = Resources.Load<Texture>(filePath);

            if (texture == null)
            {
                Debug.LogError($"Não pode ser carregado o graphic texture do caminho: {filePath}. Por favor veja se realmente existe.");
                return;
            }
            SetTexture(texture, transitionSpeed, blendingTexture, filePath);
        }
        public void SetTexture(Texture texture, float transitionSpeed = 1f, Texture blendingTexture = null, string filePath = "")
        {
            CreateGraphic(texture, transitionSpeed, filePath, blendingTexture: blendingTexture);
        }

        private void CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null)
        {
            GraphicObject newGraphic = null;
            if (graphicData is Texture)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as Texture);
            }
            currentGraphic = newGraphic;

            currentGraphic.FadeIn(transitionSpeed, blendingTexture);
        }
    }
}