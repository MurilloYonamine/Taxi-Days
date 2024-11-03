using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueContinuePrompt : MonoBehaviour
    {
        /*
        Shows a prompt at the end of dialogue
        to tell the user when input is expected
        */
        private RectTransform root;
        [SerializeField] private Animator anim;
        [SerializeField] private TextMeshProUGUI tmpro;

        public bool isShowing => anim.gameObject.activeSelf;
        void Start()
        {
            root = GetComponent<RectTransform>();
        }
        public void UpdatePosition()
        {
            if (tmpro.textInfo.characterCount == 0)
            {
                Hide();
                return;
            }

            tmpro.ForceMeshUpdate();
            TMP_CharacterInfo finalCharacter = tmpro.textInfo.characterInfo[tmpro.textInfo.characterCount - 1];
            Vector3 targetPos = finalCharacter.bottomRight;
            float characterWidth = finalCharacter.pointSize * 0.8f;
            targetPos = new Vector3(targetPos.x + characterWidth, targetPos.y, 0);
            root.localPosition = targetPos;
        }

        public void Show()
        {
            if (tmpro.text == string.Empty)
            {
                if (isShowing) Hide();
                return;
            }
            tmpro.ForceMeshUpdate();

            anim.gameObject.SetActive(true);
            root.transform.SetParent(tmpro.transform);

            TMP_CharacterInfo finalCharacter = tmpro.textInfo.characterInfo[tmpro.textInfo.characterCount - 1];
            Vector3 targetPos = finalCharacter.bottomRight;
            float characterWidth = finalCharacter.pointSize * 0.8f;
            targetPos = new Vector3(targetPos.x + characterWidth, targetPos.y, 0);

            root.localPosition = targetPos;
        }
        public void Hide()
        {
            anim.gameObject.SetActive(false);
        }
    }
}