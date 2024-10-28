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
        private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate" };
        private static string[] PARAM_ENABLED => new string[] { "-e", "-enabled" };
        private static string[] PARAM_SPEED => new string[] { "-spd", "-speed" };
        private static string[] PARAM_SMOOTH => new string[] { "-sm", "-smooth" };
        private static string PARAM_XPOS => "-x";
        private static string PARAM_YPOS => "-y";

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
            database.AddCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
            database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("hide", new Func<string[], IEnumerator>(HideAll));
        }
        public static void CreateCharacter(string[] data)
        {
            string characterName = data[0];
            bool enable = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_ENABLED, out enable, defaultValue: false);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            Character character = CharacterManager.instance.CreateCharacter(characterName);

            if (!enable) return;

            if (immediate) character.isVisible = true;
            else character.Show();
        }
        public static IEnumerator MoveCharacter(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.instance.GetCharacter(characterName);

            if (character == null) yield break;

            float x = 0, y = 0;
            float speed = 1;
            bool smooth = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_XPOS, out x);
            parameters.TryGetValue(PARAM_YPOS, out y);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1);
            parameters.TryGetValue(PARAM_SMOOTH, out smooth, defaultValue: false);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
            
            Vector2 position = new Vector2(x, y);

            if(immediate) character.SetPosition(position);
            else yield return character.MoveToPosition(position, speed, smooth);
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

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

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

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

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