using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class CharacterSpriteLayer
    {
        /*
        Contains all data and functions available
        to a layer composing a sprite character
        */
        private CharacterManager characterManager => CharacterManager.instance;
        public int layer { get; set; } = 0;
        public Image renderer { get; private set; } = null;
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>();
        private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();

        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            this.layer = layer;
            renderer = defaultRenderer;
        }
        public void SetSprite(Sprite sprite)
        {
            renderer.sprite = sprite;
        }
    }
}
