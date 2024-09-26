using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
{
    /*
    The script will serve as an example
    for how to add future commands
    to the command system
    */

    new public static void Extend(CommandDatabase database)
    {
        // Add command with no parameters
        database.AddCommand("print", new Action(PrintDefaultMessage));
    }
    private static void PrintDefaultMessage()
    {
        Debug.Log("Imprimindo uma mensagem para o console.");
    }
}
