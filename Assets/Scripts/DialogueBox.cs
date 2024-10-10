using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private int sentenceIndex = -1;
    [SerializeField] private StoryScene currentScene;
    private State state = State.COMPLETED;

    private enum State { PLAYING, COMPLETED }
    private void Awake()
    {
        GetTextComponents();
    }
    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        dialogueText.text = ""; 
        PlayNextSentence();
    }
    public void PlayNextSentence()
    {
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
        nameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        nameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
    }
    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }
    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }
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