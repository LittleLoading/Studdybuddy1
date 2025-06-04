using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using projekt;
using System.IO;

namespace Studdybuddy
{
    /// <summary>
    /// Main menu form that serves as the entry point for the flashcard application
    /// Provides navigation to different features like flashcard management, quizzes, and statistics
    /// </summary>
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }
        private void MainMenuForm_Load(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Opens the flashcard management form (Form1)
        /// </summary>
        private void btnManageFlashcards_Click(object sender, EventArgs e)
        {
           
            Form1 flashcardForm = new Form1();
            this.Hide();
            flashcardForm.ShowDialog(); 
            this.Show();
        }

        /// <summary>
        /// Opens the regular quiz form for taking flashcard quizzes
        /// Creates a new FlashCardManager instance and passes it to the quiz
        /// </summary>
        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            FlashCardManager flashCardManager = new FlashCardManager();

            QuizForm quizForm = new QuizForm(flashCardManager);
            this.Hide();
            quizForm.ShowDialog(); 
            this.Show(); 
        }

        /// <summary>
        /// Exits the entire application when user clicks exit button
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Opens the statistics form to view quiz performance data
        /// </summary>
        private void btnStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                string dataPath = Path.Combine(Application.StartupPath, "data");

                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }

               //debug need to remove later
                MessageBox.Show($"Data path: {dataPath}\nDirectories found: {Directory.GetDirectories(dataPath).Length}");

                StatisticsForm statisticsForm = new StatisticsForm(dataPath);
                this.Hide();
                statisticsForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nStack Trace: {ex.StackTrace}",
                    "Error Opening Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        /// <summary>
        /// Opens the smart quiz form
        /// </summary>
        private void btnSmartQuizForm_Click(object sender, EventArgs e)
        {
            FlashCardManager flashCardManager = new FlashCardManager();
            SmartQuizForm smartQuizForm = new SmartQuizForm(flashCardManager);
            this.Hide();
            smartQuizForm.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Opens the basic quiz form 
        /// </summary>
        private void btnBasicQuizForm_Click(object sender, EventArgs e)
        {
           
            FlashCardManager flashCardManager = new FlashCardManager();

            BasicQuizForm basicForm = new BasicQuizForm(flashCardManager);
            this.Hide(); 
            basicForm.ShowDialog();
            this.Show();
        }
    }
}