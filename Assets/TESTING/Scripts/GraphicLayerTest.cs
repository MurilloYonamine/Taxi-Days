using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRAPHICS
{
    public class GraphicLayerTest : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Running());
        }
        IEnumerator Running()
        {
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
            GraphicLayer layer = panel.GetLayer(0, true);

            yield return new WaitForSeconds(1f);

            Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");
            //layer.SetTexture("Graphics/BG Images/2", blendingTexture: blendTex);

            layer.SetVideo("Graphics/BG Videos/Fantasy Landscape", transitionSpeed: 0.01f, useAudio: true);
        }
    }
}