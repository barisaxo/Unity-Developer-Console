using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }

        public void AddCommandToConsole()
        {
            string _addMessage = " command has been added to the console.";

            DeveloperConsole.AddCommandsToConsole(Command, this);
            Debug.Log(Name + _addMessage);
        }

        public abstract void RunCommand(string[] args);
    }

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; private set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        [Header("UI Components")]
        public Canvas consoleCanvas;
        public Text consoleText;
        public Text inputText;
        public InputField consoleInput;

        private void Awake()
        {
            if(Instance != null)
            {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();

            consoleInput.onValueChanged.AddListener(delegate 
            {
                Manage();
            });
        }

        void Manage()
        {
            string _text = consoleInput.text;
            if(_text != consoleInput.text.ToUpper())
            {
                consoleInput.text = consoleInput.text.ToUpper();
            }
        }

        private void Start()
        {
            consoleCanvas.gameObject.SetActive(false);

            string _loadingMessage = "Adding commands to the console...";
            AddMessageToConsole(_loadingMessage);

            CreateCommands();

            string _doneMessage = "Done.\n\n";
            AddMessageToConsole(_doneMessage);

            string _readyMessage = "System ready\n";
            AddMessageToConsole(_readyMessage);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            string _message = "[" + type.ToString() + "] " + logMessage;
            AddMessageToConsole(_message);
        }

        private void CreateCommands()
        {
            CommandHelp.CreateCommand();
            CommandQuit.CreateCommand();
        }

        public static void AddCommandsToConsole(string name, ConsoleCommand command)
        {
            if(!Commands.ContainsKey(name))
            {
                Commands.Add(name, command);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                consoleCanvas.gameObject.SetActive(!consoleCanvas.gameObject.activeInHierarchy);
            }

            if(Input.GetKeyUp(KeyCode.BackQuote))
            {
                if(consoleCanvas.gameObject.activeInHierarchy)
                {
                    consoleInput.Select();
                    consoleInput.ActivateInputField();
                }
                else
                {
                    consoleInput.text = string.Empty;
                }
            }

            if(consoleCanvas.gameObject.activeInHierarchy)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    string _message = inputText.text;
                    AddMessageToConsole("]" + _message);

                    consoleInput.text = string.Empty;
                    consoleInput.Select();
                    consoleInput.ActivateInputField();

                    if(_message != "")
                    {
                        ParseInput(_message);
                    }
                }
            }
        }

        public void AddMessageToConsole(string msg)
        {
            consoleText.text += msg.ToUpper() + "\n";
        }

        private void ParseInput(string input)
        {
            string[] _input = input.Trim(' ').Split(null);

            if (_input.Length == 0 || _input == null)
            {
                Debug.LogWarning("Command not recognized!");
                return;
            }

            string _cmd = _input[0];

            if (!Commands.ContainsKey(_cmd))
            {
                Debug.LogWarning("Command not recognized!");
            }
            else
            {
                string[] _args = _input.Skip(1).ToArray();
                Commands[_cmd].RunCommand(_args);
            }
        }
    }
}
