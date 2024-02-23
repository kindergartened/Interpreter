using System.Globalization;
using System.Text.RegularExpressions;

namespace Intepreter;

class ExpressionInterpreter
{
    /// <summary>
    /// Словарь со всеми методами и их приоритетами
    ///     Ключ - строка метода.
    ///     Значение - класс Expression.
    /// </summary>
    private readonly IDictionary<string, Expression> _operations = new Dictionary<string, Expression>
    {
        {"+", new Expression(1, (a, b) => a + b, null, OperationType.Binary)},
        {"-", new Expression(1, (a, b) => a - b, null, OperationType.Binary)},
        {"*", new Expression(2, (a, b) => a * b, null, OperationType.Binary)},
        {"/", new Expression(2, (a, b) => a / b, null, OperationType.Binary)},
        {"^", new Expression(3, Math.Pow, null, OperationType.Binary)},
        { "sin", new Expression(4, null, Math.Sin, OperationType.Unary) },
        { "cos", new Expression(4, null, Math.Cos, OperationType.Unary) },
        { "tan", new Expression(4, null, Math.Tan, OperationType.Unary) },
    };
    
    /// <summary>
    /// Общий метод вычисления из инфиксной записи.
    ///     - Переводит в обратную польскую запись.
    ///     - Вычисляет значение выражения.
    /// </summary>
    /// <param name="expression">Строчка инфиксной записи</param>
    /// <returns>Значение выражения</returns>
    public double Interpret(string expression)
    {
        // Преобразование входного выражения в обратную польскую запись
        var postfixExpression = ConvertToPostfix(expression);

        // Вычисление результата с использованием обратной польской записи
        var result = EvaluatePostfix(postfixExpression);

        return result;
    }
    
    /// <summary>
    /// Переводит входную строку в обратную польскую запись
    /// </summary>
    /// <param name="infixExpression">Входная строка</param>
    /// <returns>Строка в обратно-польском представлении</returns>
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

            if (_operations.TryGetValue(token, out var operation))
            {
                while (stack.Count > 0 && _operations.ContainsKey(stack.Peek()) && _operations[stack.Peek()].Priority >= operation.Priority)
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
    
    /// <summary>
    /// Вычисление выражения из обратной польской записи
    /// </summary>
    /// <param name="postfixExpression">Строчка обратной польской записи</param>
    /// <returns>Результат выражения в double</returns>
    private double EvaluatePostfix(string postfixExpression)
    {
        var operands = new Stack<double>();
        var tokens = postfixExpression.Split(' ');

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out var number))
            {
                operands.Push(number);
            }
            else if (_operations[token].Type == OperationType.Binary)
            {
                // Бинарные операции
                var operand2 = operands.Pop();
                var operand1 = operands.Pop();
                var result = _operations[token].BinaryMethod(operand1, operand2);
                operands.Push(result);
            }
            else if (_operations[token].Type == OperationType.Unary)
            {
                // Унарные операции
                var operand = operands.Pop();
                var result = _operations[token].UnaryMethod(operand);
                operands.Push(result);
            }
        }

        return operands.Pop();
    }
    
    /// <summary>
    /// Делает лексический анализ выражения в инфиксной форме.
    /// </summary>
    /// <param name="expression">Строка в инфиксной форме</param>
    /// <returns>Список токенов</returns>
    public List<Token> Tokenize(string expression)
    {
        var tokens = new List<Token>();

        foreach (var token in Regex.Split(expression, @"(\s+|\b)"))
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            if (_operations.ContainsKey(token))
            {
                tokens.Add(new Token(token, TokenType.Operator));
            }
            else if (token == "(")
            {
                tokens.Add(new Token(token, TokenType.LeftParenthesis));
            }
            else if (token == ")")
            {
                tokens.Add(new Token(token, TokenType.RightParenthesis));
            }
            else if (_operations.ContainsKey(token.ToLower()))
            {
                tokens.Add(new Token(token.ToLower(), TokenType.Function));
            }
            else if (double.TryParse(token, out _))
            {
                tokens.Add(new Token(token, TokenType.Number));
            }
            else
            {
                // Неизвестный токен
                tokens.Add(new Token(token, TokenType.Unknown));
            }
        }

        return tokens;
    }
}