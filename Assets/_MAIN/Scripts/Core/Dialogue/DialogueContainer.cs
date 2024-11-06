using UnityEngine;
using TMPro;
using System.Collections;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;

        private CanvasGroupController cgController;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontSize(float size) => dialogueText.fontSize = size;

        private Vector3 containerInitialPosition;
        private bool initialized = false;

        public void Initialize()
        {
            if (initialized)
                return;

            containerInitialPosition = root.transform.position;  // Armazena a posição inicial apenas uma vez
            cgController = new CanvasGroupController(DialogueSystem.instance, root.GetComponent<CanvasGroup>());
            initialized = true;
        }

        public void SetRootContainerPosition(string characterName)
        {
            GameObject gameObject = GameObject.Find($"Character - [{characterName}]");
            if (gameObject == null)
            {
                root.transform.position = containerInitialPosition;
                return;
            }

            Vector3 containerPosition = gameObject.transform.position - new Vector3(0, 3);
            root.transform.position = containerPosition;
        }

        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);
        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
    }
}