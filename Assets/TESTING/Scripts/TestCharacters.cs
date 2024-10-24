using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;
using UnityEditor.U2D;

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
            //Character_Sprite Cind = CreateCharacter("Cind") as Character_Sprite;

            Gato_de_Botas.SetPosition(new Vector2(0.6f, 0.5f));
            Lissima.SetPosition(new Vector2(0.3f, 0.5f));

            yield return new WaitForSeconds(1);

            yield return Lissima.Flip(0.3f);

            yield return Gato_de_Botas.FaceRight(immediate: false);

            yield return Gato_de_Botas.FaceLeft(immediate: false);

            yield return Lissima.UnHighlight();

            yield return new WaitForSeconds(1);

            yield return Lissima.TransitionColor(Color.red);

            yield return new WaitForSeconds(1);

            yield return Lissima.Highlight();

            yield return new WaitForSeconds(1);

            yield return Lissima.TransitionColor(Color.white);

            Lissima.Say("Hello, I'm Lissima!");

            yield return null;
        }


    }
}