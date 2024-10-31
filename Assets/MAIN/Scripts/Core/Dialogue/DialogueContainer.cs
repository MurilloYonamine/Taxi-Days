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
        private const float DEFAULT_FADE_SPEED = 2f;

        public GameObject root;
        public GameObject rootContainer;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;

        private CanvasGroup rootCG => rootContainer.GetComponent<CanvasGroup>();

        private Coroutine co_showing = null;
        private Coroutine co_hiding = null;

        public bool isShowing => co_showing != null;
        public bool isHiding => co_hiding != null;
        public bool isFading => isShowing || isHiding;

        public bool isVisible => co_showing != null || rootCG.alpha > 0;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontText(float size) => dialogueText.fontSize = size;

        public Coroutine Show()
        {
            if (isShowing) return co_showing;
            else if (isHiding)
            {
                DialogueSystem.instance.StopCoroutine(co_hiding);
                co_hiding = null;
            }
            co_showing = DialogueSystem.instance.StartCoroutine(Fading(1));

            return co_showing;
        }
        public Coroutine Hide()
        {
            if (isHiding) return co_hiding;
            else if (isShowing)
            {
                DialogueSystem.instance.StopCoroutine(co_showing);
                co_showing = null;
            }
            co_hiding = DialogueSystem.instance.StartCoroutine(Fading(0));

            return co_hiding;
        }
        private IEnumerator Fading(float alpha)
        {
            CanvasGroup cg = rootCG;

            while (cg.alpha != alpha)
            {
                cg.alpha = Mathf.MoveTowards(cg.alpha, alpha, Time.deltaTime * DEFAULT_FADE_SPEED);
                yield return null;
            }
            co_showing = null;
            co_hiding = null;
        }
    }
}