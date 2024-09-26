using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDatabase
{
    /*
    A database of all comands that are
    available for the CommandManager to use.
    */
    private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();
    public bool HasCommand(string commandName) => database.ContainsKey(commandName);

    public void AddCommand(string commandName, Delegate command)
    {
        if (!database.ContainsKey(commandName)) database.Add(commandName, command);
        else Debug.LogError($"Comando: '{commandName}' já existe no banco de dados!");
    }
    public Delegate GetCommand(string commandName)
    {
        if (!database.ContainsKey(commandName))
        {
            Debug.LogError($"Comando: '{commandName}' não foi encontrado no banco de dados!");
            return null;
        }
        return database[commandName];
    }
}
