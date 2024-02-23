namespace InterpretatorForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            button1 = new Button();
            richTextBox2 = new RichTextBox();
            button2 = new Button();
            button3 = new Button();
            groupBox1 = new GroupBox();
            button4 = new Button();
            label2 = new Label();
            richTextBox3 = new RichTextBox();
            label3 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Cursor = Cursors.Hand;
            richTextBox1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            richTextBox1.Location = new Point(52, 47);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(558, 50);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.HotTrack;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.White;
            button1.Location = new Point(37, 40);
            button1.Name = "button1";
            button1.Size = new Size(174, 60);
            button1.TabIndex = 2;
            button1.Text = "Обратная польская запись";
            button1.UseVisualStyleBackColor = false;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(250, 22);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new Size(505, 228);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            // 
            // button2
            // 
            button2.BackColor = SystemColors.HotTrack;
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = Color.White;
            button2.Location = new Point(638, 47);
            button2.Name = "button2";
            button2.Size = new Size(132, 60);
            button2.TabIndex = 4;
            button2.Text = "Сохранить и вычислить";
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.HotTrack;
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button3.ForeColor = Color.White;
            button3.Location = new Point(37, 172);
            button3.Name = "button3";
            button3.Size = new Size(174, 60);
            button3.TabIndex = 5;
            button3.Text = "Список лексем";
            button3.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(richTextBox2);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(15, 208);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(771, 267);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Лексический анализ";
            // 
            // button4
            // 
            button4.BackColor = SystemColors.HotTrack;
            button4.Cursor = Cursors.Hand;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button4.ForeColor = Color.White;
            button4.Location = new Point(37, 106);
            button4.Name = "button4";
            button4.Size = new Size(174, 60);
            button4.TabIndex = 6;
            button4.Text = "Дерево лексем";
            button4.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(52, 117);
            label2.Name = "label2";
            label2.Size = new Size(97, 22);
            label2.TabIndex = 8;
            label2.Text = "Результат";
            // 
            // richTextBox3
            // 
            richTextBox3.Cursor = Cursors.No;
            richTextBox3.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            richTextBox3.Location = new Point(52, 142);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.ReadOnly = true;
            richTextBox3.Size = new Size(558, 50);
            richTextBox3.TabIndex = 9;
            richTextBox3.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(52, 22);
            label3.Name = "label3";
            label3.Size = new Size(114, 22);
            label3.TabIndex = 10;
            label3.Text = "Выражение";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(798, 489);
            Controls.Add(label3);
            Controls.Add(richTextBox3);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Controls.Add(button2);
            Controls.Add(richTextBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Interpretator";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button button1;
        private RichTextBox richTextBox2;
        private Button button2;
        private Button button3;
        private GroupBox groupBox1;
        private Button button4;
        private Label label2;
        private RichTextBox richTextBox3;
        private Label label3;
    }
}
