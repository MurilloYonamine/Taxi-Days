using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TextArchitect
{
    /*
    Classe responsável por construir e revelar texto de forma dinâmica.
    */

    private TextMeshProUGUI tmpro_ui;  // Texto na UI.
    private TextMeshPro tmpro_world;   // Texto genérico caso não haja especifico.
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;  // Escolhe qual usar.

    public string currentText => tmpro.text;  // Texto atual visível.
    public string targetText { get; private set; } = "";  // Texto alvo a ser revelado.
    public string preText { get; private set; } = "";  // Texto já revelado.
    private int preTextLength = 0;  // Armazena o comprimento do texto que já foi revelado, útil na revelação por fade.

    public string fullTargetText => preText + targetText;  // Texto completo (revelado + alvo).

    public enum BuildMethod { instant, typewriter, fade }  // Tipos de revelação, agora inclui "fade".
    public BuildMethod buildMethod = BuildMethod.typewriter;  // Método de revelação, padrão: typewriter.

    public Color TextColor { get { return tmpro.color; } set { tmpro.color = value; } }  // Cor do texto.

    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }  // Velocidade da revelação.
    private const float baseSpeed = 1;  // Velocidade base.
    private float speedMultiplier = 1;  // Multiplicador de velocidade.

    private int characterPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }  // Quantos caracteres revelar por ciclo.
    private int characterMultiplier = 1;

    public bool hurryUp = false;  // Acelera a revelação.

    // Construtores para textos da UI ou do genérico.
    public TextArchitect(TextMeshProUGUI tmpro_ui) { this.tmpro_ui = tmpro_ui; }
    public TextArchitect(TextMeshPro tmpro_world) { this.tmpro_world = tmpro_world; }

    // Inicia a construção de um novo texto.
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    // Adiciona texto ao atual.
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;  // Corrotina de construção.
    public bool isBuilding => buildProcess != null;  // Verifica se está construindo.

    // Para a construção.
    public void Stop()
    {
        if (!isBuilding) return;
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    // Define como o texto será revelado.
    IEnumerator Building()
    {
        Prepare();

        switch (buildMethod)
        {
            case BuildMethod.typewriter:  // Revelação do tipo "máquina de escrever".
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:  // Revelação com fade.
                yield return Build_Fade();
                break;
        }

        OnComplete();
    }

    private void OnComplete() { buildProcess = null; hurryUp = false; }  // Finaliza a construção.

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter: tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount; break;
            case BuildMethod.fade: tmpro.ForceMeshUpdate(); break;
        }
        Stop();
        OnComplete();
    }

    // Prepara o texto antes de começar a revelação.
    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant: Prepare_Instant(); break;  // Revela instantaneamente.
            case BuildMethod.typewriter: Prepare_Typewriter(); break;  // Prepara para efeito de máquina de escrever.
            case BuildMethod.fade: Prepare_Fade(); break;  // Prepara para revelação com fade.
        }
    }

    // Prepara a revelação instantânea.
    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;  // Mantém a cor do texto.
        tmpro.text = fullTargetText;  // Revela o texto inteiro.
        tmpro.ForceMeshUpdate();  // Atualiza a malha de texto.
        tmpro.maxVisibleCharacters = fullTargetText.Length;  // Todos os caracteres visíveis.
    }

    // Prepara a revelação no estilo "máquina de escrever".
    private void Prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;  // Começa sem caracteres visíveis.
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;  // Revela os caracteres do texto anterior.
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();  // Atualiza a malha com o texto alvo.
    }

    // Prepara a revelação por fade.
    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            preTextLength = tmpro.textInfo.characterCount;  // Salva o número de caracteres já visíveis (preText).
        }
        else
        {
            preTextLength = 0;
        }

        tmpro.text += targetText;
        tmpro.maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color colorVisable = new Color(TextColor.r, TextColor.g, TextColor.b, 1);  // Cor do texto visível.
        Color colorHidden = new Color(TextColor.r, TextColor.g, TextColor.b, 0);  // Cor do texto oculto.
        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;  // Configurações dos vértices (cores dos caracteres).

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue;

            if (i < preTextLength)
            {
                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v] = colorVisable;  // Define a cor visível.
                }
            }
            else
            {
                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v] = colorHidden;  // Define a cor como oculta (alpha 0).
                }
            }

            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);  // Atualiza os dados da malha (para aplicar as cores).
        }
    }

    // Corrotina para a revelação do typewriter.
    private IEnumerator Build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? characterPerCycle * 5 : characterPerCycle;  // Revela mais caracteres se "hurryUp" estiver ativo.
            yield return new WaitForSeconds(0.015f / speed);  // Controla a velocidade de revelação.
        }
    }

    // Corrotina para revelação com fade.
    private IEnumerator Build_Fade()
    {
        int midRange = preTextLength;  // Inicializa com o comprimento do preText.
        int maxRange = midRange + 1;  // Definir o intervalo de caracteres a processar.

        byte alphaThreshold = 15;  // Limite para o valor alpha ao qual os caracteres são considerados visíveis.

        TMP_TextInfo textInfo = tmpro.textInfo;  // Recupera informações sobre o texto.

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;  // Cores dos vértices.
        float[] alphas = new float[textInfo.characterCount];  // Array para controlar os valores de alpha dos caracteres.

        while (true)
        {
            float fadeSpeed = ((hurryUp ? characterPerCycle * 5 : characterPerCycle) * speed) * 4f;  // Velocidade de fade, ajustada conforme o "hurryUp".

            for (int i = midRange; i < maxRange; i++)  // Processa cada caractere no intervalo.
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];  // Informações sobre o caractere atual.

                if (!charInfo.isVisible) continue;  // Pula caracteres invisíveis.

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                alphas[i] = Mathf.MoveTowards(alphas[i], 255, fadeSpeed);  // Incrementa gradualmente o alpha.

                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v].a = (byte)alphas[i];  // Atualiza os vértices com o alpha.
                }

                if (alphas[i] >= 255) midRange++;  // Se o caractere está completamente visível, avança o intervalo.
            }
            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);  // Atualiza a malha com as cores modificadas.

            bool lastCharacterIsInvisible = !textInfo.characterInfo[maxRange - 1].isVisible;  // Verifica se o último caractere do intervalo é invisível.
            if (alphas[maxRange - 1] > alphaThreshold || lastCharacterIsInvisible)  // Se o último caractere atingiu o limite de alpha ou é invisível...
            {
                if (maxRange < textInfo.characterCount)
                {
                    maxRange++;  // Expande o intervalo.
                }
                else if (alphas[maxRange - 1] >= 255 || lastCharacterIsInvisible)  // Se todos os caracteres estão visíveis ou invisíveis, encerra o loop.
                {
                    break;
                }
            }

            yield return new WaitForEndOfFrame();  // Espera até o fim do frame antes de continuar o loop.
        }
    }
}
