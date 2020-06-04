using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandHelp : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CommandHelp()
        {
            Name = "HELP";
            Command = "HELP";
            Description = "List of all available commands.";
            Help = "Use the help command with no arguments for a list of all commands.";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] args)
        {
            if(args.Length > 0)
            {
                if(DeveloperConsole.Commands.ContainsKey(args[0]))
                {
                    DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsole.Commands[args[0]].Help);
                }
                else
                {
                   Debug.LogWarning("Command not recognized!");
                }
                return;
            }

            string _message = this.Description;
            string _commandHeader = "Command";
            int _columnSize = 18;
            string _descriptionHeader = "Description";
            string _info = "For more information on a specific command.";
            string _usage = this.Name + " command";
            
            _message += "\n\nUse\n" + _usage;
            _message += RepeatingChar(' ', _columnSize - _usage.Length);
            _message += _info + "\n\n";

            _message += _commandHeader;
            _message += RepeatingChar(' ', _columnSize - _commandHeader.Length);
            _message += _descriptionHeader + "\n";

            _message += RepeatingChar('-', _commandHeader.Length);
            _message += RepeatingChar(' ', _columnSize - _commandHeader.Length);
            _message += RepeatingChar('-', _descriptionHeader.Length) + "\n";

            foreach(KeyValuePair<string, ConsoleCommand> command in DeveloperConsole.Commands)
            {
                string _commandName = command.Value.Name; 
                if (_commandName.Length >= _columnSize)
                {
                    _commandName = command.Value.Name.Substring(0, _columnSize - 4);
                    _commandName += "...";
                }
                _message += _commandName;
                _message += RepeatingChar(' ', _columnSize - _commandName.Length);
                _message += command.Value.Description + "\n";
            }

            DeveloperConsole.Instance.AddMessageToConsole(_message);
        }

        private string RepeatingChar(char c, int n)
        {
            return new string(c, n);
        }

        public static CommandHelp CreateCommand()
        {
            return new CommandHelp();
        }
    }
}