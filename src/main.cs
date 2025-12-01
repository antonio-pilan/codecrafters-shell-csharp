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
            
            if (param.Contains('\'') )
            {
                //param is raw string
                param = param.Replace("'", "");
                param = $"{param}";
            }
            cmd = cmd.ToLower();
            
            CommandHandler.HandleCommand(cmd, param);
        }
    }
}
