using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        /*
        A character that uses sprites or
        sprite sheets to render its display
        */
        private const string SPRITE_RENDERERD_PARENT_NAME = "Renderers";
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();
        private List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();
        private string artAssetsDirectory = "";
        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = 0;
            artAssetsDirectory = rootAssetsFolder + "/Images";
            GetLayers();
            Debug.Log($"Foi criado um personagem do tipo 'Sprite' chamado: '{name}'.");
        }
        private void GetLayers()
        {
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERERD_PARENT_NAME);

            if (rendererRoot == null) return;

            for (int i = 0; i < rendererRoot.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponent<Image>();

                if (rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                };

            }
        }
        public void SetSprite(Sprite sprite, int layer = 0)
        {
            if (layer < 0 || layer >= layers.Count) return;

            layers[layer].SetSprite(sprite);
        }
        public Sprite GetSprite(string spriteName)
        {
            if (config.characterType == CharacterType.SpriteSheet)
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
            }
            else
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
            }
        }
        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, Time.deltaTime * 3f);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }
    }
}