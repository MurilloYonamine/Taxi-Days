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
            //Character_Sprite Gato_de_Botas = CreateCharacter("Gato de Botas") as Character_Sprite;

            Character_Sprite Lissima = CreateCharacter("Lissima") as Character_Sprite;
            Character_Sprite Cind = CreateCharacter("Cind") as Character_Sprite;

            Lissima.isVisible = false;

            yield return new WaitForSeconds(1f);
            Sprite s1 = Cind.GetSprite("Second Form/cind_3");
            Cind.TransitionSprite(s1);

            Cind.MoveToPosition(new Vector2(0, 0), 1f);
            yield return Lissima.Show();

            yield return new WaitForSeconds(1f);

            Cind.TransitionSprite(Cind.GetSprite("First Form/cind"));

            yield return null;
        }


    }
}