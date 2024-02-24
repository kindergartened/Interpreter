using System.Text.RegularExpressions;

namespace Intepreter;

public class ExpressionInterpreter
{
    /// <summary>
    /// Словарь со всеми операциями и их приоритетами
    ///     Ключ - строка метода.
    ///     Значение - объект, наследник класса Expression.
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
        { "sinh", new UnaryExpression(4, Math.Sinh, OperationType.Unary) },
        { "cosh", new UnaryExpression(4, Math.Cosh, OperationType.Unary) },
        { "tanh", new UnaryExpression(4, Math.Tanh, OperationType.Unary) },
        { "e", new UnaryExpression(4, Math.Exp, OperationType.Unary) },
        { "log", new UnaryExpression(4, Math.Log, OperationType.Unary) },
        
        // Logical
        { "<", new LogicalExpression<double>(-3, (a, b) => a < b, OperationType.LogicalDouble) },
        { "<=", new LogicalExpression<double>(-4, (a, b) => a <= b, OperationType.LogicalDouble) },
        { ">", new LogicalExpression<double>(-3, (a, b) => a > b, OperationType.LogicalDouble) },
        { ">=", new LogicalExpression<double>(-4, (a, b) => a >= b, OperationType.LogicalDouble) },
        { "&&", new LogicalExpression<bool>(-1, (a, b) => a && b, OperationType.Logical) },
        { "||", new LogicalExpression<bool>(-2, (a, b) => a || b, OperationType.Logical) },
        { "&", new LogicalExpression<bool>(-1,(a, b) => a & b, OperationType.Logical) },
        { "|", new LogicalExpression<bool>(-2, (a, b) => a | b, OperationType.Logical) },
        { "==", new LogicalExpression<bool>(0, (a, b) => a == b, OperationType.Logical) },
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
        var postfixExpression = ConvertToPostfix(expression);

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
                continue;

            if (_operations.TryGetValue(token, out var operation))
            {
                while (stack.Count > 0 && _operations.ContainsKey(stack.Peek()) && _operations[stack.Peek()].Priority >= operation.Priority)
                    output.Add(stack.Pop());

                stack.Push(token);
            }
            else if (token == "(")
                stack.Push(token);
            else if (token == ")")
            {
                string top;
                while ((top = stack.Pop()) != "(")
                {
                    output.Add(top);
                }
            }
            else
                output.Add(token);
        }

        while (stack.Count > 0)
            output.Add(stack.Pop());

        var result = output[0];
        for (var i = 1; i < output.Count; i++)
            result += output[i] == "," ? output[i] :
                output[i - 1] == "," ?
                    i == output.Count ? output[i] + " " :
                        output[i] :
                    i == output.Count ? " " + output[i] + " " :
                    " " + output[i];

        return result;
    }
    
    /// <summary>
    /// Вычисление выражения из обратной польской записи
    /// </summary>
    /// <param name="postfixExpression">Строчка обратной польской записи</param>
    /// <returns>Результат выражения в double</returns>
    public double EvaluatePostfix(string postfixExpression)
    {
        var operands = new Stack<double>();
        var tokens = postfixExpression.Split(' ');

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out var number))
                operands.Push(number);
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
                    operands.Push(expression.Method(operand));
            }
            else if (_operations[token].Type == OperationType.LogicalDouble)
            {
                // Логические операции с числами
                var operand2 = operands.Pop();
                var operand1 = operands.Pop();
                var expression = (LogicalExpression<double>)_operations[token];
                if (expression.Method != null)
                    operands.Push(expression.Method(operand1, operand2) ? 1 : 0);
            }
            else if (_operations[token].Type == OperationType.Logical)
            {
                // Логические операции
                var operand2 = operands.Pop() != 0.0;
                var operand1 = operands.Pop() != 0.0;
                var expression = (LogicalExpression<bool>)_operations[token];
                if (expression.Method != null)
                    operands.Push(expression.Method(operand1, operand2) ? 1 : 0);
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
        expression = ConvertToPostfix(expression);

        foreach (var token in expression.Split(" "))
        {
            if (string.IsNullOrWhiteSpace(token))
                continue;
            else if (token == "(")
                tokens.Add(new Token(token, TokenType.LeftParenthesis));
            else if (token == ")")
                tokens.Add(new Token(token, TokenType.RightParenthesis));
            else if (double.TryParse(token, out _))
                tokens.Add(new Token(token, TokenType.Number));
            else if (_operations[token].Type == OperationType.Binary)
                tokens.Add(new Token(token, TokenType.Operator));
            else if (_operations[token].Type == OperationType.Unary)
                tokens.Add(new Token(token.ToLower(), TokenType.UnaryFunction));
            else if (_operations[token].Type == OperationType.Logical || _operations[token].Type == OperationType.LogicalDouble)
                tokens.Add(new Token(token.ToLower(), TokenType.LogicalFunction));
            else
                tokens.Add(new Token(token, TokenType.Unknown));
        }

        return tokens;
    }

    /// <summary>
    /// Построение дерева лексического анализа из постфиксной записи выражения
    ///     - переводит выражение в постфиксную запись
    ///     - из переведенного выражения делает дерево 
    /// </summary>
    /// <param name="infix">Строка в инфиксной форме</param>
    /// <returns>Строчное представление дерева</returns>
    public string BuildTree(string infix)
    {
        return BuildExpressionTree(ConvertToPostfix(infix)).ToString();
    }
    
    /// <summary>
    /// Построение дерева из постфиксной записи выражения
    /// </summary>
    /// <param name="postfixExpression">Строка в постфиксной форме</param>
    /// <returns>ExpressionNode дерево</returns>
    private ExpressionNode BuildExpressionTree(string postfixExpression)
    {
        var stack = new Stack<ExpressionNode>();

        foreach (var token in postfixExpression.Split(' '))
        {
            if (_operations.ContainsKey(token))
            {
                var operationNode = new ExpressionNode(token);
                operationNode.Right = stack.Pop();
                if (stack.Count != 0)
                    operationNode.Left = stack.Pop();
                stack.Push(operationNode);
            }
            else
                stack.Push(new ExpressionNode(token));
        }

        return stack.Pop();
    }
}