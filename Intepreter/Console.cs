namespace Intepreter;

class Program
{
    private static void Main()
    {
        var interpreter = new ExpressionInterpreter();
        var result = interpreter.Interpret("cos(1/9) + 3 ^ 3 * (5 + 5) / 5");
        Console.WriteLine(result);
    }
}