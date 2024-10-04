using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        /*
        The dialogue graphical display with all
        dialogue box elements. Can be altered for
        different looks
        */
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
    }
}