using System.Globalization;
using System.Text.RegularExpressions;

namespace Intepreter;

class ExpressionInterpreter
{
    private readonly IDictionary<string, Expression> _operations = new Dictionary<string, Expression>
    {
        {"+", new Expression(1, (a, b) => a + b, null, OperationType.Binary)},
        {"-", new Expression(1, (a, b) => a - b, null, OperationType.Binary)},
        {"*", new Expression(2, (a, b) => a * b, null, OperationType.Binary)},
        {"/", new Expression(2, (a, b) => a / b, null, OperationType.Binary)},
        {"^", new Expression(3, (a, b) => Math.Pow(a, b), null, OperationType.Binary)},
        { "sin", new Expression(4, null, Math.Sin, OperationType.Unary) },
        { "cos", new Expression(4, null, Math.Cos, OperationType.Unary) },
        { "tan", new Expression(4, null, Math.Tan, OperationType.Unary) },
    };
    
    public double Interpret(string expression)
    {
        // Преобразование входного выражения в обратную польскую запись
        var postfixExpression = ConvertToPostfix(expression);

        // Вычисление результата с использованием обратной польской записи
        var result = EvaluatePostfix(postfixExpression);

        return result;
    }
    
    public string ConvertToPostfix(string infixExpression)
    {
        var output = new List<string>();
        var stack = new Stack<string>();

        foreach (var token in Regex.Split(infixExpression, @"(\s+|\b)"))
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            if (_operations.ContainsKey(token))
            {
                while (stack.Count > 0 && _operations.ContainsKey(stack.Peek()) && _operations[stack.Peek()].priority >= _operations[token].priority)
                {
                    output.Add(stack.Pop());
                }

                stack.Push(token);
            }
            else if (token == "(")
            {
                stack.Push(token);
            }
            else if (token == ")")
            {
                string top;
                while ((top = stack.Pop()) != "(")
                {
                    output.Add(top);
                }
            }
            else
            {
                output.Add(token);
            }
        }

        while (stack.Count > 0)
        {
            output.Add(stack.Pop());
        }

        return string.Join(" ", output);
    }
    
    private double EvaluatePostfix(string postfixExpression)
    {
        var operands = new Stack<double>();
        var tokens = postfixExpression.Split(' ');

        try
        {
            foreach (var token in tokens)
            {
                if (double.TryParse(token, out var number))
                {
                    operands.Push(number);
                }
                else if (char.IsLetter(token[0]) && token.Length == 1)
                {
                    // Обработка переменных, если необходимо
                    // В данном коде переменные просто игнорируются
                }
                else if (_operations[token].type == OperationType.Binary)
                {
                    // Бинарные операции
                    var operand2 = operands.Pop();
                    var operand1 = operands.Pop();
                    var result = _operations[token].method(operand1, operand2);
                    operands.Push(result);
                }
                else if (_operations[token].type == OperationType.Unary)
                {
                    // Унарные операции
                    var operand = operands.Pop();
                    var result = _operations[token].unaryMethod(operand);
                    operands.Push(result);
                }
            }
        }
        catch (InvalidOperationException)
        {
            // Обработка случая, когда стек оказывается пустым
            Console.WriteLine("Ошибка: неверное выражение.");
            return double.NaN; // или другое значение, которое указывает на ошибку
        }

        return operands.Pop();
    }
}