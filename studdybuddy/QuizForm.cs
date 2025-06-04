using projekt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static projekt.Flashcard;
using static projekt.Form1;
using static Studdybuddy.FlashCardManager;

namespace Studdybuddy
{
    /// <summary>
    /// Quiz form that displays flashcard images and lets users answer them
    /// Tracks score, progress and saves results to statistics
    /// </summary>
    public partial class QuizForm : Form
    {
        private FlashCardManager manager;

        private List<Flashcard> flashcards;

        private int currentIndex = 0;

        private int correctCount = 0;

        private int wrongCount = 0;

        private Random random = new Random();

        private int totalQuestions = 0;

        private StatisticsManager statisticsManager;

        private string currentCategory = "";

        public QuizForm(FlashCardManager flashCardManager)
        {
            InitializeComponent();
            manager = flashCardManager;
            statisticsManager = new StatisticsManager(); 

            comboCategory.Items.AddRange(manager.GetAvailableCategories());

            InitializeProgressDisplay();
        }

        /// <summary>
        /// Sets up the initial progress display before quiz starts
        /// Shows placeholder text until user selects a category
        /// </summary>
        private void InitializeProgressDisplay()
        {
            lblProgress.Text = "Select a category to start";
            lblStats.Text = "Correct: 0 | Wrong: 0";
        }

        /// <summary>
        /// Starts a new quiz when user clicks the start button
        /// Validates category selection, loads flashcards and shuffles them
        /// </summary>
        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            // Make sure user selected a category first
            if (comboCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            string selectedCategory = comboCategory.SelectedItem.ToString();
            currentCategory = selectedCategory; // Store for statistics
            manager.SetCategory(selectedCategory);

            try
            {
              
                flashcards = manager.LoadFlashcards().OrderBy(f => random.Next()).ToList(); //shuffle randomly linq
                totalQuestions = flashcards.Count;

                
                currentIndex = 0;
                correctCount = 0;
                wrongCount = 0;

               
                if (flashcards.Count == 0)
                {
                    MessageBox.Show("No flashcards found in this category.");
                    lblProgress.Text = "No questions to show";
                    return;
                }

               
                txtAnswer.Enabled = true;
                btnSubmit.Enabled = true;

                LoadNextFlashcard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading flashcards: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the next flashcard in the quiz sequence
        /// Shows the image and prepares input field for user answer
        /// </summary>
        private void LoadNextFlashcard()
        {
            if (currentIndex >= flashcards.Count)
            {
                ShowQuizComplete();
                return;
            }

            UpdateProgressDisplay();

            var flashcard = flashcards[currentIndex];
            string imagePath = manager.GetImagePath(flashcard);

            if (File.Exists(imagePath))
            {
                if (pictureFlashcard.Image != null) 
                {
                    pictureFlashcard.Image.Dispose();
                }
                pictureFlashcard.Image = Image.FromFile(imagePath);
            }

        
            txtAnswer.Text = "";
            lblFeedback.Text = "";
            txtAnswer.Focus();
        }

        /// <summary>
        /// Updates the progress and statistics display during the quiz
        /// Shows current question number, score, and accuracy percentage
        /// </summary>
        private void UpdateProgressDisplay()
        {
            int currentQuestionNumber = currentIndex + 1;
            int questionsAnswered = correctCount + wrongCount;
            int questionsRemaining = totalQuestions - questionsAnswered;

           
            lblProgress.Text = $"Question {currentQuestionNumber} of {totalQuestions}";

          
            lblStats.Text = $"Correct: {correctCount} | Wrong: {wrongCount}";

            if (questionsAnswered > 0)
            {
                double percentage = (double)correctCount / questionsAnswered * 100;
                lblStats.Text += $" | Accuracy: {percentage:F1}%"; // fixed point 1 => 0.1
            }
        }

        /// <summary>
        /// Handles when user submits an answer
        /// Compares answer with correct one, gives feedback, and moves to next question
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (currentIndex >= flashcards.Count) return;

            string userAnswer = txtAnswer.Text.Trim().ToLower();
            string correctAnswer = flashcards[currentIndex].Answer.Trim().ToLower();

            if (userAnswer == correctAnswer)
            {
                lblFeedback.ForeColor = Color.Green;
                lblFeedback.Text = "Correct!";
                correctCount++;
            }
            else
            {
                lblFeedback.ForeColor = Color.Red;
                lblFeedback.Text = $"Wrong! Correct answer: {flashcards[currentIndex].Answer}";
                wrongCount++;
            }

            currentIndex++;

            UpdateProgressDisplay();

            Task.Delay(1500).ContinueWith(_ =>
            {
                Invoke(new Action(() =>
                {
                    LoadNextFlashcard();
                }));
            });
        }

        /// <summary>
        /// Shows quiz completion screen and saves results to statistics
        /// </summary>
        private void ShowQuizComplete()
        {
            double percentage = totalQuestions > 0 ? (double)correctCount / totalQuestions * 100 : 0;

            try
            {
                QuizResult quizResult = new QuizResult
                {
                    Category = manager.CurrentCategory, 
                    Date = DateTime.Now,
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctCount,
                    WrongAnswers = wrongCount,
                    Score = Math.Round(percentage, 2) 
                };

                string categoryPath = manager.GetCategoryPath();
                statisticsManager.SaveQuizResult(quizResult, categoryPath);

                MessageBox.Show($"Quiz Completed!\n\n" +
                               $"Total Questions: {totalQuestions}\n" +
                               $"Correct: {correctCount}\n" +
                               $"Wrong: {wrongCount}\n" +
                               $"Score: {percentage:F1}%",
                               "Quiz Results",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Quiz completed! (Note: Could not save statistics: {ex.Message})",
                               "Quiz Completed",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }

            // duplicate?? maybe remove it later!
            MessageBox.Show($"Quiz Complete!\n\n" +
                          $"Total Questions: {totalQuestions}\n" +
                          $"Correct: {correctCount}\n" +
                          $"Wrong: {wrongCount}\n" +
                          $"Accuracy: {percentage:F1}%",
                          "Quiz Results",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);

            lblProgress.Text = "Quiz completed!";
            lblFeedback.Text = $"Final Score: {correctCount}/{totalQuestions} ({percentage:F1}%)";


            lblFeedback.ForeColor = percentage >= 70 ? Color.Green : Color.Orange;


            txtAnswer.Enabled = false;
            btnSubmit.Enabled = false;
        }
    }
}