using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using UnityEngine;

namespace TESTING
{
    public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
    {
        /*
        The script will serve as an example
        for how to add future commands
        to the command system
        */

        new public static void Extend(CommandDatabase database)
        {
            // Add_Action command with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLine));

            // Add lambda with no parameters
            database.AddCommand("lambda", new Action(() => Debug.Log("Imprimindo uma mensagem com lambda.")));
            database.AddCommand("lambda_1p", new Action<string>((arg) => Debug.Log($"Mensagem do usuário: {arg}")));
            database.AddCommand("lambda_mp", new Action<string[]>((args) => Debug.Log(string.Join(", ", args))));

            // Add corutine with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimplesProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

            // demo
            database.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));
        }
        private static void PrintDefaultMessage()
        {
            Debug.Log("Imprimindo uma mensagem para o console.");
        }
        private static void PrintUserMessage(string message)
        {
            Debug.Log($"Mensagem do usuário: {message}");
        }
        private static void PrintLine(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
        }
        private static IEnumerator SimplesProcess()
        {
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log($"Processando... {i}");
                yield return new WaitForSeconds(1);
            }
        }
        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 1; i <= num; i++)
                {
                    Debug.Log($"Processando... {i}");
                    yield return new WaitForSeconds(1);
                }
            }
        }
        private static IEnumerator MultiLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Processando... {line}");
                yield return new WaitForSeconds(0.5f);
            }
        }
        private static IEnumerator MoveCharacter(string direction)
        {
            bool left = direction.ToLower() == "left";

            // Get the variables I need. This would be defined somewhere else.
            Transform character = GameObject.FindGameObjectWithTag("Test").transform;
            float moveSpeed = 15;

            // Calculate the target position for the image
            float targetX = left ? -8 : 8;

            // Calculate the current position of the image
            float currentX = character.position.x;

            // Move the image gradually towards the target position
            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector2(currentX, character.position.y);
                yield return null;
            }
        }
    }
}