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

            Cind.SetPosition(new Vector2(0.2f, 0.5f));
            Lissima.SetPosition(new Vector2(0.8f, 0.5f));

            Lissima.Show();
            Cind.Show();

            Gato_de_Botas.SetPriority(1);
            Lissima.SetPriority(2);
            Cind.SetPriority(3);

            yield return Lissima.Say("Como você está?");
            yield return Cind.Say("Estou... bem.");
            yield return Lissima.Say("Sério mesmo?");

            Gato_de_Botas.Show();
            yield return Gato_de_Botas.Say("Olá, Jovens, qual o nome de vocês?");

            Cind.TransitionSprite(Cind.GetSprite("cind_1"));
            yield return Cind.Say("Meu nome é Cind!");

            Lissima.TransitionSprite(Lissima.GetSprite("lissima_pretty_2"));
            yield return Lissima.Say("Me chamo Lissima.");

            yield return Gato_de_Botas.Say("Entendi. Posso me juntar a vocês?");
            yield return Lissima.Say("Claro, quanto mais, melhor!");

            yield return null;
        }
    }
}