using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

namespace GRAPHICS
{
    public class GraphicLayerTest : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Running());
        }
        IEnumerator Running()
        {
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
            GraphicLayer layer0 = panel.GetLayer(0, true);
            GraphicLayer layer1 = panel.GetLayer(1, true);

            layer0.SetVideo("Graphics/BG Videos/Nebula");
            layer1.SetTexture("Graphics/BG Images/Spaceshipinterior");

            yield return new WaitForSeconds(1f);

            GraphicPanel cinematic = GraphicPanelManager.instance.GetPanel("Cinematic");
            GraphicLayer cinLayer = cinematic.GetLayer(0, true);

            Character gato = CharacterManager.instance.CreateCharacter("Gato", true);

            yield return gato.Say("Hello, world!");

            cinLayer.SetTexture("Graphics/Gallery/pup");

            yield return DialogueSystem.instance.Say("Narrator", "The cat said hello to the world.");

            cinLayer.Clear();

            yield return new WaitForSeconds(1f);
            
            panel.Clear();
        }
    }
}