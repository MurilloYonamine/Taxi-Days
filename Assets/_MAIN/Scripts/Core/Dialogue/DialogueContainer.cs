using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CHARACTERS;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;
        public RectTransform dialogueRect => root.GetComponent<RectTransform>();

        private CanvasGroupController cgController;
        private Vector3 initialPosition;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontSize(float size) => dialogueText.fontSize = size;

        private bool initialized = false;
        public void Initialize()
        {
            if (initialized)
                return;

            cgController = new CanvasGroupController(DialogueSystem.instance, root.GetComponent<CanvasGroup>());
            initialPosition = dialogueRect.anchoredPosition;
        }
        public void NarratorDialogueContainer()
        {
            root.GetComponent<Image>().enabled = false;
            dialogueRect.anchoredPosition = new Vector3(0, 0, 10);
        }
        public void DialogueContainerCharacterPosition(string characterName)
        {
            root.GetComponent<Image>().enabled = true;
            GameObject characterGameObject = GameObject.Find($"Character - [{characterName}]");
            if (characterGameObject == null)
            {
                dialogueRect.anchoredPosition = initialPosition;
                return;
            }

            Vector3 containerPosition = characterGameObject.transform.position - new Vector3(0, 1.5f);
            root.transform.position = containerPosition;
        }
        public void Clear()
        {
            dialogueText.text = string.Empty;
            nameContainer.nameText.text = string.Empty;
        }
        public void DialogueContainerItalic(bool isThought)
        {
            if (isThought)
            {
                root.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.65f);
                dialogueText.fontStyle = FontStyles.Italic;
                nameContainer.nameText.fontStyle = FontStyles.Italic;
            }
            else
            {
                root.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.87f);
                dialogueText.fontStyle = FontStyles.Normal;
                nameContainer.nameText.fontStyle = FontStyles.Normal;
            }
        }
        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);
        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
    }
}