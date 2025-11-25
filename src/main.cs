using System.Runtime.CompilerServices;

class Program
{
    static void Main()
    {
        Console.Write("$ ");
        string? command = Console.ReadLine();

        if (command != null)
        {
            Console.WriteLine($"{command}: command not found");
        }
    }
}
