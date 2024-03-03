using Intepreter;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace InterpretatorForm;

public partial class Form1 : Form
{
    private string _expresion = "";
    private Regex _variablePattern;
    private VariablesExpressionInterpreter _variablesExpressionInterpreter = new(new Dictionary<string, double>());
    private ExpressionInterpreter _expressionInterpreter = new();
    private Dictionary<string, double> dictionary = new();
    private bool containVariables = false;

    public Form1()
    {
        InitializeComponent();
        ButtonsOptions(false, "#4682b4");
        _expressionInterpreter = new();
        string pattern = $@"\b(?<!\w)(?!(?:{string.Join("|", _expressionInterpreter.Operations.Select(x =>
        {
            if (x.Value.Type == OperationType.Unary) return x.Key;
            return null;
        }))})(?!\w))[a-zA-Z]+\b";
        _variablePattern = new Regex(pattern);
    }

    private void button2_Click(object sender, EventArgs e) //save
    {
        try
        {
            containVariables = _variablePattern.IsMatch(richTextBox1.Text);
            CheckHaveVariables(containVariables);
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
        richTextBox2.Text = _variablesExpressionInterpreter.ConvertToPostfix(_expresion);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        List<Token> list = _variablesExpressionInterpreter.Tokenize(_expresion);
        string result = "";

        for (int i = 0; i < list.Count; i++)
            result += $"{list[i].Type}: {list[i].Value} \n";

        richTextBox2.Text = result;
    }

    private void button4_Click(object sender, EventArgs e) //tree
    {
        richTextBox2.Text = _variablesExpressionInterpreter.BuildTree(_expresion);
    }

    private void button5_Click(object sender, EventArgs e)
    {

    }

    private void button8_Click(object sender, EventArgs e) //calc
    {
        if (containVariables)
        {
            Form2 f2 = new(ref dictionary);
            var res = f2.ShowDialog();
            if (res == DialogResult.OK)
            {
                _variablesExpressionInterpreter = new VariablesExpressionInterpreter(f2.ReturnDictionary);
                richTextBox3.Text = _variablesExpressionInterpreter.Interpret(_expresion).ToString();
            }
        }
        else richTextBox3.Text = _expressionInterpreter.Interpret(_expresion).ToString();
    }

    private void CheckHaveVariables(bool containVariables)
    {
        dictionary = new Dictionary<string, double>();
        if (containVariables)
        {
            foreach (Match variable in _variablePattern.Matches(richTextBox1.Text))
            {
                if (dictionary.TryGetValue(variable.Value, out _)) continue;
                dictionary.Add(variable.ToString(), double.NaN);
            }

            _variablesExpressionInterpreter = new VariablesExpressionInterpreter(dictionary);
        }
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

    private void button9_Click(object sender, EventArgs e)
    {
        if (dictionary.Count==0)
        {
            Error("Выражение не сохранено или не имеет переменных");
        }
        else
        {
            MessageBox.Show(
            String.Join("\n", _variablesExpressionInterpreter.Variables),
            "Список переменных",
            MessageBoxButtons.OK);
        }
    }
}