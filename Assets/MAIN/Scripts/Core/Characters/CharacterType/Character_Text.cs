using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Text : Character
    {
        /*
        A character with no graphical art.
        Text Operations only.
        */
        public Character_Text(string name, CharacterConfigData config) : base(name, config, prefab: null)
        {
            Debug.Log($"Foi criado um personagem do tipo 'Texto' chamado: '{name}'.");
        }
    }
}