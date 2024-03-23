namespace Intepreter;

public class VariablesExpressionInterpreter : ExpressionInterpreter
{
    protected Dictionary<string,double> _variables;
    public Dictionary<string, double> Variables =>  _variables;

    public VariablesExpressionInterpreter(Dictionary<string, double> variables)
    {
        _variables = variables;
    }

    /// <summary>
    /// Переопределение основного метода класса Intepreter
    /// Добавляется ReplaceVariables, для замены переменных на числа
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public override double Interpret(string expression) 
    {
        var afterChangingVariables = ReplaceVariables(expression);

        var postfixExpression = ConvertToPostfix(afterChangingVariables);

        var result = EvaluatePostfix(postfixExpression);

        return result;
    }

    /// <summary>
    /// Заменяет переменные в строке expression на их значения из словаря _variables
    /// </summary>
    /// <param name="expression">Выражение в строковом представлении</param>
    /// <returns>Строка с замененными переменными</returns>
    private string ReplaceVariables(string expression)
    {
        foreach (var pair in _variables)
            expression = expression.Replace(pair.Key, pair.Value.ToString());

        return expression;
    }

    /// <summary>
    /// Переопределенный метод Tokenize с рассчетом на переменные
    /// </summary>
    /// <param name="expression">Выражение в строковом представлении</param>
    /// <returns>Список токенов</returns>
    public override List<Token> Tokenize(string expression)
    {
        var tokens = new List<Token>();
        expression = ConvertToPostfix(expression);

        foreach (var token in expression.Split(" "))
        {
            if (_variables.TryGetValue(token, out _))
                tokens.Add(new Token(token, TokenType.Variable));
            else TokenizeHelper(ref tokens, token);
        }

        return tokens;
    }
}
