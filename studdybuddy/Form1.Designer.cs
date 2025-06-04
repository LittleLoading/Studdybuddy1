namespace projekt
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
            pictureBox1 = new PictureBox();
            button1 = new Button();
            textBoxAnswer = new TextBox();
            button2 = new Button();
            listBox1 = new ListBox();
            button3 = new Button();
            button4 = new Button();
            lblCurrentCategory = new Label();
            comboBoxCategories = new ComboBox();
            button5 = new Button();
            btnStartQuiz = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(115, 51);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(421, 128);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(272, 234);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(82, 22);
            button1.TabIndex = 1;
            button1.Text = "Upload";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonUploadImage_Click;
            // 
            // textBoxAnswer
            // 
            textBoxAnswer.Location = new Point(216, 184);
            textBoxAnswer.Margin = new Padding(3, 2, 3, 2);
            textBoxAnswer.Name = "textBoxAnswer";
            textBoxAnswer.Size = new Size(198, 23);
            textBoxAnswer.TabIndex = 2;
            // 
            // button2
            // 
            button2.Location = new Point(252, 208);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(128, 22);
            button2.TabIndex = 3;
            button2.Text = "Save Flashcard";
            button2.UseVisualStyleBackColor = true;
            button2.Click += buttonSaveFlashcard_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(581, 26);
            listBox1.Margin = new Padding(3, 2, 3, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(163, 364);
            listBox1.TabIndex = 4;
            listBox1.Tag = "listBox1";
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Location = new Point(242, 355);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(173, 22);
            button3.TabIndex = 5;
            button3.Text = "Load from folder";
            button3.UseVisualStyleBackColor = true;
            button3.Click += buttonLoadFromFolder_Click;
            // 
            // button4
            // 
            button4.Location = new Point(261, 327);
            button4.Name = "button4";
            button4.Size = new Size(137, 23);
            button4.TabIndex = 6;
            button4.Text = "Create Category";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // lblCurrentCategory
            // 
            lblCurrentCategory.AutoSize = true;
            lblCurrentCategory.Location = new Point(282, 26);
            lblCurrentCategory.Name = "lblCurrentCategory";
            lblCurrentCategory.Size = new Size(98, 15);
            lblCurrentCategory.TabIndex = 7;
            lblCurrentCategory.Text = "CurrentCategory:";
            // 
            // comboBoxCategories
            // 
            comboBoxCategories.FormattingEnabled = true;
            comboBoxCategories.Location = new Point(415, 23);
            comboBoxCategories.Name = "comboBoxCategories";
            comboBoxCategories.Size = new Size(121, 23);
            comboBoxCategories.TabIndex = 8;
            comboBoxCategories.Text = "Select Category";
            comboBoxCategories.Click += comboBoxCategories_SelectedIndexChanged;
            // 
            // button5
            // 
            button5.Location = new Point(611, 395);
            button5.Name = "button5";
            button5.Size = new Size(109, 23);
            button5.TabIndex = 9;
            button5.Text = "Delete Flashcard";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // btnStartQuiz
            // 
            btnStartQuiz.Location = new Point(289, 415);
            btnStartQuiz.Name = "btnStartQuiz";
            btnStartQuiz.Size = new Size(75, 23);
            btnStartQuiz.TabIndex = 10;
            btnStartQuiz.Text = "Start Quiz";
            btnStartQuiz.UseVisualStyleBackColor = true;
            btnStartQuiz.Click += btnStartQuiz_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStartQuiz);
            Controls.Add(button5);
            Controls.Add(comboBoxCategories);
            Controls.Add(lblCurrentCategory);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(listBox1);
            Controls.Add(button2);
            Controls.Add(textBoxAnswer);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private TextBox textBoxAnswer;
        private Button button2;
        private ListBox listBox1;
        private Button button3;
        private Button button4;
        private Label lblCurrentCategory;
        private ComboBox comboBoxCategories;
        private Button button5;
        private Button btnStartQuiz;
    }
}
