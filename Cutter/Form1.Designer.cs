using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cutter
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            groupBox1 = new GroupBox();
            button2 = new Button();
            label3 = new Label();
            numericUpDown1 = new NumericUpDown();
            textBox3 = new TextBox();
            groupBox2 = new GroupBox();
            button3 = new Button();
            label4 = new Label();
            numericUpDown2 = new NumericUpDown();
            textBox4 = new TextBox();
            button4 = new Button();
            label5 = new Label();
            label1 = new Label();
            label6 = new Label();
            openFileDialog1 = new OpenFileDialog();
            button1 = new Button();
            label7 = new Label();
            textBox5 = new TextBox();
            button5 = new Button();
            labelMaxSize = new Label();
            numericUpDownMaxSize = new NumericUpDown();
            groupBox1.SuspendLayout();
            ((ISupportInitialize)numericUpDown1).BeginInit();
            groupBox2.SuspendLayout();
            ((ISupportInitialize)numericUpDown2).BeginInit();
            ((ISupportInitialize)numericUpDownMaxSize).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(15, 541);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(903, 209);
            textBox1.TabIndex = 0;
            textBox1.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(76, 14);
            textBox2.Margin = new Padding(4, 3, 4, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(748, 23);
            textBox2.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(14, 17);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(45, 16);
            label2.TabIndex = 3;
            label2.Text = "Файл:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(14, 78);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(449, 408);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Шапка";
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(6, 20);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(133, 28);
            button2.TabIndex = 3;
            button2.Text = "Запомнить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(146, 23);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(121, 16);
            label3.TabIndex = 2;
            label3.Text = "Конечная строка:";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Enabled = false;
            numericUpDown1.Location = new Point(299, 21);
            numericUpDown1.Margin = new Padding(4, 3, 4, 3);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(144, 22);
            numericUpDown1.TabIndex = 1;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            // 
            // textBox3
            // 
            textBox3.AcceptsReturn = true;
            textBox3.Enabled = false;
            textBox3.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(7, 54);
            textBox3.Margin = new Padding(4, 3, 4, 3);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ScrollBars = ScrollBars.Vertical;
            textBox3.Size = new Size(434, 346);
            textBox3.TabIndex = 0;
            textBox3.WordWrap = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(numericUpDown2);
            groupBox2.Controls.Add(textBox4);
            groupBox2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox2.Location = new Point(470, 78);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(449, 408);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Концовка";
            // 
            // button3
            // 
            button3.Enabled = false;
            button3.Location = new Point(6, 20);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(133, 28);
            button3.TabIndex = 4;
            button3.Text = "Запомнить";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(146, 24);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(130, 16);
            label4.TabIndex = 4;
            label4.Text = "Начальная строка:";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Enabled = false;
            numericUpDown2.Location = new Point(307, 21);
            numericUpDown2.Margin = new Padding(4, 3, 4, 3);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(135, 22);
            numericUpDown2.TabIndex = 3;
            numericUpDown2.ValueChanged += NumericUpDown2_ValueChanged;
            // 
            // textBox4
            // 
            textBox4.AcceptsReturn = true;
            textBox4.Enabled = false;
            textBox4.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox4.Location = new Point(7, 54);
            textBox4.Margin = new Padding(4, 3, 4, 3);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.ScrollBars = ScrollBars.Vertical;
            textBox4.Size = new Size(434, 346);
            textBox4.TabIndex = 1;
            textBox4.WordWrap = false;
            // 
            // button4
            // 
            button4.Enabled = false;
            button4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(330, 494);
            button4.Margin = new Padding(4, 3, 4, 3);
            button4.Name = "button4";
            button4.Size = new Size(279, 40);
            button4.TabIndex = 5;
            button4.Text = "Разделить";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Button4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Enabled = false;
            label5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.Gray;
            label5.Location = new Point(840, 756);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(64, 13);
            label5.TabIndex = 7;
            label5.Text = "© dece1ver";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(10, 515);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(95, 16);
            label1.TabIndex = 1;
            label1.Text = "Инорфмация:";
            // 
            // label6
            // 
            label6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.ImageAlign = ContentAlignment.BottomCenter;
            label6.Location = new Point(14, 756);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(819, 18);
            label6.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileOk += OpenFileDialog1_FileOk;
            // 
            // button1
            // 
            button1.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(832, 13);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(88, 25);
            button1.TabIndex = 4;
            button1.Text = "Обзор";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(638, 507);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(60, 16);
            label7.TabIndex = 5;
            label7.Text = "Подача:";
            label7.Visible = false;
            // 
            // textBox5
            // 
            textBox5.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textBox5.Location = new Point(718, 503);
            textBox5.Margin = new Padding(4, 3, 4, 3);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(86, 22);
            textBox5.TabIndex = 9;
            textBox5.Visible = false;
            // 
            // button5
            // 
            button5.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            button5.Location = new Point(811, 502);
            button5.Margin = new Padding(4, 3, 4, 3);
            button5.Name = "button5";
            button5.Size = new Size(108, 28);
            button5.TabIndex = 5;
            button5.Text = "Запомнить";
            button5.UseVisualStyleBackColor = true;
            button5.Visible = false;
            button5.Click += Button5_Click;
            // 
            // labelMaxSize
            // 
            labelMaxSize.AutoSize = true;
            labelMaxSize.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            labelMaxSize.Location = new Point(14, 48);
            labelMaxSize.Margin = new Padding(4, 0, 4, 0);
            labelMaxSize.Name = "labelMaxSize";
            labelMaxSize.Size = new Size(165, 16);
            labelMaxSize.TabIndex = 10;
            labelMaxSize.Text = "Допустимый размер, Кб:";
            // 
            // numericUpDownMaxSize
            // 
            numericUpDownMaxSize.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            numericUpDownMaxSize.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownMaxSize.Location = new Point(177, 46);
            numericUpDownMaxSize.Margin = new Padding(4, 3, 4, 3);
            numericUpDownMaxSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDownMaxSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownMaxSize.Name = "numericUpDownMaxSize";
            numericUpDownMaxSize.Size = new Size(100, 22);
            numericUpDownMaxSize.TabIndex = 11;
            numericUpDownMaxSize.Value = new decimal(new int[] { 480, 0, 0, 0 });
            numericUpDownMaxSize.ValueChanged += NumericUpDownMaxSize_ValueChanged;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 780);
            Controls.Add(numericUpDownMaxSize);
            Controls.Add(labelMaxSize);
            Controls.Add(button5);
            Controls.Add(textBox5);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(button4);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximumSize = new Size(949, 819);
            MinimumSize = new Size(949, 819);
            Name = "MainWindow";
            Text = "Разделятель";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((ISupportInitialize)numericUpDown1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((ISupportInitialize)numericUpDown2).EndInit();
            ((ISupportInitialize)numericUpDownMaxSize).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private Button button2;
        private Label label4;
        private NumericUpDown numericUpDown2;
        private Button button3;
        private Button button4;
        private Label label5;
        private Label label1;
        private Label label6;
        private OpenFileDialog openFileDialog1;
        private Button button1;
        private Label label7;
        private TextBox textBox5;
        private Button button5;
        private Label labelMaxSize;
        private NumericUpDown numericUpDownMaxSize;
    }
}

