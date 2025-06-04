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
    /// BasicQuizForm is a quiz learning without immediate feedback
    /// this form presents all questions first and shows results only 
    /// at the completion of the entire quiz like normal school scenar
    /// </summary>
    public partial class BasicQuizForm : Form
    {

        private FlashCardManager manager;
        private List<Flashcard> flashcards;
        private List<string> userAnswers;
        private int currentIndex = 0;
        private int correctCount = 0;
        private int wrongCount = 0;
        private Random random = new Random();
        private int totalQuestions = 0;
        private StatisticsManager statisticsManager;
        private string currentCategory = "";



        /// <summary>
        /// Initializes a new BasicQuizForm with the flashcard manager.
        /// </summary>
        /// <param name="flashCardManager">Manager instance for handling flashcard operations</param>
        public BasicQuizForm(FlashCardManager flashCardManager)
        {
            InitializeComponent();
            manager = flashCardManager;
            statisticsManager = new StatisticsManager();
            comboCategory.Items.AddRange(manager.GetAvailableCategories());
            userAnswers = new List<string>();

            InitializeProgressDisplay();
        }


        /// <summary>
        /// Sets up the initial state of progress
        /// </summary>
        private void InitializeProgressDisplay()
        {
            lblProgress.Text = "Select a category to start";
            lblStats.Text = "Correct: 0 | Wrong: 0";
            lblFeedback.Text = "";
        }


        /// <summary>
        /// Handles the quiz start button click event
        /// validates category selection loads and randomizes flashcards and begins the quiz session
        /// </summary>
        /// <param name="sender">The button that triggered the event</param>
        /// <param name="e">Event arguments</param>
        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            if (comboCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            string selectedCategory = comboCategory.SelectedItem.ToString();
            currentCategory = selectedCategory;
            manager.SetCategory(selectedCategory);

            try
            {
                flashcards = manager.LoadFlashcards().OrderBy(f => random.Next()).ToList(); //returns randomized shuffeled flashcards
                totalQuestions = flashcards.Count;
                currentIndex = 0;
                correctCount = 0;
                wrongCount = 0;
                userAnswers.Clear();

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
        /// Handles the answer 
        /// Stores the user's response without providing immediate feedback and moves to next question
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (currentIndex >= flashcards.Count) return;

            string userAnswer = txtAnswer.Text.Trim();
            userAnswers.Add(userAnswer);

            currentIndex++;

            UpdateProgressDisplay();

            LoadNextFlashcard();
        }



        /// <summary>
        /// Loads and displays the next flashcard in the quiz sequence
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
                    pictureFlashcard.Image.Dispose(); //dispose last image shown
                }
                pictureFlashcard.Image = Image.FromFile(imagePath); //loaaad new one
            }

          //reset
            txtAnswer.Text = "";
            lblFeedback.Text = ""; 
            txtAnswer.Focus();

            // update button for last questin
            if (currentIndex == flashcards.Count - 1)
            {
                btnSubmit.Text = "Finish Quiz";
            }
            else
            {
                btnSubmit.Text = "Next";
            }
        }

        /// <summary>
        /// Updates the progress display labels to show current quiz status
        /// Shows question number, total questions, and questions answered without revealing scores
        /// results are hidden until completion
        /// </summary>
        private void UpdateProgressDisplay()
        {
            
            int currentQuestionNumber = currentIndex + 1;
            int questionsAnswered = userAnswers.Count;
            
            lblProgress.Text = $"Question {currentQuestionNumber} of {totalQuestions}";

            lblStats.Text = $"Questions answered: {questionsAnswered} of {totalQuestions}";
        }



        /// <summary>
        /// Handles quiz completion by calculating final scores and displaying  results
        /// and presents a complete summary to the user including individual question review.
        /// </summary>
        private void ShowQuizComplete()
        {
            correctCount = 0;
            wrongCount = 0;

            StringBuilder detailedResults = new StringBuilder();
            detailedResults.AppendLine("QUIZ RESULTS");
            detailedResults.AppendLine(new string('=', 50));
            detailedResults.AppendLine();

            for (int i = 0; i < flashcards.Count && i < userAnswers.Count; i++)
            {
                string correctAnswer = flashcards[i].Answer.Trim().ToLower();
                string userAnswer = userAnswers[i].Trim().ToLower();
                bool isCorrect = userAnswer == correctAnswer;

                if (isCorrect)
                {
                    correctCount++;
                }
                else
                {
                    wrongCount++;
                }

                detailedResults.AppendLine($"Question {i + 1}:");
                detailedResults.AppendLine($"  Your answer: {userAnswers[i]}");
                detailedResults.AppendLine($"  Correct answer: {flashcards[i].Answer}");
                detailedResults.AppendLine($"  Result: {(isCorrect ? "✓ CORRECT" : "✗ WRONG")}");
                detailedResults.AppendLine();
            }

            double percentage;
            if (totalQuestions > 0)
            {
                percentage = (double)correctCount / totalQuestions * 100;
            }
            else
            {
                percentage = 0;
            }

            detailedResults.AppendLine(new string('=', 50));
            detailedResults.AppendLine("SUMMARY:");
            detailedResults.AppendLine($"Total Questions: {totalQuestions}");
            detailedResults.AppendLine($"Correct Answers: {correctCount}");
            detailedResults.AppendLine($"Wrong Answers: {wrongCount}");
            detailedResults.AppendLine($"Final Score: {percentage:F1}%");

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
                               $"Score: {percentage:F1}%\n\n" +
                               $"Detailed results:\n{detailedResults.ToString()}",
                               "Quiz Results",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Quiz completed! (Note: Could not save statistics: {ex.Message})\n\n" +
                               $"Total Questions: {totalQuestions}\n" +
                               $"Correct: {correctCount}\n" +
                               $"Wrong: {wrongCount}\n" +
                               $"Score: {percentage:F1}%",
                               "Quiz Completed",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }

            // Update final display with results
            lblProgress.Text = "Quiz completed!";
            lblFeedback.Text = $"Final Score: {correctCount}/{totalQuestions} ({percentage:F1}%)";
            lblFeedback.ForeColor = percentage >= 70 ? Color.Green : Color.Orange;
            lblStats.Text = $"Correct: {correctCount} | Wrong: {wrongCount} | Accuracy: {percentage:F1}%";

            // Disable quiz controls to prevent further interaction
            txtAnswer.Enabled = false;
            btnSubmit.Enabled = false;
            btnSubmit.Text = "Submit";
        }
    }
}