using DIALOGUE;
using History;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        private static readonly string[] PARAM_SPEED = new string[] { "-s", "-spd" };
        private static readonly string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
        private static readonly string[] PARAM_FILEPATH = new string[] { "-f", "-file", "-filepath" };
        private static readonly string[] PARAM_ENQUEUE = new string[] { "-e", "-enqueue" };
        private static readonly string[] PARAM_THOUGHT = new string[] { "-t", "-thought" };

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));

            //Dialogue System Controls
            database.AddCommand("showui", new Func<string[], IEnumerator>(ShowDialogueSystem));
            database.AddCommand("hideui", new Func<string[], IEnumerator>(HideDialogueSystem));

            //Dialogue Box Controls
            database.AddCommand("showdb", new Func<string[], IEnumerator>(ShowDialogueBox));
            database.AddCommand("hidedb", new Func<string[], IEnumerator>(HideDialogueBox));

            database.AddCommand("hideAll", new Func<string[], IEnumerator>(HideAll));
            database.AddCommand("showAll", new Func<string[], IEnumerator>(ShowAll));

            database.AddCommand("load", new Action<string[]>(LoadNewDialogueFile));

            database.AddCommand("print", new Action<string[]>(PrintMessage));

            database.AddCommand("thought", new Action<string[]>(ThoughtDialogue));

            database.AddCommand("loadscene", new Func<string[], IEnumerator>(LoadScene));
        }
        private static void PrintMessage(string message)
        {
            Debug.Log(message);
        }
        private static void PrintMessage(string[] messages)
        {
            string messageToShow = "Mensagem:\n";
            foreach (string message in messages)
            {
                messageToShow += $"{message}\n";
            }
            Debug.Log(messageToShow);
        }


        private static void LoadNewDialogueFile(string[] data)
        {
            string fileName = string.Empty;
            bool enqueue = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_FILEPATH, out fileName);
            parameters.TryGetValue(PARAM_ENQUEUE, out enqueue, defaultValue: false);

            string filePath = FilePaths.GetPathToResource(FilePaths.resources_dialogueFiles, fileName);
            TextAsset file = Resources.Load<TextAsset>(filePath);

            Debug.Log(FileManager.TextFileName(file));

            if (file == null)
            {
                Debug.LogWarning($"File '{filePath}' could not be loaded from dialogue files. Please ensure it exists within the '{FilePaths.resources_dialogueFiles}' resources folder.");
                return;
            }

            List<string> lines = FileManager.ReadTextAsset(file, includeBlankLines: true);
            Conversation newConversation = new Conversation(lines);

            if (enqueue)
                DialogueSystem.instance.conversationManager.Enqueue(newConversation);
            else
                DialogueSystem.instance.conversationManager.StartConversation(newConversation);
                
        }

        private static IEnumerator Wait(string data)
        {
            if (float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
        }

        private static IEnumerator ShowDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Show(speed, immediate);
        }

        private static IEnumerator HideDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Hide(speed, immediate);
        }

        private static IEnumerator ShowDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Show(speed, immediate);
        }

        private static IEnumerator HideDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Hide(speed, immediate);
        }
        private static IEnumerator ShowAll(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.ShowAll(speed, immediate);
        }
        private static IEnumerator HideAll(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.HideAll(speed, immediate);
        }

        private static void ThoughtDialogue(string[] data)
        {
            bool isThought = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_THOUGHT, out isThought, defaultValue: false);

            DialogueSystem.instance.dialogueContainer.DialogueContainerItalic(isThought);
        }

        private static IEnumerator LoadScene(string[] data)
        {
            if (data.Length > 0)
            {
                string sceneName = data[0];
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }
    }
}