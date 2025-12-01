using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using Utils;

namespace Commands
{
    public static class CommandHandler
    {
        // Command dictionary: ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static readonly Dictionary<string, Action<string>> _commandMap = new()
        {
            { "exit", _ => Environment.Exit(0) },
            { "echo", parameters => Console.WriteLine(parameters) },
            { "type", parameters => TypeCommand(parameters) } ,
            { "pwd", parameters => Console.WriteLine(Directory.GetCurrentDirectory()) },
            { "cd", parameters => cdCommand(parameters) }
        };

        // Handlers implementations: //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void HandleCommand(string command, string parameters)
        {
            if (_commandMap.TryGetValue(command, out Action<string>? action))
                action(parameters);
            else if (PathVariables.GetPath(command) is string executablePath)
            {
                var parametersList = parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                try 
                {
                    var process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = parameters;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.RedirectStandardError = false;
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command '{command}': {ex.Message}");
                }
            }
            else
                Console.WriteLine($"{command}: command not found");    
        }

    public static (string Command, string Parameters) ParseCommand(string fullCommand)
    {
        var tokens = new List<string>();
        var currentToken = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in fullCommand)
        {
            if (c == '\'')
            {   
                // If we hit a quote, we toggle the inQuotes flag for handling spaces inside quotes
                inQuotes = !inQuotes;
            }
            else if (char.IsWhiteSpace(c) && !inQuotes)
            {   
                // Adds current token to the list and resets the buffer
                if (currentToken.Length > 0)
                {
                    tokens.Add(currentToken.ToString());
                    currentToken.Clear();
                }
            }
            else
            {
                currentToken.Append(c);
            }
        }

        // Add the last token if there's any
        if (currentToken.Length > 0)
            tokens.Add(currentToken.ToString());
        
        // Return Handling //////////////////////
        if (tokens.Count == 0)
            return ("", "");
        

        // First token is the command
        string command = tokens[0];

        // Rebuild parameters string
        string parameters = "";
        
        if (tokens.Count > 1)
            parameters = string.Join(" ", tokens.Skip(1));

        return (command, parameters);
    }

        // Commands implementations: ////////////////////////////////////////////////////////////////////////////////////////
        public static void TypeCommand(string parameters)
        {
            string parameterType = "";
            string? executable = PathVariables.GetPath(parameters);
            
            if (_commandMap.ContainsKey(parameters)) parameterType = "builtin";
            else if (executable != null)
            {
                Console.WriteLine($"{parameters} is {executable}");
                return;
            }
            else
            {
                Console.WriteLine($"{parameters}: not found");
                return;
            }
            
            Console.WriteLine($"{parameters} is a shell {parameterType}");
        }

        public static void cdCommand(string parameters)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            switch (parameters)
            {
                case "./":
                    var parentDirectory = Directory.GetParent(currentDirectory);
                    if (parentDirectory != null)
                    {
                        Directory.SetCurrentDirectory(parentDirectory.FullName);
                    }
                    break;

                case "../":
                    int indexer = 0;
                    
                    while (indexer <2)
                    {
                        currentDirectory = Directory.GetCurrentDirectory();
                        parentDirectory = Directory.GetParent(currentDirectory);
                        if (parentDirectory != null)
                        {
                            Directory.SetCurrentDirectory(parentDirectory.FullName);
                        } 
                        indexer += 1;
                    }
                        break;
                
                case "~":
                    var homeDirectory = DirectoryManipulation.GetHomeDirectory();
                    Directory.SetCurrentDirectory(homeDirectory);
                    break;
                default:
                    DirectoryManipulation.FromPath(parameters);
                    break;
            }
        }
    }
}