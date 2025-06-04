using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Studdybuddy.Lib;

namespace Studdybuddy
{
    /// <summary>
    /// This class is used for showing the statistics of the user based on category and their quiz activity
    /// Loading the data from json file
    /// </summary>
    public partial class StatisticsForm : Form
    {
        private StatisticsManager statisticsManager;
        private string baseDirectory;
        private Chart scoreChart;
        private Label lblStatsSummary;

        /// <summary>
        /// Default constructor inicializing components
        /// </summary>
        public StatisticsForm()
        {
            InitializeComponent();
            try
            {
                AddChartControl();
                AddSummaryLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// constructor that initializes the form with base directory
        /// sets up the statistics manager and loads available categories
        /// </summary>
        /// <param name="baseDirectory">The root directory path where quiz data is stored</param>
        public StatisticsForm(string baseDirectory) : this() // ": this" calls the staticsForm first so inicializes all components
        {
            this.baseDirectory = baseDirectory;
            this.statisticsManager = new StatisticsManager();

            LoadCategories();
        }

        /// <summary>
        /// Creates and adds the chart control to the form for displaying quiz statistics
        /// Uses the StatisticsLibrary to properly configure and position the chart
        /// </summary>
        private void AddChartControl()
        {
            scoreChart = StatisticsLibrary.AddChartControl(this);
        }

        /// <summary>
        /// Creates and configures the summary label that displays statistical overview
        /// The label shows category name, total quizzes, average score, and best score
        /// </summary>
        private void AddSummaryLabel()
        {
            try
            {
                lblStatsSummary = new Label();
                lblStatsSummary.Size = new Size(800, 60);
                lblStatsSummary.Location = new Point(50, 580);
                lblStatsSummary.Font = new Font("Arial", 10, FontStyle.Regular);
                lblStatsSummary.ForeColor = Color.DarkBlue;
                lblStatsSummary.Text = "Select a category to view statistics";
                lblStatsSummary.AutoSize = false;
                this.Controls.Add(lblStatsSummary);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating summary label: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        /// <summary>
        /// Loads available quiz categories into the category combo box
        /// Uses the StatisticsLibrary to populate the dropdown with categories
        /// found in the base directory
        /// </summary>
        private void LoadCategories()
        {
            StatisticsLibrary.LoadCategories(cmbCategory, lblStatsSummary, baseDirectory);
        }

        /// <summary>
        /// Loads and displays statistical data for a specific quiz category
        /// This includes populating the chart with score data and updating the summary
        /// </summary>
        /// <param name="categoryName">The name of the category to load statistics for</param>
        private void LoadCategoryStatistics(string categoryName)
        {
            StatisticsLibrary.LoadCategoryStatistics(
                categoryName,
                baseDirectory,
                statisticsManager,
                scoreChart,
                lblStatsSummary,
                UpdateSummaryLabel
            );
        }



        /// <summary>
        /// Handles the category selection change event from the combo box.
        /// When a user selects a different category, this method clears the current
        /// chart data and loads statistics for the newly selected category.
        /// </summary>
        /// <param name="sender">The combo box that triggered the event</param>
        /// <param name="e">Event arguments containing selection details</param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedItem == null) return;

                string selectedCategory = cmbCategory.SelectedItem.ToString();

              
                if (scoreChart != null && scoreChart.Series.Count > 0)
                {
                    scoreChart.Series[0].Points.Clear(); //clear the charttr data for new data
                }

                if (!string.IsNullOrEmpty(baseDirectory) && statisticsManager != null)
                {
                    LoadCategoryStatistics(selectedCategory);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates the summary label with calculated statistics for the category
        /// </summary>
        /// <param name="categoryName">Name of the category being displayed</param>
        /// <param name="totalQuizzes">Total number of quizzes taken in this category</param>
        /// <param name="averageScore">Average score percentage across all quizzes</param>
        /// <param name="bestScore">Highest score percentage achieved in this category</param>
        private void UpdateSummaryLabel(string categoryName, int totalQuizzes, double averageScore, double bestScore)
        {
            try
            {
                if (lblStatsSummary != null)
                {
                    lblStatsSummary.Text = $"Category: {categoryName} | " +
                                         $"Total Quizzes: {totalQuizzes} | " +
                                         $"Average Score: {averageScore:F1}% | " +
                                         $"Best Score: {bestScore:F1}%";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating summary: {ex.Message}");
            }
        }
    }
}