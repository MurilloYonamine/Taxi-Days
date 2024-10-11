using System.Collections;
using UnityEngine;
using TMPro;
using Entities;

namespace Dialogue
{
    public class DialogueBox : MonoBehaviour // Classe para exibir o diálogo de uma cena
    {
        private CanvasGroup canvasGroup; // Componente de grupo de canvas para controlar a visibilidade da caixa de diálogo
        [SerializeField] private TextMeshProUGUI nameText; // Componente de texto para o nome do personagem
        [SerializeField] private TextMeshProUGUI dialogueText; // Componente de texto para o diálogo

        private int sentenceIndex = -1; // Índice da sentença atual
        [SerializeField] private StoryScene currentScene; // Cena atual da história

        private State state = State.COMPLETED; // Estado atual do diálogo
        private enum State { PLAYING, COMPLETED } // Estados possíveis do diálogo

        private bool isHidden = false; // Flag para verificar se a caixa de diálogo está escondida


        // Método chamado ao inicializar o componente
        private void Awake()
        {
            GetTextComponents();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        // Esconde a caixa de diálogo gradualmente
        public void Hide()
        {
            if (!isHidden)
            {
                StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, 0.5f));
                isHidden = true;
            }
        }

        // Mostra a caixa de diálogo gradualmente
        public void Show()
        {
            if (isHidden)
            {
                StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, 0.5f));
                isHidden = false;
            }
        }

        // Coroutine para fazer o fade in/out do CanvasGroup
        private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            cg.alpha = end;
        }
        // Inicia a reprodução de uma cena
        public void PlayScene(StoryScene scene)
        {
            currentScene = scene;
            sentenceIndex = -1;
            PlayNextSentence();
        }

        // Reproduz a próxima sentença da cena
        public void PlayNextSentence()
        {
            StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
            nameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
            nameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
        }

        public bool IsCompleted() => state == State.COMPLETED; // Verifica se o diálogo foi completado
        public bool IsLastSentence() => sentenceIndex + 1 == currentScene.sentences.Count; // Verifica se a sentença atual é a última da cena
        public void ClearDialogueText() => dialogueText.text = ""; // Limpa o texto da caixa de diálogo

        // Coroutine para exibir o texto da sentença gradualmente
        private IEnumerator TypeText(string text)
        {
            dialogueText.text = "";
            state = State.PLAYING;
            int wordIndex = 0;

            while (state != State.COMPLETED)
            {
                dialogueText.text += text[wordIndex];
                yield return new WaitForSeconds(0.05f);

                if (++wordIndex == text.Length)
                {
                    state = State.COMPLETED;
                    break;
                }
            }
        }
        // Busca os componentes de texto nos filhos do objeto com este script
        private void GetTextComponents()
        {
            TextMeshProUGUI[] textComponents = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var textComponent in textComponents)
            {
                if (textComponent.name == "Name Text") nameText = textComponent;
                else if (textComponent.name == "Dialogue Text") dialogueText = textComponent;
            }
            if (nameText == null || dialogueText == null)
            {
                Debug.LogError("Não foram encontrados componentes TextMeshProUGUI com os nomes 'Name Text' e 'Dialogue Text' nos objetos filhos.");
            }
        }
    }
}