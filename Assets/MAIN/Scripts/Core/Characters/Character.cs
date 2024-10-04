using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        /*
        The base class from witch all characers types devire from.
        */
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;

        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            this.config = config;
            displayName = name;
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        private Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            dialogueSystem.ApplySpeakerDataToDialogueContainer(name);
            return DialogueSystem.instance.Say(dialogue);
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