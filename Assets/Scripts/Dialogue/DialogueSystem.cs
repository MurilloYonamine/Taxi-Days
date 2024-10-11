using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour // Classe para controlar a história
    {
        public StoryScene currentScene; // Cena atual da história
        public DialogueBox dialogueBox; // Caixa de diálogo para exibir as falas

        private State state = State.IDLE;
        private enum State { IDLE, ANIMATE }

        // Método chamado ao inicializar o componente
        void Start()
        {
            dialogueBox = FindObjectOfType<DialogueBox>();
            dialogueBox.PlayScene(currentScene);
        }

        // Método chamado a cada frame para verificar entradas do usuário
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (state == State.IDLE && dialogueBox.IsCompleted())
                {
                    if (dialogueBox.IsLastSentence())
                    {
                        PlayScene(currentScene.nextScene); // Avança para a próxima cena
                    }
                    else
                    {
                        dialogueBox.PlayNextSentence(); // Reproduz a próxima sentença
                    }
                }
            }
        }
        // Método para iniciar a reprodução de uma cena
        public void PlayScene(StoryScene scene)
        {
            StartCoroutine(SwitchScene(scene));
        }

        // Método para iniciar a reprodução de uma cena
        private IEnumerator SwitchScene(StoryScene scene)
        {
            state = State.ANIMATE;
            currentScene = scene;
            dialogueBox.Hide();
            yield return new WaitForSeconds(1f);
            dialogueBox.Show();
            yield return new WaitForSeconds(0.01f);
            dialogueBox.PlayScene(currentScene);
            state = State.IDLE;
        }
    }
}