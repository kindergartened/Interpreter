using System.Text.RegularExpressions;

namespace Intepreter;

public class ExpressionInterpreter
{
    /// <summary>
    /// Словарь со всеми методами и их приоритетами
    ///     Ключ - строка метода.
    ///     Значение - класс Expression.
    /// </summary>
    private readonly IDictionary<string, Expression> _operations = new Dictionary<string, Expression>
    {
        // Binary
        {"+", new BinaryExpression(1, (a, b) => a + b, OperationType.Binary)},
        {"-", new BinaryExpression(1, (a, b) => a - b, OperationType.Binary)},
        {"*", new BinaryExpression(2, (a, b) => a * b, OperationType.Binary)},
        {"/", new BinaryExpression(2, (a, b) => a / b, OperationType.Binary)},
        {"**", new BinaryExpression(3, Math.Pow, OperationType.Binary)},
        
        // Unary
        { "sin", new UnaryExpression(4, Math.Sin, OperationType.Unary) },
        { "cos", new UnaryExpression(4, Math.Cos, OperationType.Unary) },
        { "tan", new UnaryExpression(4, Math.Tan, OperationType.Unary) },
        { "cot", new UnaryExpression(4, (a) => 1 / Math.Tan(a), OperationType.Unary) },
        
        // Logical
        { "<", new LogicalExpression<double>(0, (a, b) => a < b, OperationType.LogicalDouble) },
        { "<=", new LogicalExpression<double>(0, (a, b) => a <= b, OperationType.LogicalDouble) },
        { ">", new LogicalExpression<double>(0, (a, b) => a > b, OperationType.LogicalDouble) },
        { ">=", new LogicalExpression<double>(0, (a, b) => a >= b, OperationType.LogicalDouble) },
        { "&&", new LogicalExpression<bool>(0, (a, b) => a && b, OperationType.Logical) },
        { "||", new LogicalExpression<bool>(0, (a, b) => a || b, OperationType.Logical) },
        { "&", new LogicalExpression<bool>(0,(a, b) => a & b, OperationType.Logical) },
        { "|", new LogicalExpression<bool>(0, (a, b) => a | b, OperationType.Logical) },
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

        Console.WriteLine(postfixExpression);

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
        infixExpression = infixExpression.Replace('.', ',');

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

        var result = output[0];
        for (var i = 1; i < output.Count; i++)
        {
            result += output[i] == "," ? output[i] :
                output[i - 1] == "," ?
                    i == output.Count ? output[i] + " " :
                        output[i] :
                    i == output.Count ? " " + output[i] + " " :
                    " " + output[i];
        }

        return result;
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
                var expression = (BinaryExpression)_operations[token];
                if (expression.Method != null)
                {
                    var result = expression.Method(operand1, operand2);
                    operands.Push(result);
                }
            }
            else if (_operations[token].Type == OperationType.Unary)
            {
                // Унарные операции
                var operand = operands.Pop();
                var expression = (UnaryExpression)_operations[token];
                if (expression.Method != null)
                {
                    var result = expression.Method(operand);
                    operands.Push(result);
                }
            }
            else if (_operations[token].Type == OperationType.LogicalDouble)
            {
                // Логические операции с числами
                var operand2 = operands.Pop();
                var operand1 = operands.Pop();
                var expression = (LogicalExpression<double>)_operations[token];
                if (expression.Method != null)
                {
                    var result = expression.Method(operand1, operand2);
                    operands.Push(result ? 1 : 0);
                }
            }
            else if (_operations[token].Type == OperationType.Logical)
            {
                // Логические операции
                var operand2 = operands.Pop() != 0.0;
                var operand1 = operands.Pop() != 0.0;
                var expression = (LogicalExpression<bool>)_operations[token];
                if (expression.Method != null)
                {
                    var result = expression.Method(operand1, operand2);
                    operands.Push(result ? 1 : 0);
                }
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

            if (_operations[token].Type == OperationType.Binary)
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
            else if (_operations[token].Type == OperationType.Unary)
            {
                tokens.Add(new Token(token.ToLower(), TokenType.UnaryFunction));
            }
            else if (_operations[token].Type == OperationType.Logical || _operations[token].Type == OperationType.LogicalDouble)
            {
                tokens.Add(new Token(token.ToLower(), TokenType.LogicalFunction));
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