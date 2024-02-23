using System.Text;

namespace Intepreter;

class Program
{
    private static void Main()
    {
        var interpreter = new ExpressionInterpreter();
        var result = interpreter.Interpret("55 + sin(2 + 5) + 3.5");
        // Console.WriteLine(string.Join("\n", result));
        // Console.WriteLine(result);
        // for (int i = 0; i < result.Count; i++)
        // {
        //     Console.WriteLine($"{result[i].Type}: {result[i].Value}");
        // }
        Console.WriteLine(result);
    }
}