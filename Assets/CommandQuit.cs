using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandQuit : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CommandQuit()
        {
            Name = "QUIT";
            Command = "QUIT";
            Description = "Quits the program.";
            Help = "Use the quit command with no arguments to force the program to quit.";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] args)
        {
            if(args.Length > 0)
            {
                DeveloperConsole.Instance.AddMessageToConsole(this.Help);
                return;
            }

            if (Application.isEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }

        public static CommandQuit CreateCommand()
        {
            return new CommandQuit();
        }
    }
}