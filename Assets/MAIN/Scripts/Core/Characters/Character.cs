using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        /*
        The base class from witch all characers types devire from.
        */

        // Constants
        public const bool ENABLE_ON_START = true;
        private const float UNHIGHLIGHTED_DARKEN_STRENGHT = 0.65F;
        public const bool DEFAULT_ORIENTATION_IS_FACING_LEFT = true;

        // Fields
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Animator animator;

        public Color color { get; protected set; } = Color.white;
        protected Color displayColor => highlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => color;
        protected Color unhighlightedColor => new Color(color.r * UNHIGHLIGHTED_DARKEN_STRENGHT, color.g * UNHIGHLIGHTED_DARKEN_STRENGHT, color.b * UNHIGHLIGHTED_DARKEN_STRENGHT, color.a);

        public bool highlighted { get; protected set; } = true;
        protected bool facingLeft = DEFAULT_ORIENTATION_IS_FACING_LEFT;

        public int priority { get; protected set; }

        protected CharacterManager characterManager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        // Coroutines
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        protected Coroutine co_highlighting;
        protected Coroutine co_flipping;


        // Properties
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public bool isChangingColor => co_changingColor != null;
        public bool isHighlighting => (highlighted && co_highlighting != null);
        public bool isUnHighlighting => (!highlighted && co_highlighting != null);
        public virtual bool isVisible { get; set; }
        public bool isFacingLeft => facingLeft;
        public bool isFacingRight => !facingLeft;
        public bool isFlipping => co_flipping != null;

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, characterManager.characterPanel);
                ob.name = characterManager.FormatCharacterPath(characterManager.characterPrefabNameFormat, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        #region Dialogue Methods
        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        private Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationsOnScreen();
            return DialogueSystem.instance.Say(dialogue);
        }
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void ResetConfiguration() => config = CharacterManager.instance.GetCharacterConfig(name);
        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        #endregion
        
        #region Visibility Methods
        public virtual Coroutine Show()
        {
            if (isRevealing) return co_revealing;

            if (isHiding) characterManager.StopCoroutine(co_hiding);

            co_revealing = characterManager.StartCoroutine(ShowingOrHiding(true));

            return co_revealing;
        }
        public virtual Coroutine Hide()
        {
            if (isHiding) return co_hiding;

            if (isRevealing) characterManager.StopCoroutine(co_hiding);

            co_hiding = characterManager.StartCoroutine(ShowingOrHiding(false));

            return co_hiding;
        }
        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.Log("Aparecendo ou desaparecendo não pode ser chamado por uma base de personagem.");
            yield return null;
        }
        #endregion

        #region Position Methods
        public virtual void SetPosition(Vector2 position)
        {
            if (root == null) return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterArchorTargets(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }
        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (root == null) return null;

            if (isMoving) characterManager.StopCoroutine(co_moving);

            co_moving = characterManager.StartCoroutine(MovingToPosition(position, speed, smooth));

            return co_moving;
        }
        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterArchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = smooth ?
                    Vector2.Lerp(root.anchorMin, minAnchorTarget, Time.deltaTime * speed) :
                    Vector2.MoveTowards(root.anchorMin, minAnchorTarget, Time.deltaTime * speed);

                root.anchorMax = root.anchorMin + padding;

                if (smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) < 0.0001f)
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;
                    break;
                }
                yield return null;
            }
            Debug.Log("Personagem movido com sucesso.");
            co_moving = null;
        }
        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterArchorTargets(Vector2 posiiton)
        {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minArchorTarget = new Vector2(maxX * posiiton.x, maxY * posiiton.y);
            Vector2 maxArchorTarget = minArchorTarget + padding;

            return (minArchorTarget, maxArchorTarget);
        }
        #endregion

        #region Color Methods
        public virtual void SetColor(Color color)
        {
            this.color = color;
        }
        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            this.color = color;

            if (isChangingColor) characterManager.StopCoroutine(co_changingColor);

            co_changingColor = characterManager.StartCoroutine(ChangingColor(displayColor, speed));

            return co_changingColor;
        }

        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Mudando a cor do personagem não é aplicável ao tipo de personagem.");
            yield return null;
        }
        #endregion

        #region Highlight Methods
        public Coroutine Highlight(float speed = 1f)
        {
            if (isHighlighting) return co_highlighting;

            if (isUnHighlighting) characterManager.StopCoroutine(co_highlighting);

            highlighted = true;
            co_highlighting = characterManager.StartCoroutine(Highlighting(highlighted, speed));

            return co_highlighting;
        }
        public Coroutine UnHighlight(float speed = 1f)
        {
            if (isUnHighlighting) return co_highlighting;

            if (isHighlighting) characterManager.StopCoroutine(co_highlighting);

            highlighted = false;
            co_highlighting = characterManager.StartCoroutine(Highlighting(highlighted, speed));

            return co_highlighting;
        }

        public virtual IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            Debug.Log("Highlighting não está disponível para este tipo de personagem.");
            yield return null;
        }
        #endregion

        #region Flip Methods
        public Coroutine Flip(float speed = 1, bool immediate = false)
        {
            if (isFacingLeft) return FaceRight(speed, immediate);
            else return FaceLeft(speed, immediate);
        }
        public Coroutine FaceLeft(float speed = 1, bool immediate = false)
        {
            if (isFlipping) characterManager.StopCoroutine(co_flipping);

            facingLeft = true;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingLeft, speed, immediate));
            return co_flipping;
        }
        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping) characterManager.StopCoroutine(co_flipping);

            facingLeft = false;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingLeft, speed, immediate));

            return co_flipping;
        }
        public virtual IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            Debug.Log("Não pode flippar o personagem desse tipo!");
            yield return null;
        }
        #endregion

        public void SetPriority(int priority, bool autoSortCharactersOnUI = true)
        {
            this.priority = priority;
            if(autoSortCharactersOnUI) characterManager.SortCharacters();
        }
        public enum CharacterType
        {
            Text, // No graphics on the screen
            Sprite, // Texture as sprites
            SpriteSheet, // Multiple sprites per texture
            Live2D,
            Model3D
        }

    }
}