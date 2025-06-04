using projekt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Studdybuddy
{
    /// <summary>
    /// The main class for managing flashcards in the study app
    /// Handles all the file operations, JSON serialization, and basic business logic
    /// for creating, loading, and organizing flashcards by category
    /// </summary>
    public class FlashCardManager
    {
        public string BasePath { get; set; }
        public string CurrentCategory { get; set; }
        public FlashCardManager()
        {
            BasePath = BasePath ?? "data"; //hasnt been set yet? -> data
            Directory.CreateDirectory(BasePath);
        }

        /// <summary>
        /// Sets the active category for flashcard operations
        /// Creates the category folder if it doesn't exist yet
        /// </summary>
        /// <param name="categoryName">Name of the category to work with</param>
        /// <exception cref="ArgumentNullException">Thrown when category name is empty or null</exception>
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

        /// <summary>
        /// Saves a new flashcard or updates an existing one
        /// Copies the image file to the category folder and updates the JSON data file
        /// If a flashcard with the same image already exists, it just updates the answer
        /// </summary>
        /// <param name="originalImagePath">Full path to the source image file</param>
        /// <param name="answer">The answer text for this flashcard</param>
        /// <exception cref="InvalidOperationException">Thrown when no category is set</exception>
        public void SaveFlashcard(string originalImagePath, string answer)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            Directory.CreateDirectory(categoryPath);

            string imageFileName = Path.GetFileName(originalImagePath);
            string destImagePath = Path.Combine(categoryPath, imageFileName);

            // Copy image only if it doesn't already exist in our folder removes duplicates
            if (!File.Exists(destImagePath))
            {
                File.Copy(originalImagePath, destImagePath);
            }

            string jsonPath = Path.Combine(categoryPath, "data.json");

            List<Flashcard> flashcards = new List<Flashcard>();
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                flashcards = JsonSerializer.Deserialize<List<Flashcard>>(json) ?? new List<Flashcard>();  //try to deserialize if fail return null
            }

            var existingFlashcard = flashcards.FirstOrDefault(f => f.ImagePath == imageFileName);
            if (existingFlashcard != null)
            {

                existingFlashcard.Answer = answer;
            }
            else
            {
                flashcards.Add(new Flashcard(imageFileName, answer));
            }

           
            string newJson = JsonSerializer.Serialize(flashcards, new JsonSerializerOptions { WriteIndented = true });//prettyier format
            File.WriteAllText(jsonPath, newJson);
        }

        /// <summary>
        /// Loads both the image and answer for a specific flashcard
        /// Returns a bitmap with the image and the answer text.
        /// </summary>
        /// <param name="flashcard">The flashcard to load details for</param>
        /// <returns>image and answer</returns>
        public FlashcardDetails LoadFlashcardDetails(Flashcard flashcard)
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

            return new FlashcardDetails(image, flashcard.Answer);
        }

        /// <summary>
        /// Loads all flashcards for the current category from the JSON file
        /// </summary>
        /// <returns>List of flashcards in the current category</returns>
        /// <exception cref="InvalidOperationException">Thrown when no category is set</exception>
        public List<Flashcard> LoadFlashcards()
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            string jsonPath = Path.Combine(categoryPath, "data.json");

            if (!File.Exists(jsonPath))
                return new List<Flashcard>();

            string json = File.ReadAllText(jsonPath);
            var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(json) ?? new List<Flashcard>(); //deserialize json if not return empty lsit

            return flashcards;
        }

        /// <summary>
        /// Gets a list of all available category names by looking at the folder 
        /// Creates the base directory if it doesn't exist, kinda have no idea how or when i'v done this
        /// </summary>
        /// <returns>Array of category names</returns>
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
        /// import flashcards from a folder full of images
        /// Uses the filename as the answer for each image
        /// </summary>
        /// <param name="folderPath">Path to folder containing images to import</param>
        /// <returns>Updated list of flashcards after import</returns>
        /// <exception cref="InvalidOperationException">Thrown when no category is set</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the source folder doesn't exist</exception>
        public List<Flashcard> LoadFlashcardsFromFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

            // Get all image files from the folder
            string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".bmp" };
            string[] imageFiles = Directory.GetFiles(folderPath, "*.*")
                .Where(file => supportedExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            //create dir
            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            Directory.CreateDirectory(categoryPath);

          
            string jsonPath = Path.Combine(categoryPath, "data.json");
            List<Flashcard> flashcards = new List<Flashcard>();
            if (File.Exists(jsonPath))
            {
                string jsonContent = File.ReadAllText(jsonPath);
                flashcards = JsonSerializer.Deserialize<List<Flashcard>>(jsonContent) ?? new List<Flashcard>();
            }

            foreach (string imagePath in imageFiles)
            {
                string fileName = Path.GetFileName(imagePath);

                bool flashcardExists = flashcards.Any(f => f.ImagePath == fileName);
                if (flashcardExists)
                    continue;

                string destinationPath = Path.Combine(categoryPath, fileName);
                if (!File.Exists(destinationPath))
                {
                    File.Copy(imagePath, destinationPath);
                }

                string answer = Path.GetFileNameWithoutExtension(imagePath);
                flashcards.Add(new Flashcard(fileName, answer));
            }

            string updatedJson = JsonSerializer.Serialize(flashcards, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, updatedJson);

            return flashcards;
        }

        /// <summary>
        /// Completely removes a flashcard from the category.
        /// Deletes both the JSON entry and the actual image file from disk.
        /// </summary>
        /// <param name="flashcardToDelete">The flashcard to remove</param>
        /// <exception cref="InvalidOperationException">Thrown when no category is set or flashcard not found</exception>
        /// <exception cref="ArgumentNullException">Thrown when flashcard parameter is null</exception>
        public void DeleteFlashcard(Flashcard flashcardToDelete)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            if (flashcardToDelete == null)
                throw new ArgumentNullException(nameof(flashcardToDelete));

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            string jsonPath = Path.Combine(categoryPath, "data.json");

            List<Flashcard> flashcards = LoadFlashcards();

          
            var flashcard = flashcards.FirstOrDefault(f => f.ImagePath == flashcardToDelete.ImagePath);
            if (flashcard != null)
            {
                flashcards.Remove(flashcard);

                string updatedJson = JsonSerializer.Serialize(flashcards, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, updatedJson);

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
        /// Quick check to see if a file is a valid image that we can work with.
        /// </summary>
        /// <param name="filePath">Path to the file to check</param>
        /// <returns>True if it looks like a valid image file</returns>
        public bool IsValidImageFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp";
        }

        /// <summary>
        /// Validates flashcard data before saving to catch common errors early.
        /// Checks for missing category, image file, and answer text.
        /// </summary>
        /// <param name="imagePath">Path to the image file</param>
        /// <param name="answer">Answer text</param>
        /// <returns>Tuple with validation result and error message</returns>
        public ValidationResult ValidateFlashcardData(string imagePath, string answer)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                return new ValidationResult(false, "No category selected. Please select or create a category first.");

            if (string.IsNullOrWhiteSpace(imagePath))
                return new ValidationResult(false, "Please upload an image.");

            if (!IsValidImageFile(imagePath))
                return new ValidationResult(false, "Invalid image file. Please select a valid image file (JPG, PNG, BMP).");

            if (string.IsNullOrWhiteSpace(answer))
                return new ValidationResult(false, "Please enter an answer.");

            return new ValidationResult(true);
        }

        /// <summary>
        /// Gets the full file system path to a flashcard's image file.
        /// </summary>
        /// <param name="flashcard">The flashcard to get the image path for</param>
        /// <returns>Full path to the image file</returns>
        /// <exception cref="InvalidOperationException">Thrown when no category is set</exception>
        public string GetImagePath(Flashcard flashcard)
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
                throw new InvalidOperationException("Category is not set");

            string categoryPath = Path.Combine(BasePath, CurrentCategory);
            return Path.Combine(categoryPath, flashcard.ImagePath);
        }

        /// <summary>
        /// Gets the full path to the current category's folder.
        /// </summary>
        /// <returns>Full path to the current category folder</returns>
        /// <exception cref="InvalidOperationException">Thrown when no category is currently selected</exception>
        public string GetCategoryPath()
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
            {
                throw new InvalidOperationException("No category is currently selected.");
            }

            return Path.Combine(BasePath, CurrentCategory);
        }
    }
}