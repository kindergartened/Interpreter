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
        Console.WriteLine(postfixExpression);

        // Вычисление результата с использованием обратной польской записи
        var result = EvaluatePostfix(expression);

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
        // Обработка унарных операций (таких как sin, cos и т.д.)
        foreach (var op in _operations.Where(e => e.Value.type == OperationType.Unary))
        {
            var match = Regex.Match(postfixExpression, $@"\([^)]+\){op.Key}|{op.Key}\((\d+\.\d+?)\){op.Key}");
            while (match.Success)
            {
                var innerExpression = match.Groups[1].Value;
                var innerResult = EvaluatePostfix(innerExpression);
                var result = _operations[op.Key];
                postfixExpression = postfixExpression.Replace($"{op.Key}({innerExpression})", result.unaryMethod(innerResult).ToString());
                match = Regex.Match(postfixExpression, $@"\([^)]+\){op.Key}|\((\d+\.\d+?)\){op.Key}");
            }
        }

        // Обработка бинарных операций (таких как +, -, * и т.д.)
        var tokens = postfixExpression.Split(' ');
        var stack = new Stack<double>();
        foreach (var token in tokens)
        {
            if (_operations.TryGetValue(token, out var operation))
            {
                var rightOperand = stack.Pop();
                var leftOperand = stack.Pop();
                var result = operation.method;
                stack.Push(result(leftOperand, rightOperand));
            }
            else
            {
                stack.Push(double.Parse(token));
            }
        }
        return stack.Pop();
    }
}