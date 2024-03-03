using Intepreter;
using System.Text.RegularExpressions;

namespace InterpretatorForm;

public partial class Form1 : Form
{
    private string _expresion = "";
    private Regex _variablePattern = new(@"(?<!\w)[a-zA-Z](?!\w)");
    private VariablesExpressionInterpreter _expressionInterpreter = new(new Dictionary<string, double>());
    public Form1()
    {
        InitializeComponent();
        ButtonsOptions(false, "#4682b4");
    }

    private void button2_Click(object sender, EventArgs e) //save and calc
    {
        try
        {
            if (richTextBox1.Text == "")
            {
                Error("Expression not entered");
            }
            else
            {
                _expresion = richTextBox1.Text;

                ButtonsOptions(true, "#0066CC");
            }
        }
        catch (Exception)
        {
            Error("Unknown error =(");
        }
    }

    private void button1_Click(object sender, EventArgs e) //Reverse Polish Note
    {
        richTextBox2.Text = _expressionInterpreter.ConvertToPostfix(_expresion);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        CheckHaveVariables();
        List<Token> list = _expressionInterpreter.Tokenize(_expresion);
        string result = "";
        for (int i = 0; i < list.Count; i++)
        {
            result += $"{list[i].Type}: {list[i].Value} \n";
        }
        richTextBox2.Text = result;
    }

    private void button4_Click(object sender, EventArgs e)
    {
        richTextBox2.Text = _expressionInterpreter.BuildTree(_expresion);
    }

    private void ButtonsOptions(bool enabled, string color)
    {
        button1.Enabled = enabled; button1.BackColor = ColorTranslator.FromHtml(color);
        button3.Enabled = enabled; button3.BackColor = ColorTranslator.FromHtml(color);
        button4.Enabled = enabled; button4.BackColor = ColorTranslator.FromHtml(color);
        button5.Enabled = enabled; button5.BackColor = ColorTranslator.FromHtml(color);
        button6.Enabled = enabled; button6.BackColor = ColorTranslator.FromHtml(color);
        button7.Enabled = enabled; button7.BackColor = ColorTranslator.FromHtml(color);
        button8.Enabled = enabled; button8.BackColor = ColorTranslator.FromHtml(color);
    }

    private static void Error(string text)
    {
        MessageBox.Show(
            text,
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error,
            MessageBoxDefaultButton.Button1);
    }

    private void button5_Click(object sender, EventArgs e)
    {

    }

    private void button8_Click(object sender, EventArgs e)
    {
        CheckHaveVariables();
        richTextBox3.Text = _expressionInterpreter.Interpret(_expresion).ToString();
    }

    private void CheckHaveVariables()
    {
        var containVariables = _variablePattern.IsMatch(richTextBox1.Text);
        if (containVariables)
        {
            var dict = new Dictionary<string, double>();
            foreach (Match variable in _variablePattern.Matches(richTextBox1.Text))
            {
                dict[variable.Value] = 1; // Vlad rabotai
            }

            _expressionInterpreter = new VariablesExpressionInterpreter(dict);
        }
    }
}