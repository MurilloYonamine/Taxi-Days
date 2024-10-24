using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;
using UnityEditor.U2D;
using System;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        public TMP_FontAsset tempFont;
        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
        void Start()
        {
            //Character Fada_emo = CharacterManager.instance.CreateCharacter("Fada Emo");
            //Character Narrator = CharacterManager.instance.CreateCharacter("Narrator");
            //Character Gato_de_Botas = CharacterManager.instance.CreateCharacter("Gato de Botas");
            //Character Lissima = CharacterManager.instance.CreateCharacter("Lissima");
            //Character Cind = CharacterManager.instance.CreateCharacter("Cind");

            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            //Character_Sprite Fada_emo = CreateCharacter("Fada Emo") as Character_Sprite;
            //Character Narrator = CreateCharacter("Narrator");
            Character_Sprite Gato_de_Botas = CreateCharacter("Gato de Botas") as Character_Sprite;
            Character_Sprite Lissima = CreateCharacter("Lissima") as Character_Sprite;
            Character_Sprite Cind = CreateCharacter("Cind") as Character_Sprite;
            Character_Sprite CindRed = CreateCharacter("Cind Red as Generic") as Character_Sprite;

            //Gato_de_Botas.SetPosition(new Vector2(0.6f, 0.5f));
            //Lissima.SetPosition(new Vector2(0.3f, 0.5f));

            CindRed.SetColor(Color.red);

            Gato_de_Botas.SetPosition(new Vector2(0.3f, 0.5f));
            Lissima.SetPosition(new Vector2(0.45f, 0.5f));
            Cind.SetPosition(new Vector2(0.6f, 0.5f));
            CindRed.SetPosition(new Vector2(0.75f, 0.5f));

            CindRed.SetPriority(1000);
            Lissima.SetPriority(15);
            Gato_de_Botas.SetPriority(8);
            Cind.SetPriority(30);

            yield return new WaitForSeconds(1);
            CharacterManager.instance.SortCharacters(new string[] { "Lissima", "Gato de Botas" });

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters();

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters(new string[] { "Gato de Botas", "Cind Red", "Cind", "Lissima" });


            yield return null;
        }
    }
}