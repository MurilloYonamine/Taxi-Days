using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        void Start()
        {
            Character Fada_emo = CharacterManager.instance.CreateCharacter("Fada Emo");
            Character Fada_emo2 = CharacterManager.instance.CreateCharacter("Fada Emo");
            Character Narrator = CharacterManager.instance.CreateCharacter("Narrator");
            Character Gato_de_Botas = CharacterManager.instance.CreateCharacter("Gato de Botas");

            Character Lissima = CharacterManager.instance.CreateCharacter("Lissima");
        }

        void Update()
        {

        }
    }
}