using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intepreter;

public class VariablesExpressionInterpreter : ExpressionInterpreter
{
    private IDictionary<string,double> _variables;


    public VariablesExpressionInterpreter(IDictionary<string, double> variables)
    {
        _variables = variables;
    }

    public new double Interpret(string expression) 
    {
        var afterChangingVariables = ReplaceVariables(expression);

        var postfixExpression = ConvertToPostfix(afterChangingVariables);

        var result = EvaluatePostfix(postfixExpression);

        return result;
    }

    private string ReplaceVariables(string expression)
    {
        foreach (KeyValuePair<string, double> pair in _variables)
        {
            _variables[pair.Key] = 0;
            expression = expression.Replace(pair.Key, pair.Value.ToString());
        }

        return expression;
    }
}
