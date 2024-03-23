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
    private Regex _hasntLogicalPattern;
    private TruthTable _truthTable = new(new Dictionary<string, double>());
    private Dictionary<string, double> _variablesDictionary = new();
    private bool _containVariables = false;
    // private bool _onlyLogical = false;

    public Form1()
    {
        InitializeComponent();
        ButtonsOptions(false, "#4682b4");
        _truthTable = new(new());
        string pattern = $@"\b(?<!\w)(?!(?:{string.Join("|", _truthTable.Operations.Select(x =>
        {
            if (x.Value.Type == OperationType.Unary) return x.Key;
            return null;
        }))})(?!\w))[a-zA-Z]+\b";
        // string logicalPattern = $@"^(?![a-zA-Z]*[0-9][a-zA-Z]*$)(?![0-9]*[a-zA-Z][0-9]*)[a-zA-Z0-9]+(\s*(?:&&|\|\||&|\||==|!=|\->|\^|!)\s*[a-zA-Z0-9]+)*$";
        // Console.WriteLine(logicalPattern);
        // _hasntLogicalPattern = new Regex(logicalPattern);
        _variablePattern = new Regex(pattern);
    }

    private void button2_Click(object sender, EventArgs e) //save
    {
        try
        {
            _containVariables = _variablePattern.IsMatch(richTextBox1.Text);
            CheckHaveVariables(_containVariables);
            // _onlyLogical = !_hasntLogicalPattern.IsMatch(richTextBox1.Text);
            _truthTable.BuildTruthTable(richTextBox1.Text);
            if (richTextBox1.Text == "")
            {
                Error("Expression not entered");
            }
            else
            {
                _expresion = richTextBox1.Text;
                ButtonsOptions(true, "#0066CC");
                // if (_onlyLogical)
                // {
                    // DisableLogical();
                // }
            }
        }
        catch (Exception)
        {
            Error("Unknown error =(");
        }
    }

    private void button1_Click(object sender, EventArgs e) //Reverse Polish Note
    {
        richTextBox2.Text = _truthTable.ConvertToPostfix(_expresion);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        List<Token> list = _truthTable.Tokenize(_expresion);
        string result = "";

        for (int i = 0; i < list.Count; i++)
            result += $"{list[i].Type}: {list[i].Value} \n";

        richTextBox2.Text = result;
    }

    private void button4_Click(object sender, EventArgs e) //tree
    {
        richTextBox2.Text = _truthTable.BuildTree(_expresion);
    }

    private void button5_Click(object sender, EventArgs e)
    {
        richTextBox2.Text = _truthTable.GetDnf();
    }

    private void button8_Click(object sender, EventArgs e) //calc
    {
        if (_containVariables)
        {
            Form2 f2 = new(ref _variablesDictionary);
            var res = f2.ShowDialog();
            if (res == DialogResult.OK)
            {
                _truthTable = new TruthTable(f2.ReturnDictionary);
                richTextBox3.Text = _truthTable.Interpret(_expresion).ToString();
            }
            _truthTable.BuildTruthTable(richTextBox1.Text);
        }
        else richTextBox3.Text = _truthTable.Interpret(_expresion).ToString();
    }

    private void CheckHaveVariables(bool containVariables)
    {
        _variablesDictionary = new Dictionary<string, double>();
        if (containVariables)
        {
            foreach (Match variable in _variablePattern.Matches(richTextBox1.Text))
            {
                if (_variablesDictionary.TryGetValue(variable.Value, out _)) continue;
                _variablesDictionary.Add(variable.ToString(), double.NaN);
            }

            _truthTable = new TruthTable(_variablesDictionary);
        }
    }

    private void ButtonsOptions(bool enabled, string color)
    {
        button1.Enabled = enabled;
        button1.BackColor = ColorTranslator.FromHtml(color);
        button3.Enabled = enabled;
        button3.BackColor = ColorTranslator.FromHtml(color);
        button4.Enabled = enabled;
        button4.BackColor = ColorTranslator.FromHtml(color);
        button5.Enabled = enabled;
        button5.BackColor = ColorTranslator.FromHtml(color);
        button6.Enabled = enabled;
        button6.BackColor = ColorTranslator.FromHtml(color);
        button7.Enabled = enabled;
        button7.BackColor = ColorTranslator.FromHtml(color);
        button8.Enabled = enabled;
        button8.BackColor = ColorTranslator.FromHtml(color);
    }

    private void DisableLogical()
    {
        button5.Enabled = false;
        button5.BackColor = ColorTranslator.FromHtml("#4682b4");
        button6.Enabled = false;
        button6.BackColor = ColorTranslator.FromHtml("#4682b4");
        button7.Enabled = false;
        button7.BackColor = ColorTranslator.FromHtml("#4682b4");
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
        if (_variablesDictionary.Count == 0)
        {
            Error("Выражение не сохранено или не имеет переменных");
        }
        else
        {
            MessageBox.Show(
                String.Join("\n", _truthTable.Variables),
                "Список переменных",
                MessageBoxButtons.OK);
        }
    }

    private void button7_Click(object sender, EventArgs e)
    {
        richTextBox2.Text = _truthTable.Table?.ToString();
    }

    private void button6_Click(object sender, EventArgs e)
    {
        richTextBox2.Text = _truthTable.GetKnf();
    }
}