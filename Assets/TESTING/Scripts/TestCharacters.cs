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
            Character Fada_emo = CreateCharacter("Fada Emo");
            //Character Narrator = CreateCharacter("Narrator");
            Character Gato_de_Botas = CreateCharacter("Gato de Botas");
            Character Lissima = CreateCharacter("Lissima");
            Character Cind = CreateCharacter("Cind");

            Cind.SetPosition(Vector2.zero);
            Fada_emo.SetPosition(new Vector2(0.5f, 0.5f));
            Gato_de_Botas.SetPosition(Vector2.one);
            Lissima.SetPosition(new Vector2(2, 1));

            yield return Cind.MoveToPosition(Vector2.one);

            Fada_emo.Show();
            Cind.Show();
            Gato_de_Botas.Show();

            Fada_emo.Say("Olá, eu sou a Fada Emo.");
            yield return new WaitForSeconds(2f);
            Gato_de_Botas.Say("Eu sou o Gato de Botas.");
            yield return new WaitForSeconds(2f);
            //Lissima.Say("Eu sou a Lissima.");
            //yield return new WaitForSeconds(2f);
            Cind.Say("Eu sou a Cind.");
            //yield return new WaitForSeconds(2f);
            //Narrator.Say("Era uma vez...");
            yield return new WaitForSeconds(2f);
            yield return Cind.MoveToPosition(Vector2.zero);



            yield return null;
        }


    }
}