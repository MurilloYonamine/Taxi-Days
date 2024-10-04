using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Live2D : Character
    {
        /*
        A character that uses Live2D technology
        to render an animated graphical display.
        */
        public Character_Live2D(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Foi criado um personagem do tipo 'Live2D' chamado: '{name}'.");
        }
    }
}