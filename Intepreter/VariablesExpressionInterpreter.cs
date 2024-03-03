using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intepreter;

public class VariablesExpressionInterpreter : ExpressionInterpreter
{
    private readonly IDictionary<string,double> _variables;

    public VariablesExpressionInterpreter(IDictionary<string, double> variables)
    {
        _variables = variables;
    }

    public new double Interpret(string expression)
    {
        var postfixExpression = ConvertToPostfix(expression);

        var result = EvaluatePostfix(postfixExpression);

        return result;
    }

    public new double EvaluatePostfix(string postfixExpression)
    {
        var operands = new Stack<double>();
        var tokens = postfixExpression.Split(' ');

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out var number))
            {
                if (_variables.ContainsKey(token))
                    operands.Push(_variables[token]);
                else operands.Push(number);
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

    public void SetVariable(string variable, double value)
    {
        _variables[variable] = value;
    }
}
