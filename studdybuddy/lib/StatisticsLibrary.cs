using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Studdybuddy.Lib
{
    public static class StatisticsLibrary
    {
        public static Chart AddChartControl(Form parentForm, int x = 50, int y = 120, int width = 800, int height = 450)
        {
            try
            {
                Chart scoreChart = new Chart();
                scoreChart.Size = new Size(width, height);
                scoreChart.Location = new Point(x, y);
                scoreChart.BackColor = Color.White;
                scoreChart.BorderlineColor = Color.Gray;
                scoreChart.BorderlineWidth = 1;
                scoreChart.BorderlineDashStyle = ChartDashStyle.Solid;

                // Add Chart Area
                ChartArea chartArea = new ChartArea("ScoreArea");
                chartArea.AxisX.Title = "Date";
                chartArea.AxisY.Title = "Score (%)";
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.Maximum = 100;
                chartArea.AxisX.MajorGrid.Enabled = true;
                chartArea.AxisY.MajorGrid.Enabled = true;
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.LabelStyle.Format = "MM/dd/yyyy";
                chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
                chartArea.BackColor = Color.WhiteSmoke;
                scoreChart.ChartAreas.Add(chartArea);

                // Add Series
                Series series = new Series("Quiz Scores");
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.Blue;
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 8;
                series.MarkerColor = Color.DarkBlue;
                series.BorderWidth = 3;
                series.IsValueShownAsLabel = true;
                series.LabelFormat = "{0:F1}%";
                scoreChart.Series.Add(series);

                // Add legend
                Legend legend = new Legend("ScoreLegend");
                legend.Docking = Docking.Top;
                legend.Alignment = StringAlignment.Center;
                scoreChart.Legends.Add(legend);

                // Add title
                Title title = new Title("Quiz Score Progress", Docking.Top, new Font("Arial", 16, FontStyle.Bold), Color.Black);
                scoreChart.Titles.Add(title);

                // Add to form
                parentForm.Controls.Add(scoreChart);

                return scoreChart;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating chart: {ex.Message}", "Chart Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public static void LoadCategories(ComboBox cmbCategory, Label lblStatsSummary, string baseDirectory)
        {
            try
            {
                // Clear existing items
                cmbCategory.Items.Clear();

                // Load actual categories from your data directory
                if (!string.IsNullOrEmpty(baseDirectory) && Directory.Exists(baseDirectory))
                {
                    string[] categoryDirectories = Directory.GetDirectories(baseDirectory);

                    foreach (string categoryDir in categoryDirectories)
                    {
                        string categoryName = Path.GetFileName(categoryDir);

                        // Check if this category has both flashcard data AND quiz statistics
                        string dataJsonPath = Path.Combine(categoryDir, "data.json");
                        string quizStatsPath = Path.Combine(categoryDir, "quiz_statistics.json");

                        if (File.Exists(dataJsonPath))
                        {
                            // Add all categories that have flashcards, even if no quiz stats yet
                            cmbCategory.Items.Add(categoryName);
                        }
                    }

                    if (cmbCategory.Items.Count == 0)
                    {
                        lblStatsSummary.Text = "No categories found. Create some flashcard categories first!";
                    }
                    else
                    {
                        lblStatsSummary.Text = "Select a category to view quiz statistics";
                    }
                }
                else
                {
                    lblStatsSummary.Text = "No data directory found. Please check your application setup.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void LoadCategoryStatistics(string categoryName, string baseDirectory,
            StatisticsManager statisticsManager, Chart scoreChart, Label lblStatsSummary,
            Action<string, int, double, double> updateSummaryCallback)
        {
            try
            {
                string categoryPath = Path.Combine(baseDirectory, categoryName);
                List<QuizResult> quizResults = statisticsManager.GetQuizResultsForCategory(categoryPath);

                if (quizResults == null || quizResults.Count == 0)
                {
                    // Show message that no quiz data exists yet, but don't show error dialog
                    lblStatsSummary.Text = $"No quiz statistics yet for '{categoryName}'. Take a quiz to see your progress!";

                    // Clear the chart
                    if (scoreChart != null && scoreChart.Series.Count > 0)
                    {
                        scoreChart.Series[0].Points.Clear();
                    }
                    return;
                }

                // Clear existing data
                if (scoreChart != null && scoreChart.Series.Count > 0)
                {
                    scoreChart.Series[0].Points.Clear();

                    // Add real data points, sorted by date
                    var sortedResults = quizResults.OrderBy(r => r.Date).ToList();

                    foreach (var result in sortedResults)
                    {
                        scoreChart.Series[0].Points.AddXY(result.Date, result.Score);
                    }

                    // Calculate and display summary statistics
                    double averageScore = statisticsManager.GetAverageScore(categoryPath);
                    double bestScore = statisticsManager.GetBestScore(categoryPath);
                    int totalQuizzes = quizResults.Count;

                    // Call the callback to update summary
                    updateSummaryCallback?.Invoke(categoryName, totalQuizzes, averageScore, bestScore);

                    // Adjust chart area if we have data
                    if (sortedResults.Count > 0)
                    {
                        // Set X-axis range based on data
                        var minDate = sortedResults.First().Date.AddDays(-1);
                        var maxDate = sortedResults.Last().Date.AddDays(1);

                        scoreChart.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
                        scoreChart.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading category statistics: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}