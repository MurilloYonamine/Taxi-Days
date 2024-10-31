using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine.Events;
using CHARACTERS;

namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        /*
        System responsible for validating
        and executing special commands
        */
        private const char SUB_COMMAND_IDENTIFIER = '.';
        public const string DATABASE_CHARACTER_BASE = "characters";
        public const string DATABASE_CHARACTER_SPRITE = "characters_sprite";
        public static CommandManager instance { get; private set; }
        private CommandDatabase database;
        private Dictionary<string, CommandDatabase> subDatabase = new Dictionary<string, CommandDatabase>();
        private List<CommandProcess> activeProcesses = new List<CommandProcess>();
        private CommandProcess topProcess => activeProcesses.Last();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                database = new CommandDatabase();

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { database });
                }
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        public CoroutineWrapper Execute(string commandName, params string[] args)
        {
            if (commandName.Contains(SUB_COMMAND_IDENTIFIER)) return ExecuteSubCommand(commandName, args);

            Delegate command = database.GetCommand(commandName);

            if (command == null) return null;

            return StartProcess(commandName, command, args);

        }
        private CoroutineWrapper ExecuteSubCommand(string commandName, string[] args)
        {
            string[] parts = commandName.Split(SUB_COMMAND_IDENTIFIER);
            string databaseName = string.Join(SUB_COMMAND_IDENTIFIER, parts.Take(parts.Length - 1));
            string subCommandName = parts.Last();

            if (subDatabase.ContainsKey(databaseName))
            {
                Delegate command = subDatabase[databaseName].GetCommand(subCommandName);
                if (command != null) return StartProcess(subCommandName, command, args);
                else
                {
                    Debug.LogError($"Nenhum comando chamado: {subCommandName} foi encontrado no banco de dados: {databaseName}");
                    return null;
                }
            }
            string characterName = databaseName;
            if (CharacterManager.instance.HasCharacter(characterName))
            {
                List<string> newArgs = new List<string>(args);
                newArgs.Insert(0, characterName);
                args = newArgs.ToArray();

                return ExecuteCharacterCommand(subCommandName, args);
            }
            Debug.LogError($"Nenhum banco de dados chamado: {databaseName} foi encontrado. Commando: {commandName} não pode ser executado.");
            return null;
        }
        private CoroutineWrapper ExecuteCharacterCommand(string commandName, params string[] args)
        {
            Delegate command = null;

            CommandDatabase db = subDatabase[DATABASE_CHARACTER_BASE];

            if (db.HasCommand(commandName))
            {
                command = db.GetCommand(commandName);
                return StartProcess(commandName, command, args);
            }
            CharacterConfigData characterConfigData = CharacterManager.instance.GetCharacterConfig(args[0]);

            if (characterConfigData.characterType == Character.CharacterType.Sprite ||
                characterConfigData.characterType == Character.CharacterType.SpriteSheet)
            {
                db = subDatabase[DATABASE_CHARACTER_SPRITE];
            }
            command = db.GetCommand(commandName);

            if (command != null) return StartProcess(commandName, command, args);

            Debug.LogError($"O gerenciador de comandos foi incapaz de executar o comando: {commandName} para o personagem: {args[0]}. O nome do personagem ou comando talvez esteja invalido.");
            return null;
        }
        private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
        {
            System.Guid processID = System.Guid.NewGuid();

            CommandProcess cmd = new CommandProcess(processID, commandName, command, null, args, null);
            activeProcesses.Add(cmd);

            Coroutine co = StartCoroutine(RunningProcess(cmd));

            cmd.runningProcess = new CoroutineWrapper(this, co);

            return cmd.runningProcess;
        }
        public void StopCurrentProcess()
        {
            if (topProcess != null) KillProcess(topProcess);
        }
        private IEnumerator RunningProcess(CommandProcess process)
        {
            yield return WaitingForProcessToComplete(process.command, process.args);

            KillProcess(process);
        }
        public void KillProcess(CommandProcess cmd)
        {
            activeProcesses.Remove(cmd);

            if (cmd.runningProcess != null && !cmd.runningProcess.IsDone) cmd.runningProcess.Stop();

            cmd.onTerminateAction?.Invoke();
        }
        public void StopAllProcesses()
        {
            foreach (var cmd in activeProcesses)
            {
                if (cmd.runningProcess != null && !cmd.runningProcess.IsDone)
                {
                    cmd.runningProcess.Stop();
                }
                cmd.onTerminateAction?.Invoke();
            }
            activeProcesses.Clear();
        }
        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
            {
                command.DynamicInvoke();
            }
            else if (command is Action<string>)
            {
                command.DynamicInvoke(args.Length == 0 ? string.Empty : args[0]);
            }
            else if (command is Action<string[]>)
            {
                command.DynamicInvoke((object)args);
            }
            else if (command is Func<IEnumerator>)
            {
                yield return ((Func<IEnumerator>)command)();
            }
            else if (command is Func<string, IEnumerator>)
            {
                yield return ((Func<string, IEnumerator>)command)(args.Length == 0 ? string.Empty : args[0]);
            }
            else if (command is Func<string[], IEnumerator>)
            {
                yield return ((Func<string[], IEnumerator>)command)(args);
            }
        }
        public void AddTerminationActionToCurrentProcess(UnityAction action)
        {
            CommandProcess process = topProcess;

            if (process == null) return;

            process.onTerminateAction = new UnityEvent();
            process.onTerminateAction.AddListener(action);
        }
        public CommandDatabase CreateSubDatabase(string name)
        {
            name = name.ToLower();

            if(subDatabase.TryGetValue(name, out CommandDatabase db))
            {
                Debug.LogWarning($"Um banco de dados chamado: {name} já existe!");
                return db;
            }
            CommandDatabase newDatabase = new CommandDatabase();
            subDatabase.Add(name, newDatabase);
            
            return newDatabase;
        }
    }
}