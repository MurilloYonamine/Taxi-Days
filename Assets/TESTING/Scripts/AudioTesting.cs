using System.Collections;
using System.Collections.Generic;
using AUDIO;
using CHARACTERS;
using DIALOGUE;
using GRAPHICS;
using UnityEngine;

namespace TESTING
{
    public class AudioTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }
        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
        IEnumerator Running()
        {
            Character_Sprite lissima = CreateCharacter("Lissima") as Character_Sprite;
            lissima.Show();

            yield return DialogueSystem.instance.Say("Narrator", "Can we see your ship?");

            GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/Background Images/5");
            AudioManager.instance.PlayTrack("Audio/Music/Calm", volumeCap: 0.5f);
            AudioManager.instance.PlayVoice("Audio/Voices/exclamation");

            yield return lissima.Say("Of course!");

            yield return AudioManager.instance.PlayTrack("Audio/Music/Happy", volumeCap: 0.5f);

            yield return null;
        }
    }
}