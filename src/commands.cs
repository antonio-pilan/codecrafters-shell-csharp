namespace Commands
{
    public static class CommandHandler
    {
        public static void HandleCommand(string command, string parameters)
        {
            switch (command)
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "echo":
                    Console.WriteLine(parameters);
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;
            }
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
    }
}