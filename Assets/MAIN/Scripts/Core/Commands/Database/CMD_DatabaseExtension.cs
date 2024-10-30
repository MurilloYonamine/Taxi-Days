using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public abstract class CMD_DatabaseExtension
    {
        /*
        A base class used to extend the available
        commands in the CommandDatabase
        */
        public static void Extend(CommandDatabase database) { }
        public static CommandParameters ConvertDataToParameters(string[] data, int startingIndex = 0) => new CommandParameters(data, startingIndex);
    }
}