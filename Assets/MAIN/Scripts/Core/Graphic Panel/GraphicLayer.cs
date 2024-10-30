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

        public Coroutine SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null, bool immediate = false)
        {
            Texture texture = Resources.Load<Texture2D>(filePath);

            if (texture == null)
            {
                Debug.LogError($"Não pode ser carregado o graphic texture do caminho: {filePath}. Por favor veja se realmente existe.");
                return null;
            }
            return SetTexture(texture, transitionSpeed, blendingTexture, filePath, immediate);
        }
        public Coroutine SetTexture(Texture texture, float transitionSpeed = 1f, Texture blendingTexture = null, string filePath = "", bool immediate = false)
        {
            return CreateGraphic(texture, transitionSpeed, filePath, blendingTexture: blendingTexture, immediate: immediate);
        }
        public Coroutine SetVideo(string filePath, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, bool immediate = false)
        {
            VideoClip clip = Resources.Load<VideoClip>(filePath);

            if (clip == null)
            {
                Debug.LogError($"Não pode ser carregado o video clip do caminho: {filePath}. Por favor veja se realmente existe.");
                return null;
            }
            return SetVideo(clip, transitionSpeed, useAudio, blendingTexture, filePath, immediate);
        }
        public Coroutine SetVideo(VideoClip video, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, string filePath = "", bool immediate = false)
        {
            return CreateGraphic(video, transitionSpeed, filePath, useAudio, blendingTexture: blendingTexture, immediate: immediate);
        }
        private Coroutine CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null, bool immediate = false)
        {
            GraphicObject newGraphic = null;
            if (graphicData is Texture)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as Texture, immediate);
            }
            else if (graphicData is VideoClip)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo, immediate);
            }
            if (currentGraphic != null && oldGraphics.Contains(currentGraphic)) oldGraphics.Add(currentGraphic);

            currentGraphic = newGraphic;

            if(!immediate) return currentGraphic.FadeIn(transitionSpeed, blendingTexture);

            DestroyOldGraphics();
            return null;
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