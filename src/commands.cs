using System.Reflection.Metadata.Ecma335;

namespace Commands
{
    public static class CommandHandler
    {
        private static readonly Dictionary<string, Action<string>> _commandMap = new()
        {
            { "exit", _ => Environment.Exit(0) },
            { "echo", parameters => Console.WriteLine(parameters) },
            { "type", parameters => TypeCommand(parameters) } 
        };


        public static void HandleCommand(string command, string parameters)
        {
            if (_commandMap.TryGetValue(command, out Action<string>? action))
                action(parameters);
            
            else
                Console.WriteLine($"{command}: command not found");    
        }

        public static (string Command, string Parameters) ParseCommand(string fullCommand)
        {
            int firstSpaceIndex = fullCommand.IndexOf(' ');
            string command;
            string parameters = "";

            if (firstSpaceIndex != -1)
            {
                command = fullCommand.Substring(0, firstSpaceIndex);
                parameters = fullCommand.Substring(firstSpaceIndex + 1);
            }
            else
            {
                command = fullCommand;
                parameters = "";
            }

            return (command, parameters);
        }

    public static void TypeCommand(string parameters)
        {
            string parameterType = "";
            
            if (_commandMap.ContainsKey(parameters))
                parameterType = "builtin";
            else
            {
                Console.WriteLine($"{parameters}: not found");
                return;
            }
            
            Console.WriteLine($"{parameters} is a shell {parameterType}");
        }
    }
}