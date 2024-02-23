using System.Text;

namespace Intepreter;

class Program
{
    private static void Main()
    {
        var interpreter = new ExpressionInterpreter();
        var result = interpreter.Interpret("sin(1 / 9) + 1");
        Console.WriteLine(result);
        // Console.WriteLine(result);
    }
}