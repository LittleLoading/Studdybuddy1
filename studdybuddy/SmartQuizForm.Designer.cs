namespace Studdybuddy
{
    partial class SmartQuizForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblStats = new Label();
            lblProgress = new Label();
            lblFeedback = new Label();
            btnSubmit = new Button();
            txtAnswer = new TextBox();
            pictureFlashcard = new PictureBox();
            btnStartQuiz = new Button();
            comboCategory = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureFlashcard).BeginInit();
            SuspendLayout();
            // 
            // lblStats
            // 
            lblStats.AutoSize = true;
            lblStats.Location = new Point(615, 165);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(115, 15);
            lblStats.TabIndex = 15;
            lblStats.Text = "Correct: 0 | Wrong: 0";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(615, 110);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(122, 15);
            lblProgress.TabIndex = 14;
            lblProgress.Text = "SelectCategoryToStart";
            // 
            // lblFeedback
            // 
            lblFeedback.AutoSize = true;
            lblFeedback.Location = new Point(358, 115);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new Size(57, 15);
            lblFeedback.TabIndex = 13;
            lblFeedback.Text = "Feedback";
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(358, 320);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(123, 23);
            btnSubmit.TabIndex = 12;
            btnSubmit.Text = "Submit Awnser";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // txtAnswer
            // 
            txtAnswer.Location = new Point(315, 291);
            txtAnswer.Name = "txtAnswer";
            txtAnswer.Size = new Size(198, 23);
            txtAnswer.TabIndex = 11;
            // 
            // pictureFlashcard
            // 
            pictureFlashcard.Location = new Point(290, 165);
            pictureFlashcard.Name = "pictureFlashcard";
            pictureFlashcard.Size = new Size(256, 99);
            pictureFlashcard.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureFlashcard.TabIndex = 10;
            pictureFlashcard.TabStop = false;
            // 
            // btnStartQuiz
            // 
            btnStartQuiz.Location = new Point(63, 136);
            btnStartQuiz.Name = "btnStartQuiz";
            btnStartQuiz.Size = new Size(75, 23);
            btnStartQuiz.TabIndex = 9;
            btnStartQuiz.Text = "Start Quiz";
            btnStartQuiz.UseVisualStyleBackColor = true;
            btnStartQuiz.Click += btnStartQuiz_Click;
            // 
            // comboCategory
            // 
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new Point(63, 107);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new Size(121, 23);
            comboCategory.TabIndex = 8;
            // 
            // SmartQuizForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblStats);
            Controls.Add(lblProgress);
            Controls.Add(lblFeedback);
            Controls.Add(btnSubmit);
            Controls.Add(txtAnswer);
            Controls.Add(pictureFlashcard);
            Controls.Add(btnStartQuiz);
            Controls.Add(comboCategory);
            Name = "SmartQuizForm";
            Text = "SmartQuizForm";
            ((System.ComponentModel.ISupportInitialize)pictureFlashcard).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblStats;
        private Label lblProgress;
        private Label lblFeedback;
        private Button btnSubmit;
        private TextBox txtAnswer;
        private PictureBox pictureFlashcard;
        private Button btnStartQuiz;
        private ComboBox comboCategory;
    }
}