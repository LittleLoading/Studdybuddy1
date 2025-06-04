namespace Studdybuddy
{
    partial class QuizForm
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
            comboCategory = new ComboBox();
            btnStartQuiz = new Button();
            pictureFlashcard = new PictureBox();
            txtAnswer = new TextBox();
            btnSubmit = new Button();
            lblFeedback = new Label();
            lblProgress = new Label();
            lblStats = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureFlashcard).BeginInit();
            SuspendLayout();
            // 
            // comboCategory
            // 
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new Point(12, 28);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new Size(121, 23);
            comboCategory.TabIndex = 0;
            // 
            // btnStartQuiz
            // 
            btnStartQuiz.Location = new Point(12, 57);
            btnStartQuiz.Name = "btnStartQuiz";
            btnStartQuiz.Size = new Size(75, 23);
            btnStartQuiz.TabIndex = 1;
            btnStartQuiz.Text = "Start Quiz";
            btnStartQuiz.UseVisualStyleBackColor = true;
            btnStartQuiz.Click += btnStartQuiz_Click;
            // 
            // pictureFlashcard
            // 
            pictureFlashcard.Location = new Point(239, 86);
            pictureFlashcard.Name = "pictureFlashcard";
            pictureFlashcard.Size = new Size(256, 99);
            pictureFlashcard.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureFlashcard.TabIndex = 2;
            pictureFlashcard.TabStop = false;
            // 
            // txtAnswer
            // 
            txtAnswer.Location = new Point(264, 212);
            txtAnswer.Name = "txtAnswer";
            txtAnswer.Size = new Size(198, 23);
            txtAnswer.TabIndex = 3;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(307, 241);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(123, 23);
            btnSubmit.TabIndex = 4;
            btnSubmit.Text = "Submit Awnser";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // lblFeedback
            // 
            lblFeedback.AutoSize = true;
            lblFeedback.Location = new Point(307, 36);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new Size(57, 15);
            lblFeedback.TabIndex = 5;
            lblFeedback.Text = "Feedback";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(564, 31);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(122, 15);
            lblProgress.TabIndex = 6;
            lblProgress.Text = "SelectCategoryToStart";
            // 
            // lblStats
            // 
            lblStats.AutoSize = true;
            lblStats.Location = new Point(564, 86);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(115, 15);
            lblStats.TabIndex = 7;
            lblStats.Text = "Correct: 0 | Wrong: 0";
            // 
            // QuizForm
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
            Name = "QuizForm";
            Text = "QuizForm";
            ((System.ComponentModel.ISupportInitialize)pictureFlashcard).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboCategory;
        private Button btnStartQuiz;
        private PictureBox pictureFlashcard;
        private TextBox txtAnswer;
        private Button btnSubmit;
        private Label lblFeedback;
        private Label lblProgress;
        private Label lblStats;
    }
}