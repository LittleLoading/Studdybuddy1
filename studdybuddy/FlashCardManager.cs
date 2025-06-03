using projekt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Studdybuddy
{
    internal class FlashCardManager
    {
        public string BasePath { get; set; }
        public string CurrentCategory { get; set; }

        public FlashCardManager()
        {
            BasePath = BasePath ?? "data";
            Directory.CreateDirectory(BasePath);
        }

        public void SetCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException("Name of category can't be empty");
            }

            CurrentCategory = categoryName;
            string path = Path.Combine(BasePath, categoryName);
            Directory.CreateDirectory(path);
        }

        public void SaveFlashcard(string originalImagePath, string answer)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            Directory.CreateDirectory(categoryPath);

            string imageFileName = Path.GetFileName(originalImagePath);
            string destImagePath = Path.Combine(categoryPath, imageFileName);

            // Copy image only if it doesn't already exist in our folder
            if (!File.Exists(destImagePath))
            {
                File.Copy(originalImagePath, destImagePath);
            }

            string jsonPath = Path.Combine(categoryPath, "data.json");

            List<Flashcard> flashcards = new List<Flashcard>();
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                flashcards = JsonSerializer.Deserialize<List<Flashcard>>(json) ?? new List<Flashcard>();
            }

            // Check if flashcard with this image already exists
            var existingFlashcard = flashcards.FirstOrDefault(f => f.ImagePath == imageFileName);
            if (existingFlashcard != null)
            {
                // Update the answer if the flashcard exists
                existingFlashcard.Answer = answer;
            }
            else
            {
                // Add a new flashcard
                flashcards.Add(new Flashcard(imageFileName, answer));
            }

            // Save JSON
            string newJson = JsonSerializer.Serialize(flashcards, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, newJson);
        }

        public (Image?, string) LoadFlashcardDetails(Flashcard flashcard)
        {
            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            string fullImagePath = Path.Combine(categoryPath, flashcard.ImagePath);

            Image? image = null;

            if (File.Exists(fullImagePath))
            {
                using (var stream = new FileStream(fullImagePath, FileMode.Open, FileAccess.Read))
                {
                    image = Image.FromStream(stream);
                }
            }

            return (image, flashcard.Answer);
        }

        public List<Flashcard> LoadFlashcards()
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            string jsonPath = Path.Combine(categoryPath, "data.json");

            if (!File.Exists(jsonPath))
                return new List<Flashcard>();

            string json = File.ReadAllText(jsonPath);
            var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(json) ?? new List<Flashcard>();

            return flashcards;
        }

        // NEW METHODS - Moved business logic from Form1

        /// <summary>
        /// Gets all available categories from the file system
        /// </summary>
        public string[] GetAvailableCategories()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            return Directory.GetDirectories(BasePath)
                            .Select(Path.GetFileName)
                            .ToArray();
        }

        /// <summary>
        /// Loads flashcards from a folder containing images, using filename as answer
        /// </summary>
        public List<Flashcard> LoadFlashcardsFromFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

            string[] imageFiles = Directory.GetFiles(folderPath, "*.*")
                .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var flashcards = new List<Flashcard>();
            foreach (string imagePath in imageFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(imagePath);
                flashcards.Add(new Flashcard(imagePath, fileName));
            }

            return flashcards;
        }

        /// <summary>
        /// Deletes a flashcard and its associated image file
        /// </summary>
        public void DeleteFlashcard(Flashcard flashcardToDelete)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            if (flashcardToDelete == null)
                throw new ArgumentNullException(nameof(flashcardToDelete));

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            string jsonPath = Path.Combine(categoryPath, "data.json");

            // Load current flashcards
            List<Flashcard> flashcards = LoadFlashcards();

            // Remove the flashcard from the list
            var flashcard = flashcards.FirstOrDefault(f => f.ImagePath == flashcardToDelete.ImagePath);
            if (flashcard != null)
            {
                flashcards.Remove(flashcard);

                // Save updated list back to JSON
                string updatedJson = JsonSerializer.Serialize(flashcards, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, updatedJson);

                // Delete the image file
                string imagePath = Path.Combine(categoryPath, flashcardToDelete.ImagePath);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            else
            {
                throw new InvalidOperationException("Flashcard not found in current category");
            }
        }

        /// <summary>
        /// Validates if an image file can be uploaded (basic validation)
        /// </summary>
        public bool IsValidImageFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp";
        }

        /// <summary>
        /// Validates flashcard data before saving
        /// </summary>
        public (bool IsValid, string ErrorMessage) ValidateFlashcardData(string imagePath, string answer)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                return (false, "No category selected. Please select or create a category first.");

            if (string.IsNullOrWhiteSpace(imagePath))
                return (false, "Please upload an image.");

            if (!IsValidImageFile(imagePath))
                return (false, "Invalid image file. Please select a valid image file (JPG, PNG, BMP).");

            if (string.IsNullOrWhiteSpace(answer))
                return (false, "Please enter an answer.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Gets the full path for an image in the current category
        /// </summary>
        public string GetImagePath(Flashcard flashcard)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            return Path.Combine(categoryPath, flashcard.ImagePath);
        }

        /// <summary>
        /// Checks if the current category has any flashcards
        /// </summary>
        public bool HasFlashcards()
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                return false;

            return LoadFlashcards().Count > 0;
        }
    }
}