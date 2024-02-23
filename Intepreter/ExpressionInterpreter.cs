using System.Text.RegularExpressions;

namespace Intepreter;

class ExpressionInterpreter
{
    private readonly IDictionary<string, Expression> _precedenceMap = new Dictionary<string, Expression>
    {
        {"+", new Expression(1, (a, b) => a + b)},
        {"-", new Expression(2, (a, b) => a - b)},
        {"*", new Expression(3, (a, b) => a * b)},
        {"/", new Expression(4, (a, b) => a / b)},
        {"^", new Expression(5, (a, b) => Math.Pow(a, b))},
    };
    private readonly IDictionary<string, Func<double, double>> _unaryOperations = new Dictionary<string, Func<double, double>>
    {
        { "sin", Math.Sin },
        { "cos", Math.Cos },
        { "tan", Math.Tan },
    };

    public double Interpret(string expression)
    {
        // Преобразование входного выражения в обратную польскую запись
        var postfixExpression = ConvertToPostfix(expression);

        // Вычисление результата с использованием обратной польской записи
        var result = EvaluatePostfix(postfixExpression);

        return result;
    }

    private string ConvertToPostfix(string infixExpression)
    {
        var operators = new Stack<string>();
        var output = new Queue<string>();

        var tokens = Regex.Split(infixExpression, @"(\d+\.\d+|\d+|[+\-*/^()]|[a-zA-Z]+)");

        foreach (var token in tokens.Where(t => !string.IsNullOrWhiteSpace(t)))
        {
            if (double.TryParse(token, out var number) || char.IsLetter(token[0]))
            {
                output.Enqueue(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Count > 0 && operators.Peek() != "(")
                {
                    output.Enqueue(operators.Pop());
                }
                operators.Pop(); // Удаляем открывающую скобку
            }
            else if (_unaryOperations.ContainsKey(token))
            {
                operators.Push(token); // Добавляем унарную операцию в стек
            }
            else if (_precedenceMap.ContainsKey(token))
            {
                while (operators.Count > 0 && GetPrecedence(operators.Peek()) >= GetPrecedence(token))
                {
                    output.Enqueue(operators.Pop());
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            output.Enqueue(operators.Pop());
        }

        return string.Join(" ", output.ToArray());
    }

    private int GetPrecedence(string operatorToken)
    {
        return _precedenceMap.TryGetValue(operatorToken, out var value) ? value.priority : 0;
    }

    private double EvaluatePostfix(string postfixExpression)
    {
        var operands = new Stack<double>();
        var tokens = postfixExpression.Split(' ');

        Console.WriteLine(postfixExpression);
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
                else if (_precedenceMap.ContainsKey(token))
                {
                    // Бинарные операции
                    var operand2 = operands.Pop();
                    var operand1 = operands.Pop();
                    var result = _precedenceMap[token].method(operand1, operand2);
                    operands.Push(result);
                }
                else if (_unaryOperations.ContainsKey(token))
                {
                    // Унарные операции
                    var operand = operands.Pop();
                    var result = _unaryOperations[token](operand);
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