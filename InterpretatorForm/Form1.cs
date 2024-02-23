using Intepreter;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InterpretatorForm
{
    public partial class Form1 : Form
    {
        private string expresion = "";
        private readonly ExpressionInterpreter expressionInterpreter = new();
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
                    expresion = richTextBox1.Text;
                    richTextBox3.Text = expressionInterpreter.Interpret(expresion).ToString();
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
            richTextBox2.Text = expressionInterpreter.ConvertToPostfix(expresion);
        }

        private void ButtonsOptions(bool enabled, string color)
        {
            button1.Enabled = enabled; button1.BackColor = ColorTranslator.FromHtml(color);
            button3.Enabled = enabled; button3.BackColor = ColorTranslator.FromHtml(color);
            button4.Enabled = enabled; button4.BackColor = ColorTranslator.FromHtml(color);
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

        private void button3_Click(object sender, EventArgs e)
        {
            List<Token> list = expressionInterpreter.Tokenize(expresion);
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += $"{list[i].Type}: {list[i].Value} \n";
            }
            richTextBox2.Text = result;
        }
    }
}
