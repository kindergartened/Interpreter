using System.Text;

namespace Intepreter;

public class ExpressionNode
{
    public string Value { get; set; }
    public ExpressionNode? Left { get; set; }
    public ExpressionNode? Right { get; set; }

    public ExpressionNode(string value)
    {
        Value = value;
        Left = null;
        Right = null;
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        ToStringHelper(this, result, "", "");

        return result.ToString();
    }

    private void ToStringHelper(ExpressionNode? node, StringBuilder? result, string prefix, string childrenPrefix)
    {
        if (node == null)
            return;

        result?.AppendLine($"{prefix}{node.Value}");

        if (node.Left != null || node.Right != null)
        {
            ToStringHelper(node.Left, result, $"{childrenPrefix}├─ ", $"{childrenPrefix}│  ");
            ToStringHelper(node.Right, result, $"{childrenPrefix}└─ ", $"{childrenPrefix}   ");
        }
    }
}