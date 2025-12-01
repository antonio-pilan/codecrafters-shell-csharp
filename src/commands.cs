using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using Utils;

namespace Commands
{
    public static class CommandHandler
    {
        // Command dictionary: ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static readonly Dictionary<string, Action<string[]>> _commandMap = new()
        {
            { "exit", _ => Environment.Exit(0) },
            { "echo", args => Console.WriteLine(string.Join(" ", args)) },
            { "type", args => TypeCommand(args) } ,
            { "pwd", parameters => Console.WriteLine(Directory.GetCurrentDirectory()) },
            { "cd", args => cdCommand(args) },
            { "cat", args => CatCommand(args) }
        };

        // Handlers implementations: //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void HandleCommand(string command, string[] parameters)
        {
            if (_commandMap.TryGetValue(command, out Action<string[]>? action))
            {
                action(parameters);
                return;
            }
                
            else if (PathVariables.GetPath(command) is string executablePath)
            {
                try 
                {
                    var process = new System.Diagnostics.Process();

                    process.StartInfo.FileName = executablePath;
                    foreach (var param in parameters)
                        {
                            process.StartInfo.ArgumentList.Add(param);
                        }
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

        public static (string Command, string[] Parameters) ParseCommand(string fullCommand)
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
                return ("", Array.Empty<string>());
            

            // First token is the command
            string command = tokens[0];
            string[] parameters = tokens.Skip(1).ToArray();

            return (command, parameters);
        }

            // Commands implementations: ////////////////////////////////////////////////////////////////////////////////////////
        public static void TypeCommand(string[] parameters)
        {
            string parameterType = "";
            string? executable = null;
            string parameter = "";
            int numberOfParameters = parameters.Length;

            if (numberOfParameters == 1)
            {
                parameter = parameters[0];
                executable = PathVariables.GetPath(parameter);
            }
            else
            {
                string errorString = String.Join(" ", parameters);
                Console.WriteLine($"{errorString}: not found");
                return;
            }
            
            if (_commandMap.ContainsKey(parameter)) 
                parameterType = "builtin";
            else if (executable != null)
            {
                Console.WriteLine($"{parameter} is {executable}");
                return;
            }
            else
            {
                Console.WriteLine($"{parameter}: not found");
                return;
            }
            
            Console.WriteLine($"{parameter} is a shell {parameterType}");
        }

        public static void cdCommand(string[] parameters)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string? parameter = null;

            if (parameters.Length == 1)
            {
                parameter = parameters[0];
            }
            else return;

            switch (parameter)
            {
                case "./" or ".":
                    var parentDirectory = Directory.GetParent(currentDirectory);
                    if (parentDirectory != null)
                    {
                        Directory.SetCurrentDirectory(parentDirectory.FullName);
                    }
                    break;

                case "../" or "..":
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
                    DirectoryManipulation.FromPath(parameter);
                    break;
            }
        }

        public static void CatCommand(string[] parameters)
        {
            if (parameters.Length == 0)
            {
                Console.WriteLine("cat: file operand expected");
                return;
            }

            foreach (var filePath in parameters)
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"cat: {filePath}: No such file or directory");
                    continue; 
                }

                try
                {
                    string content = File.ReadAllText(filePath);
                    Console.WriteLine(content);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"cat: {filePath}: Permission denied");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"cat: read error: {ex.Message}");
                }
            }
        }







    } 
}