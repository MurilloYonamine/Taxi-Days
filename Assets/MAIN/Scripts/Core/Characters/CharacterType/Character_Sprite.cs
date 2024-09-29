using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        /*
        A character that uses sprites or
        sprite sheets to render its display
        */
        public Character_Sprite(string name) : base(name)
        {
            Debug.Log($"Foi criado um personagem do tipo 'Sprite' chamado: '{name}'.");
        }
    }
}