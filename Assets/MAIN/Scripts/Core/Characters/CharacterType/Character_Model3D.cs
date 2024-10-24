using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Model3D : Character
    {
        /*
        A character that uses a 3D model
        to render their display in the scene.
        */
        public Character_Model3D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            Debug.Log($"Foi criado um personagem do tipo 'Modelo 3D' chamado: '{name}'.");
        }
    }
}