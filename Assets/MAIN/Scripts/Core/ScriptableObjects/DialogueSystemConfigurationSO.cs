using CHARACTERS;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        /* 
        Scriptable Object that defines the parameters
        for configuring the dialogue system as a whole
        */
        public CharacterConfigSO characterConfigurationAsset;
        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;
    }
}