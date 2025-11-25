using System.Runtime.CompilerServices;

class Program
{
    static void Main()
    {   
        bool isRunning = true;
        while (isRunning)
        {
            Console.Write("$ ");
            string? command = Console.ReadLine();

            if (command != null)
            {
                switch (command)
                {
                    case "exit":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine($"{command}: command not found");
                        continue;
                }
                
            }
        }
    }
}
