using System.Collections;
using System.Collections.Generic;
using AUDIO;
using CHARACTERS;
using UnityEngine;

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

        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1f);

        AudioManager.instance.PlaySoundEffect("Audio/SFX/RadioStatic", loop: true);

        yield return lissima.Say("U gay.");
        
        AudioManager.instance.StopSoundEffect("RadioStatic");

        lissima.Say("nah. i think u are not.");
    }
}
