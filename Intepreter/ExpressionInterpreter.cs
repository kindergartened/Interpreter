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
    private Dictionary<string, Func<double, double>> unaryOperations = new Dictionary<string, Func<double, double>>
    {
        { "sin", Math.Sin },
        { "cos", Math.Cos },
        { "tan", Math.Tan },
        // Добавьте здесь больше операций по мере необходимости...
    };

    public double Interpret(string expression)
    {
        // Преобразование входного выражения в обратную польскую запись
        string postfixExpression = ConvertToPostfix(expression);

        // Вычисление результата с использованием обратной польской записи
        double result = EvaluatePostfix(postfixExpression);

        return result;
    }

    private string ConvertToPostfix(string infixExpression)
    {
        Stack<char> operators = new Stack<char>();
        Queue<char> output = new Queue<char>();

        foreach (char token in infixExpression)
        {
            if (char.IsDigit(token) || char.IsLetter(token))
            {
                output.Enqueue(token);
            }
            else if (token == '(')
            {
                operators.Push(token);
            }
            else if (token == ')')
            {
                while (operators.Count > 0 && operators.Peek() != '(')
                {
                    output.Enqueue(operators.Pop());
                }
                operators.Pop(); // Удаляем открывающую скобку
            }
            else if (unaryOperations.ContainsKey(token.ToString()))
            {
                output.Enqueue(token); // Добавляем унарную операцию в вывод
            }
            else if (_precedenceMap.ContainsKey(token.ToString()))
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

        return new string(output.ToArray());
    }

    private int GetPrecedence(char operatorToken)
    {
        string operatorStr = operatorToken.ToString();
        return _precedenceMap.TryGetValue(operatorStr, out var value) ? value.priority : 0;
    }

    private double EvaluatePostfix(string postfixExpression)
    {
        Stack<double> operands = new Stack<double>();

        foreach (char token in postfixExpression)
        {
            if (char.IsDigit(token))
            {
                operands.Push(double.Parse(token.ToString()));
            }
            else if (char.IsLetter(token))
            {
                // Обработка переменных, если необходимо
                // В данном коде переменные просто игнорируются
            }
            else if (_precedenceMap.ContainsKey(token.ToString()))
            {
                // Бинарные операции
                double operand2 = operands.Pop();
                double operand1 = operands.Pop();
                double result = _precedenceMap[token.ToString()].method(operand1, operand2);
                operands.Push(result);
            }
            else if (unaryOperations.ContainsKey(token.ToString()))
            {
                // Унарные операции
                double operand = operands.Pop();
                double result = unaryOperations[token.ToString()](operand);
                operands.Push(result);
            }
        }

        return operands.Pop();
    }
}