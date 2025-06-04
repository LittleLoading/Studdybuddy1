using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Studdybuddy
{
    public class StatisticsManager
    {
        private const string STATISTICS_FILE_NAME = "quiz_statistics.json";

        /// <summary>
        /// Saves a quiz result to the statistics file for the specified category
        /// </summary>
        public void SaveQuizResult(QuizResult quizResult, string categoryPath)
        {
            try
            {
                if (quizResult == null)
                    throw new ArgumentNullException(nameof(quizResult));

                if (string.IsNullOrWhiteSpace(categoryPath))
                    throw new ArgumentException("Category path cannot be null or empty", nameof(categoryPath));

                // Ensure the category directory exists
                Directory.CreateDirectory(categoryPath);

                string statisticsFilePath = Path.Combine(categoryPath, STATISTICS_FILE_NAME);

                // Load existing statistics or create new
                QuizStatistics statistics = LoadStatisticsFromFile(statisticsFilePath);

                // Add the new result
                statistics.QuizResults.Add(quizResult);

                // Save back to file
                SaveStatisticsToFile(statisticsFilePath, statistics);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save quiz result: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all quiz results for a specific category
        /// </summary>
        public List<QuizResult> GetQuizResultsForCategory(string categoryPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryPath))
                    return new List<QuizResult>();

                string statisticsFilePath = Path.Combine(categoryPath, STATISTICS_FILE_NAME);

                if (!File.Exists(statisticsFilePath))
                    return new List<QuizResult>();

                QuizStatistics statistics = LoadStatisticsFromFile(statisticsFilePath);
                return statistics.QuizResults ?? new List<QuizResult>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load quiz results: {ex.Message}", ex);
            }
        }

       

        /// <summary>
        /// Gets the average score for a category
        /// </summary>
        public double GetAverageScore(string categoryPath)
        {
            var results = GetQuizResultsForCategory(categoryPath);
            return results.Count > 0 ? results.Average(r => r.Score) : 0.0;
        }

        /// <summary>
        /// Gets the best score for a category
        /// </summary>
        public double GetBestScore(string categoryPath)
        {
            var results = GetQuizResultsForCategory(categoryPath);
            return results.Count > 0 ? results.Max(r => r.Score) : 0.0;
        }

        /// <summary>
        /// Gets the total number of quizzes taken for a category
        /// </summary>
        public int GetTotalQuizzesTaken(string categoryPath)
        {
            var results = GetQuizResultsForCategory(categoryPath);
            return results.Count;
        }


        /// <summary>
        /// Loads statistics from a JSON file
        /// </summary>
        private QuizStatistics LoadStatisticsFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new QuizStatistics();

                string jsonContent = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(jsonContent))
                    return new QuizStatistics();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<QuizStatistics>(jsonContent, options) ?? new QuizStatistics();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Invalid JSON format in statistics file: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load statistics file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves statistics to a JSON file
        /// </summary>
        private void SaveStatisticsToFile(string filePath, QuizStatistics statistics)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                string jsonContent = JsonSerializer.Serialize(statistics, options);
                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save statistics file: {ex.Message}", ex);
            }
        }

        
    }
}