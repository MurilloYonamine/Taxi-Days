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
            //Character Fada_emo2 = CharacterManager.instance.CreateCharacter("Fada Emo");
            //Character Narrator = CharacterManager.instance.CreateCharacter("Narrator");
            //Character Gato_de_Botas = CharacterManager.instance.CreateCharacter("Gato de Botas");

            //Character Lissima = CharacterManager.instance.CreateCharacter("Lissima");

            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            Character Fada_emo = CharacterManager.instance.CreateCharacter("Fada emo");
            Character Gato_de_Botas = CharacterManager.instance.CreateCharacter("Gato de Botas");
            Character Ben = CharacterManager.instance.CreateCharacter("Benjamin");
            Character Adam = CharacterManager.instance.CreateCharacter("Adam");

            List<string> lines = new List<string>()
            {
                "Hi, there!",
                "My name is Fada emo.",
                "What's your name?",
                "Oh,{wa 1} that's very nice."
            };

            foreach (string line in lines) { yield return Fada_emo.Say(line); }

            Fada_emo.SetNameColor(Color.red);
            Fada_emo.SetDialogueColor(Color.green);
            Fada_emo.SetNameFont(tempFont);
            Fada_emo.SetDialogueFont(tempFont);
            foreach (string line in lines) { yield return Fada_emo.Say(line); }

            Fada_emo.ResetConfiguration();
            foreach (string line in lines) { yield return Fada_emo.Say(line); }
            
            lines = new List<string>()
            {
                "I am Gato de Botas.",
                "More lines{c}Here."
            };

            foreach (string line in lines) { yield return Gato_de_Botas.Say(line); }

            lines = new List<string>()
            {
                "I am Adam.",
                "More lines{c}Here."
            };

            foreach (string line in lines) { yield return Adam.Say(line); }

            yield return Ben.Say("This is a line that I want to say.{a} It is a simple line.");

            Debug.Log("Finished");
        }


    }
}