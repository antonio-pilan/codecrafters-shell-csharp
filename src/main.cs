using System.Runtime.CompilerServices;
using Commands;

class Program
{
    static void Main()
    {   
        bool isRunning = true;
        while (isRunning)
        {
            Console.Write("$ ");
            string? fullCommand = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fullCommand)) continue;

            var (cmd, param) = CommandHandler.ParseCommand(fullCommand);
            CommandHandler.HandleCommand(cmd, param);
        }
    }
}
