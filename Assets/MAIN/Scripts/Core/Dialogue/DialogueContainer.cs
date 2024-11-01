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

        private CanvasGroupController cgController;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontText(float size) => dialogueText.fontSize = size;

        private bool initialized = false;

        public void Initialize()
        {
            if (initialized) return;

            cgController = new CanvasGroupController(DialogueSystem.instance, root.GetComponent<CanvasGroup>());
        }

        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);
        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
        public void SetRootContainerPosition(string characterName)
        {
            GameObject gameObject = GameObject.Find($"Character - [{characterName}]");
            if (gameObject == null) return;
            Vector3 containerPosition = gameObject.transform.position - new Vector3(0, 1);
            root.transform.position = containerPosition;
        }
    }
}