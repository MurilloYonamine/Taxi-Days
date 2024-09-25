using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiles : MonoBehaviour
{
    // Arquivo de texto que será lido.
    [SerializeField] private TextAsset fileName;

    void Start()
    {
        // Inicia a rotina para ler e processar o arquivo de texto ao iniciar o jogo.
        StartCoroutine(Run());
    }

    // Corrotina que gerencia o processo de leitura e exibição das linhas.
    private IEnumerator Run()
    {
        List<string> lines = FileManager.ReadTextAsset(fileName, false); // Lê o conteúdo do arquivo de texto e armazena em uma lista de strings.

        foreach (string line in lines)
        {
            Debug.Log(line);
        }
        // Exibe cada linha do arquivo no console da Unity para fins de debug.

        yield return null;
    }
}
