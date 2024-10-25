using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using System.Linq;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_Characters : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("hide", new Func<string[], IEnumerator>(HideAll));
        }
        public static IEnumerator ShowAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false;

            foreach (string characterName in data)
            {
                Character character = CharacterManager.instance.GetCharacter(characterName, createIfDoesNotExist: false);

                if (character != null) characters.Add(character);
            }
            if (characters.Count == 0) yield break;

            // Convert the data array to a paramter cantainer
            CommandParameters parameters = new CommandParameters(data);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            // Call the logic on all the characters
            foreach (Character character in characters)
            {
                if (immediate) character.isVisible = true;
                else character.Show();
            }

            if (!immediate)
            {
                while (characters.Any(c => c.isRevealing)) yield return null;
            }
        }
        public static IEnumerator HideAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false;

            foreach (string characterName in data)
            {
                Character character = CharacterManager.instance.GetCharacter(characterName, createIfDoesNotExist: false);

                if (character != null) characters.Add(character);
            }
            if (characters.Count == 0) yield break;

            // Convert the data array to a paramter cantainer
            CommandParameters parameters = new CommandParameters(data);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            // Call the logic on all the characters
            foreach (Character character in characters)
            {
                if (immediate) character.isVisible = false;
                else character.Hide();
            }

            if (!immediate)
            {
                while (characters.Any(c => c.isHiding)) yield return null;
            }
        }
    }
}