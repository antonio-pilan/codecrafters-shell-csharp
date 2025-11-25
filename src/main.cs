using System.Runtime.CompilerServices;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            string? command = Console.ReadLine();

            if (command != null)
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
    }
}
