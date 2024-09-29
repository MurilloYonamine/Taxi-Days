using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        /*
        The base class from witch all characers types devire from.
        */
        public string name = "";
        public RectTransform root = null;

        public Character(string name)
        {
            this.name = name;
        }

        public enum CharacterType
        {
            Text, // No graphics on the screen
            Sprite, // Texture as sprites
            SpriteSheet, // Multiple sprites per texture
            Live2D,
            Model3D
        }
    }
}