using System.Text;

namespace Intepreter;

class Program
{
    private static void Main()
    {
        var interpreter = new ExpressionInterpreter();
        var result = interpreter.Interpret("sin(1 / 9) + 1");
        // Console.WriteLine(string.Join("\n", result));
        // Console.WriteLine(result);
        // for (int i = 0; i < result.Count; i++)
        // {
        //     Console.WriteLine($"{result[i].Type}: {result[i].Value}");
        // }
        
    }
}