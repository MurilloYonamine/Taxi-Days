using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace VISUALNOVEL
{
    /// <summary>
    /// Handles the Visual Novel startup and loading operations
    /// </summary>
    public class VNManager : MonoBehaviour
    {
        public static VNManager instance { get; private set; }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
            VNDatabaseLinkSetup linkSetup = GetComponent<VNDatabaseLinkSetup>();
            linkSetup.SetupExternalLinks();
        }
        public void LoadFile(string filePath)
        {
            List<string> lines = new List<string>();
            TextAsset file = Resources.Load<TextAsset>(filePath);

            try
            {
                lines = FileManager.ReadTextAsset(file);
            }
            catch
            {
                Debug.LogError($"O arquivo de diálogo do caminho 'Resources/{filePath}' não foi encontrado!");
                return;
            }
            DialogueSystem.instance.Say(lines, filePath);
        }

    }
}