using System.Data;


namespace InterpretatorForm
{
    public partial class Form2 : Form
    {
        public event EventHandler<Dictionary<string, double>> ReturnDictionary;
        private Dictionary<string, double> dictionary;

        public Form2(ref Dictionary<string, double> dict)
        {
            InitializeComponent();
            dictionary = new Dictionary<string, double>(dict);
            toolTip1.SetToolTip(variablesTextBox, "Введите значения переменных\r\nв том порядке, в котором они\r\nвведены в выражении.\r\nПодсказка: " 
                + String.Join(", ", dictionary.Select(w => w.Key))+".");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<double> result = variablesTextBox.Text.Split(", ").Select(w=>double.Parse(w)).ToList();
            int index = 0;

            foreach (var key in dictionary.Keys)
            {
                dictionary[key] = result[index];
                index++;
            }

            ReturnDictionary?.Invoke(this, dictionary);

            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
