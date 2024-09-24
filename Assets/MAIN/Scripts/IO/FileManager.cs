using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    /*
    Handles saving, loading, and encrypting of files in the project.
    */

    public static List<string> ReadTextFile(string filePath, bool includBlankLines = true)
    {
        if (!filePath.StartsWith('/')) filePath = FilePaths.root + filePath;

        List<string> lines = new List<string>();

        try
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    if (includBlankLines || !string.IsNullOrEmpty(line)) lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException exception)
        {
            Debug.LogError($"Pasta não encontrada: '{exception.FileName}'");
        }
        return lines;
    }
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);

        if (asset == null)
        {
            Debug.LogError($"Asset não encontrado: '{filePath}'");
            return null;
        }
        return ReadTextAsset(asset, includeBlankLines);
    }
    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader stringReader = new StringReader(asset.text))
        {
            while (stringReader.Peek() > -1)
            {
                string line = stringReader.ReadLine();
                if (includeBlankLines || !string.IsNullOrEmpty(line)) lines.Add(line);
            }
        }
        return lines;
    }
}
