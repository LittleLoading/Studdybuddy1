using projekt;
using Studdybuddy;
using System.Text.Json;
using static projekt.Flashcard;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace projekt
{
    /// <summary>
    /// handles user interactions and displays flashcards
    /// This form lets users create, view, edit and delete flashcards organized by categories
    /// </summary>
    public partial class Form1 : Form
    {
        FlashCardManager flashcardManager = new FlashCardManager();
        private List<Flashcard> flashcards = new List<Flashcard>();
        private string currentImagePath = "";
        private string currentCategory = "";

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        /// <summary>
        /// Sets up the initial state of the UI when form loads
        /// Loads categories and resets everything to default
        /// </summary>
        private void InitializeUI()
        {
            LoadCategoriesToDropdown();
            ResetUI();
        }

        /// <summary>
        /// Resets all UI elements to their default empty state
        /// </summary>
        private void ResetUI()
        {
            lblCurrentCategory.Text = "No category selected";
            listBox1.Items.Clear();
            pictureBox1.Image = null;
            textBoxAnswer.Text = "";
            currentImagePath = "";
        }

        /// <summary>
        /// Handles the upload image button click
        /// Opens file dialog to let user select an image file for a flashcard
        /// </summary>
        private void buttonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Check if the selected file is a valid image
                if (flashcardManager.IsValidImageFile(dialog.FileName))
                {
                    currentImagePath = dialog.FileName;
                    pictureBox1.Image = Image.FromFile(currentImagePath);
                }
                else
                {
                    MessageBox.Show("Invalid image file selected.");
                }
            }
        }

        /// <summary>
        /// Saves a new flashcard with the uploaded image and entered answer
        /// Validates data first, then saves if everything is ok
        /// </summary>
        private void buttonSaveFlashcard_Click(object sender, EventArgs e)
        {
            ValidationResult result = flashcardManager.ValidateFlashcardData(currentImagePath, textBoxAnswer.Text.Trim());
            if (!result.IsValid)
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }

            try
            {
                flashcardManager.SaveFlashcard(currentImagePath, textBoxAnswer.Text.Trim());
                RefreshFlashcardList();
                ClearFlashcardInputs();
                MessageBox.Show("Flashcard saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving flashcard: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles when user selects a flashcard from the list
        /// Loads and displays the selected flashcard's image and answer
        /// </summary>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Flashcard selectedFlashcard)
            {
                try
                {
                    FlashcardDetails details = flashcardManager.LoadFlashcardDetails(selectedFlashcard);
                    pictureBox1.Image = details.Image;
                    textBoxAnswer.Text = details.Answer;
                    currentImagePath = flashcardManager.GetImagePath(selectedFlashcard);

                    if (details.Image == null)
                    {
                        MessageBox.Show("Image file not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading flashcard: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Bulk loads flashcards from a selected folder
        /// Creates flashcards automatically using image filenames as answers
        /// </summary>
        private void buttonLoadFromFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var loadedFlashcards = flashcardManager.LoadFlashcardsFromFolder(fbd.SelectedPath);

                        flashcards.AddRange(loadedFlashcards);

                        foreach (var flashcard in loadedFlashcards)
                        {
                            listBox1.Items.Add(flashcard);
                        }

                        MessageBox.Show($"Loaded {loadedFlashcards.Count} flashcards from folder.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading flashcards from folder: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Opens the category creation form to create a new 
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            using (var form = new CategoryForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string categoryName = form.CategoryName;

                    try
                    {
                        flashcardManager.SetCategory(categoryName);
                        lblCurrentCategory.Text = $"Current category: {categoryName}";
                        LoadCategoriesToDropdown(); 
                        RefreshFlashcardList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating category: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Loads all available categories into the dropdown 
        /// </summary>
        private void LoadCategoriesToDropdown()
        {
            comboBoxCategories.Items.Clear();
            comboBoxCategories.Items.Add("None"); // Default option

            try
            {

                string[] categories = flashcardManager.GetAvailableCategories();
                comboBoxCategories.Items.AddRange(categories);
                comboBoxCategories.SelectedIndex = 0; //Select none defaultne
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles category selection from dropdown
        /// Switches to selected category and loads its flashcards
        /// </summary>
        private void comboBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCategories.SelectedItem is string selectedCategory)
            {
                ClearFlashcardDisplay();

                if (selectedCategory == "None")
                {
                    flashcardManager.CurrentCategory = null;
                    lblCurrentCategory.Text = "No category selected";
                    listBox1.Items.Clear();
                    return;
                }

                try
                {
                    flashcardManager.SetCategory(selectedCategory);
                    lblCurrentCategory.Text = $"Current category: {selectedCategory}";
                    RefreshFlashcardList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error switching category: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Deletes the currently selected flashcard
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Flashcard selectedFlashcard)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to delete this flashcard?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo
                );

                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        flashcardManager.DeleteFlashcard(selectedFlashcard);
                        RefreshFlashcardList();
                        ClearFlashcardDisplay();
                        MessageBox.Show("Flashcard deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while deleting flashcard: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a flashcard to delete.");
            }
        }

        /// <summary>
        /// Refreshes the flashcard list display
        /// Reloads all flashcards from current category and updates listbox
        /// </summary>
        private void RefreshFlashcardList()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(flashcardManager.CurrentCategory))
                {
                    var loadedFlashcards = flashcardManager.LoadFlashcards();
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(loadedFlashcards.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing flashcard list: " + ex.Message);
            }
        }

        /// <summary>
        /// Clears the input fields after saving a flashcard
        /// Resets image, path and answer text
        /// </summary>
        private void ClearFlashcardInputs()
        {
            pictureBox1.Image = null;
            currentImagePath = "";
            textBoxAnswer.Text = "";
        }

        /// <summary>
        /// Clears the flashcard display area
        /// Used when switching categories or after deletion
        /// </summary>
        private void ClearFlashcardDisplay()
        {
            listBox1.ClearSelected();
            pictureBox1.Image = null;
            textBoxAnswer.Clear();
            currentImagePath = "";
        }

        /// <summary>
        /// Opens the quiz form to start a quiz with current flashcards
        /// Passes the flashcard manager to the quiz form
        /// </summary>
        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            var quizForm = new QuizForm(flashcardManager);
            quizForm.Show();
        }
    }
}