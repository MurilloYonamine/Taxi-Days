using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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

        public GraphicObject currentGraphic = null;
        private List<GraphicObject> oldGraphics = new List<GraphicObject>();

        public void SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null)
        {
            Texture texture = Resources.Load<Texture2D>(filePath);

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
        public void SetVideo(string filePath, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null)
        {
            VideoClip clip = Resources.Load<VideoClip>(filePath);

            if (clip == null)
            {
                Debug.LogError($"Não pode ser carregado o video clip do caminho: {filePath}. Por favor veja se realmente existe.");
                return;
            }
            SetVideo(clip, transitionSpeed, useAudio, blendingTexture, filePath);
        }
        public void SetVideo(VideoClip video, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, string filePath = "")
        {
            CreateGraphic(video, transitionSpeed, filePath, useAudio, blendingTexture: blendingTexture);
        }
        private void CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null)
        {
            GraphicObject newGraphic = null;
            if (graphicData is Texture)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as Texture);
            }
            else if (graphicData is VideoClip)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo);
            }
            if (currentGraphic != null && oldGraphics.Contains(currentGraphic)) oldGraphics.Add(currentGraphic);

            currentGraphic = newGraphic;

            currentGraphic.FadeIn(transitionSpeed, blendingTexture);
        }
        public void DestroyOldGraphics()
        {

            foreach (GraphicObject graphic in oldGraphics)
            {
                Object.Destroy(graphic.renderer.gameObject);
            }
            oldGraphics.Clear();
        }
        public void Clear()
        {
            if(currentGraphic != null) currentGraphic.FadeOut();

            foreach(GraphicObject graphic in oldGraphics) graphic.FadeOut();
        }
    }
}