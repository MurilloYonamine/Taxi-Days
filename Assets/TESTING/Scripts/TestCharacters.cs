using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        public TMP_FontAsset tempFont;
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
            yield return new WaitForSeconds(1f);
            Character Fada_emo = CharacterManager.instance.CreateCharacter("Fada Emo");

            yield return new WaitForSeconds(1f);
            yield return Fada_emo.Hide();

            yield return new WaitForSeconds(0.5f);
            yield return Fada_emo.Show();

            yield return Fada_emo.Say("Olá, eu sou a Fada Emo.");
        }


    }
}