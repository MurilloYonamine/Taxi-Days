using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GRAPHICS
{
    public class GraphicObject
    {
        /*
         The video containing the image/video
         on a single graphic layer
        */
        private const string NAME_FORMAT = "Graphic - [{0}]";
        private const string MATERIAL_PATH = "Materials/layerTransitionMaterial";
        private const string MATERIAL_FIELD_COLOR = "_Color";
        private const string MATERIAL_FIELD_MAINTEXT = "_MainTex";
        private const string MATERIAL_FIELD_BLENDTEXT = "_BlendText";
        private const string MATERIAL_FIELD_BLEND = "_Blend";
        private const string MATERIAL_FIELD_ALPHA = "_Alpha";
        public RawImage renderer;

        public bool isVideo { get { return video != null; } }
        public VideoPlayer video = null;
        public AudioSource audio = null;

        public string graphicPath = "";
        public string graphicName { get; private set; }

        private Coroutine co_fadingIn = null;
        private Coroutine co_fadingOut = null;

        public GraphicObject(GraphicLayer layer, string graphicPath, Texture texture)
        {
            this.graphicPath = graphicPath;

            GameObject ob = new GameObject();
            ob.transform.SetParent(layer.panel);
            renderer = ob.AddComponent<RawImage>();

            graphicName = texture.name;

            InitGraphic();

            renderer.name = string.Format(NAME_FORMAT, graphicName);

            renderer.texture = texture;
            renderer.material.SetTexture(MATERIAL_FIELD_MAINTEXT, texture);
        }
        private void InitGraphic()
        {
            renderer.transform.localPosition = Vector3.zero;
            renderer.transform.localScale = Vector3.one;

            RectTransform rect = renderer.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.one;

            renderer.material = GetTransitionMaterial();

            renderer.material.SetFloat(MATERIAL_FIELD_BLEND, 0);
            renderer.material.SetFloat(MATERIAL_FIELD_ALPHA, 0);

        }
        private Material GetTransitionMaterial()
        {
            Material material = Resources.Load<Material>(MATERIAL_PATH);

            if (material != null) return new Material(material);

            return null;
        }
        #region Fade Methods
        GraphicPanelManager panelManager => GraphicPanelManager.instance;
        public Coroutine FadeIn(float speed, Texture blend = null)
        {
            if (co_fadingOut != null) panelManager.StopCoroutine(co_fadingOut);

            if (co_fadingIn != null) return co_fadingIn;

            co_fadingIn = panelManager.StartCoroutine(Fading(1, speed, blend));

            return co_fadingIn;
        }
        public Coroutine FadeOut(float speed, Texture blend = null)
        {
            if (co_fadingIn != null) panelManager.StopCoroutine(co_fadingIn);

            if (co_fadingOut != null) return co_fadingOut;

            co_fadingOut = panelManager.StartCoroutine(Fading(0, speed, blend));

            return co_fadingOut;
        }
        private IEnumerator Fading(float target, float speed, Texture blend)
        {
            bool isBlending = blend != null;
            bool fadingIn = target > 0;

            renderer.material.SetTexture(MATERIAL_FIELD_BLENDTEXT, blend);
            renderer.material.SetFloat(MATERIAL_FIELD_ALPHA, isBlending ? 1 : fadingIn ? 0 : 1);
            renderer.material.SetFloat(MATERIAL_FIELD_BLEND, isBlending ? fadingIn ? 0 : 1 : 1);

            string opacityParam = isBlending ? MATERIAL_FIELD_BLEND : MATERIAL_FIELD_ALPHA;

            while (renderer.material.GetFloat(opacityParam) != target)
            {
                float opacity = Mathf.MoveTowards(renderer.material.GetFloat(opacityParam), target, Time.deltaTime * speed);
                renderer.material.SetFloat(opacityParam, opacity);
                yield return null;
            }
            co_fadingIn = null;
            co_fadingOut = null;
        }
        #endregion
    }
}