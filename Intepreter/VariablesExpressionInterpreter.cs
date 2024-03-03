namespace Intepreter;

public class VariablesExpressionInterpreter : ExpressionInterpreter
{
    private IDictionary<string,double> _variables;
    public IDictionary<string, double> Variables =>  _variables;

    public VariablesExpressionInterpreter(IDictionary<string, double> variables)
    {
        _variables = variables;
    }

    public override double Interpret(string expression) 
    {
        var afterChangingVariables = ReplaceVariables(expression);

        var postfixExpression = ConvertToPostfix(afterChangingVariables);

        var result = EvaluatePostfix(postfixExpression);

        return result;
    }

    private string ReplaceVariables(string expression)
    {
        foreach (var pair in _variables)
            expression = expression.Replace(pair.Key, pair.Value.ToString());

        return expression;
    }

    public override List<Token> Tokenize(string expression)
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
            else if (_variables.TryGetValue(token, out _))
                tokens.Add(new Token(token, TokenType.Variable));                
            else if (Operations[token].Type == OperationType.Binary)
                tokens.Add(new Token(token, TokenType.Operator));
            else if (Operations[token].Type == OperationType.Unary)
                tokens.Add(new Token(token.ToLower(), TokenType.UnaryFunction));
            else if (Operations[token].Type == OperationType.Logical ||
                     Operations[token].Type == OperationType.LogicalDouble)
                tokens.Add(new Token(token.ToLower(), TokenType.LogicalFunction));
            else
                tokens.Add(new Token(token, TokenType.Unknown));
        }

        return tokens;
    }
}
