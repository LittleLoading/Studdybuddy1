namespace Studdybuddy
{
    partial class MainMenuForm
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
            lblTitle = new Label();
            btnManageFlashcards = new Button();
            btnStartQuiz = new Button();
            btnExit = new Button();
            btnStatistics = new Button();
            btnSmartQuizForm = new Button();
            btnBasicQuizForm = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.Location = new Point(25, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(297, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "StuddyBuddy - flashcard learning";
            // 
            // btnManageFlashcards
            // 
            btnManageFlashcards.Location = new Point(54, 59);
            btnManageFlashcards.Name = "btnManageFlashcards";
            btnManageFlashcards.Size = new Size(232, 23);
            btnManageFlashcards.TabIndex = 1;
            btnManageFlashcards.Text = "Manage Flashcards";
            btnManageFlashcards.UseVisualStyleBackColor = true;
            btnManageFlashcards.Click += btnManageFlashcards_Click;
            // 
            // btnStartQuiz
            // 
            btnStartQuiz.Location = new Point(54, 121);
            btnStartQuiz.Name = "btnStartQuiz";
            btnStartQuiz.Size = new Size(232, 23);
            btnStartQuiz.TabIndex = 2;
            btnStartQuiz.Text = "Start Quiz";
            btnStartQuiz.UseVisualStyleBackColor = true;
            btnStartQuiz.Click += btnStartQuiz_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Brown;
            btnExit.Location = new Point(54, 406);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(232, 23);
            btnExit.TabIndex = 3;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // btnStatistics
            // 
            btnStatistics.Location = new Point(54, 349);
            btnStatistics.Name = "btnStatistics";
            btnStatistics.Size = new Size(232, 23);
            btnStatistics.TabIndex = 5;
            btnStatistics.Text = "Statistics";
            btnStatistics.UseVisualStyleBackColor = true;
            btnStatistics.Click += btnStatistics_Click;
            // 
            // btnSmartQuizForm
            // 
            btnSmartQuizForm.Location = new Point(54, 214);
            btnSmartQuizForm.Name = "btnSmartQuizForm";
            btnSmartQuizForm.Size = new Size(232, 23);
            btnSmartQuizForm.TabIndex = 6;
            btnSmartQuizForm.Text = "Smart Quiz";
            btnSmartQuizForm.UseVisualStyleBackColor = true;
            btnSmartQuizForm.Click += btnSmartQuizForm_Click;
            // 
            // btnBasicQuizForm
            // 
            btnBasicQuizForm.Location = new Point(54, 185);
            btnBasicQuizForm.Name = "btnBasicQuizForm";
            btnBasicQuizForm.Size = new Size(232, 23);
            btnBasicQuizForm.TabIndex = 7;
            btnBasicQuizForm.Text = "Basic Quiz";
            btnBasicQuizForm.UseVisualStyleBackColor = true;
            btnBasicQuizForm.Click += btnBasicQuizForm_Click;
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 450);
            Controls.Add(btnBasicQuizForm);
            Controls.Add(btnSmartQuizForm);
            Controls.Add(btnStatistics);
            Controls.Add(btnExit);
            Controls.Add(btnStartQuiz);
            Controls.Add(btnManageFlashcards);
            Controls.Add(lblTitle);
            Name = "MainMenuForm";
            Text = "MainMenuForm";
            Load += MainMenuForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Button btnManageFlashcards;
        private Button btnStartQuiz;
        private Button btnExit;
        private Button btnStatistics;
        private Button btnSmartQuizForm;
        private Button btnBasicQuizForm;
    }
}