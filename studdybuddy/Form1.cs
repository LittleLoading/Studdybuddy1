using projekt;
using Studdybuddy;
using System.Text.Json;
using static projekt.Flashcard;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace projekt
{
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

        private void InitializeUI()
        {
            LoadCategoriesToDropdown();
            ResetUI();
        }

        private void ResetUI()
        {
            lblCurrentCategory.Text = "No category selected";
            listBox1.Items.Clear();
            pictureBox1.Image = null;
            textBoxAnswer.Text = "";
            currentImagePath = "";
        }

        private void buttonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
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

        private void buttonSaveFlashcard_Click(object sender, EventArgs e)
        {
            var (isValid, errorMessage) = flashcardManager.ValidateFlashcardData(currentImagePath, textBoxAnswer.Text.Trim());

            if (!isValid)
            {
                MessageBox.Show(errorMessage);
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Flashcard selectedFlashcard)
            {
                try
                {
                    var (image, answer) = flashcardManager.LoadFlashcardDetails(selectedFlashcard);

                    pictureBox1.Image = image;
                    textBoxAnswer.Text = answer;
                    currentImagePath = flashcardManager.GetImagePath(selectedFlashcard);

                    if (image == null)
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
            // Reserved for future functionality
        }

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
                        LoadCategoriesToDropdown(); // Refresh dropdown
                        RefreshFlashcardList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating category: " + ex.Message);
                    }
                }
            }
        }

        private void LoadCategoriesToDropdown()
        {
            comboBoxCategories.Items.Clear();
            comboBoxCategories.Items.Add("None"); // Add default item

            try
            {
                string[] categories = flashcardManager.GetAvailableCategories();
                comboBoxCategories.Items.AddRange(categories);
                comboBoxCategories.SelectedIndex = 0; // Select "None" by default
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

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

        // Helper methods for cleaner code
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

        private void ClearFlashcardInputs()
        {
            pictureBox1.Image = null;
            currentImagePath = "";
            textBoxAnswer.Text = "";
        }

        private void ClearFlashcardDisplay()
        {
            listBox1.ClearSelected();
            pictureBox1.Image = null;
            textBoxAnswer.Clear();
            currentImagePath = "";
        }
    }
}