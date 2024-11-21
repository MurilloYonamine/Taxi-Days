using System.Collections;
using UnityEngine;

public class TABuilder_Typewriter : TABuilder
{
    public override Coroutine Build(AudioClip sound = null)
    {
        Prepare();
        return architect.tmpro.StartCoroutine(Building(sound));
    }

    public override void ForceComplete()
    {
        architect.tmpro.ForceMeshUpdate();
        architect.tmpro.maxVisibleCharacters = architect.tmpro.textInfo.characterCount;
    }

    private void Prepare()
    {
        architect.tmpro.color = architect.tmpro.color;
        architect.tmpro.maxVisibleCharacters = 0;
        architect.tmpro.text = architect.preText;

        if (architect.preText != "")
        {
            architect.tmpro.ForceMeshUpdate();
            architect.tmpro.maxVisibleCharacters = architect.tmpro.textInfo.characterCount;
        }

        architect.tmpro.text += architect.targetText;
        architect.tmpro.ForceMeshUpdate();
    }

    private IEnumerator Building(AudioClip sound)
    {
        string fullText = architect.tmpro.text; // Texto completo
        string[] words = fullText.Split(' ');   // Divide o texto em palavras
        int wordIndex = 0;                      // Índice da palavra atual
        int nextWordStart = 0;                  // Posição inicial da próxima palavra

        // Calcula a posição inicial de cada palavra
        for (int i = 0; i < wordIndex; i++)
        {
            nextWordStart += words[i].Length + 1; // +1 para o espaço
        }

        while (architect.tmpro.maxVisibleCharacters < architect.tmpro.textInfo.characterCount)
        {
            // Incrementa os caracteres visíveis
            architect.tmpro.maxVisibleCharacters += architect.hurryUp
                ? architect.charactersPerCycle * 2
                : architect.charactersPerCycle;

            // Verifica se é hora de tocar o som (no início de uma nova palavra)
            if (wordIndex < words.Length && architect.tmpro.maxVisibleCharacters >= nextWordStart)
            {
                if (sound != null)
                {
                    AudioManager.instance.PlayVoice(sound);
                }

                wordIndex++; // Avança para a próxima palavra

                // Atualiza o início da próxima palavra
                if (wordIndex < words.Length)
                {
                    nextWordStart += words[wordIndex].Length + 1;
                }
            }

            // Aguarda um pouco antes de processar o próximo ciclo
            yield return new WaitForSeconds(0.015f / (architect.hurryUp ? architect.speed * 5 : architect.speed));
        }

        OnComplete();
    }

}
