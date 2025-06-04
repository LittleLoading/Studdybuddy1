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
    /// A smart quiz form that repeats wrong answers until all questions are mastered
    /// Shows flashcard images and tracks learning progress with detailed statistics
    /// </summary>
    public partial class SmartQuizForm : Form
    {
        private FlashCardManager manager;
        private Queue<Flashcard> questionQueue;
        private List<Flashcard> originalFlashcards; 
        private List<Flashcard> wrongAnswers; 
        private int currentAttempt = 0;
        private int totalCorrectAnswers = 0;
        private int totalAttempts = 0;
        private Random random = new Random();
        private StatisticsManager statisticsManager;
        private string currentCategory = "";

    
        private Flashcard currentFlashcard;
        private int questionsAnsweredCorrectly = 0;
        private int originalQuestionCount = 0;

        /// <summary>
        /// Creates a new smart quiz form and sets up the initial state
        /// Loads available categories and prepares the progress display
        /// </summary>
        /// <param name="flashCardManager">The manager that handles flashcard data</param>
        public SmartQuizForm(FlashCardManager flashCardManager)
        {
            InitializeComponent();
            manager = flashCardManager;
            statisticsManager = new StatisticsManager();
            questionQueue = new Queue<Flashcard>();
            wrongAnswers = new List<Flashcard>();

            // Load categories
            comboCategory.Items.AddRange(manager.GetAvailableCategories());
            InitializeProgressDisplay();
        }

        /// <summary>
        /// Sets up the initial progress display with helpful instructions
        /// </summary>
        private void InitializeProgressDisplay()
        {
            lblProgress.Text = "Select a category to start smart quiz";
            lblStats.Text = "Mastered: 0 | Attempts: 0 | Accuracy: 0%";
            lblFeedback.Text = "This quiz will repeat wrong answers until you master all questions!";
            lblFeedback.ForeColor = Color.Blue;
        }

        /// <summary>
        /// Starts a new quiz when the user clicks the start button
        /// Loads flashcards from selected category and shuffles them randomly
        /// </summary>
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

                originalFlashcards = manager.LoadFlashcards().OrderBy(f => random.Next()).ToList(); //random shuffle linq

                if (originalFlashcards.Count == 0)
                {
                    MessageBox.Show("No flashcards found in this category.");
                    lblProgress.Text = "No questions available";
                    return;
                }

              
                originalQuestionCount = originalFlashcards.Count;
                questionQueue.Clear();
                wrongAnswers.Clear();

             
                foreach (var flashcard in originalFlashcards)
                {
                    questionQueue.Enqueue(flashcard); //add in Q
                }

              
                questionsAnsweredCorrectly = 0;
                totalAttempts = 0;
                totalCorrectAnswers = 0;
                currentAttempt = 0;

                txtAnswer.Enabled = true;
                btnSubmit.Enabled = true;

                LoadNextQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading flashcards: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the next question from the queue and displays it
        /// Shows completion message if all questions have been mastered
        /// </summary>
        private void LoadNextQuestion()
        {
            if (questionQueue.Count == 0)
            {
                ShowQuizComplete();
                return;
            }

            currentFlashcard = questionQueue.Dequeue();

            UpdateProgressDisplay();

            string imagePath = manager.GetImagePath(currentFlashcard);
            if (File.Exists(imagePath))
            {
               //dispose image previous
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
        /// Updates the progress display with current statistics and status
        /// Shows how many questions are mastered and if current question is a repeat
        /// </summary>
        private void UpdateProgressDisplay()
        {
            int questionsRemaining = questionQueue.Count + 1; // +1 for current question
            int totalQuestions = questionsRemaining + questionsAnsweredCorrectly;

           
            double accuracy = totalAttempts > 0 ? (double)totalCorrectAnswers / totalAttempts * 100 : 0; //if true than return value else return 0


            lblProgress.Text = $"Mastered: {questionsAnsweredCorrectly}/{originalQuestionCount} | Remaining: {questionsRemaining}";
            lblStats.Text = $"Total Attempts: {totalAttempts} | Correct: {totalCorrectAnswers} | Accuracy: {accuracy:F1}%";

          
            if (wrongAnswers.Any(w => w.ImagePath == currentFlashcard?.ImagePath))
            {
                lblProgress.Text += " [REPEAT]";
            }
        }

        /// <summary>
        /// Processes the user's answer when they click submit
        /// Correct answers move to next question, wrong answers get added back to queue
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (currentFlashcard == null) return;

            string userAnswer = txtAnswer.Text.Trim().ToLower();
            string correctAnswer = currentFlashcard.Answer.Trim().ToLower();

            totalAttempts++;

            if (userAnswer == correctAnswer)
            {
                lblFeedback.ForeColor = Color.Green;
                lblFeedback.Text = "Correct! ✓";

                totalCorrectAnswers++;
                questionsAnsweredCorrectly++;

                wrongAnswers.RemoveAll(w => w.ImagePath == currentFlashcard.ImagePath);
            }
            else
            {
                // Wrong answer
                lblFeedback.ForeColor = Color.Red;
                lblFeedback.Text = $"Wrong! Correct answer: {currentFlashcard.Answer}";

              //add to wrong awnsers
                if (!wrongAnswers.Any(w => w.ImagePath == currentFlashcard.ImagePath))
                {
                    wrongAnswers.Add(currentFlashcard);
                }

                // Add question back to queue at random
                var tempQueue = questionQueue.ToList();
                int insertPosition = random.Next(Math.Max(1, tempQueue.Count / 3), tempQueue.Count + 1); //s timhle mi pomohl chat
                tempQueue.Insert(Math.Min(insertPosition, tempQueue.Count), currentFlashcard);

                questionQueue.Clear();
                foreach (var item in tempQueue)
                {
                    questionQueue.Enqueue(item);
                }
            }

            UpdateProgressDisplay();

            Task.Delay(1500).ContinueWith(_ =>
            {
                if (!IsDisposed)
                {
                    Invoke(new Action(() =>
                    {
                        LoadNextQuestion();
                    }));
                }
            });
        }

        /// <summary>
        /// Shows the completion screen when all questions have been mastered.
        /// Calculates final statistics and saves quiz results to file.
        /// </summary>
        private void ShowQuizComplete()
        {
           
            double finalAccuracy = totalAttempts > 0 ? (double)totalCorrectAnswers / totalAttempts * 100 : 0;  //if true than return value else return 0
            int extraAttempts = totalAttempts - originalQuestionCount;

            try
            {
            
                QuizResult quizResult = new QuizResult
                {
                    Category = manager.CurrentCategory,
                    Date = DateTime.Now,
                    TotalQuestions = originalQuestionCount,
                    CorrectAnswers = originalQuestionCount, 
                    WrongAnswers = extraAttempts, 
                    Score = 100.0 
                };

               
                string categoryPath = manager.GetCategoryPath();
                statisticsManager.SaveQuizResult(quizResult, categoryPath);

               
                string message = $"🎉 QUIZ MASTERED! 🎉\n\n" + //styl generoval chat ale napsal jsem to sam jinak
                               $"All {originalQuestionCount} questions answered correctly!\n\n" +
                               $"📊 Performance:\n" +
                               $"• Total Attempts: {totalAttempts}\n" +
                               $"• Extra Practice: {extraAttempts} questions\n" +
                               $"• Overall Accuracy: {finalAccuracy:F1}%\n\n";

                if (extraAttempts == 0)
                {
                    message += "🏆 PERFECT! No mistakes made!";
                }
                else if (extraAttempts <= originalQuestionCount * 0.2)
                {
                    message += "⭐ EXCELLENT! Very few mistakes!";
                }
                else if (extraAttempts <= originalQuestionCount * 0.5)
                {
                    message += "👍 GOOD! Some questions needed practice!";
                }
                else
                {
                    message += "💪 PERSISTENT! You mastered all the tough ones!";
                }

                MessageBox.Show(message, "Smart Quiz Complete!",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Quiz mastered! All questions answered correctly!\n\n" +
                               $"(Note: Could not save statistics: {ex.Message})",
                               "Smart Quiz Complete!",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            lblProgress.Text = $"🎉 ALL {originalQuestionCount} QUESTIONS MASTERED! 🎉";
            lblFeedback.Text = $"Completed in {totalAttempts} attempts with {finalAccuracy:F1}% accuracy";
            lblFeedback.ForeColor = Color.Green;

            txtAnswer.Enabled = false;
            btnSubmit.Enabled = false;
        }
    }
}