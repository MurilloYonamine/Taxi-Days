using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiles : MonoBehaviour
{

    [SerializeField] private TextAsset fileName;

    void Start()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        List<string> lines = FileManager.ReadTextAsset(fileName, false);

        foreach (string line in lines)
        {
            Debug.Log(line);
        }

        yield return null;
    }
}
