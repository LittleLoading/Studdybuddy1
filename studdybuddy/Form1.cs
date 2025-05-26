using projekt;

using static projekt.Flashcard;

namespace projekt

{

    public partial class Form1 : Form

    {

        private List<Flashcard> flashcards = new List<Flashcard>();

        private string currentImagePath = "";



        public Form1()

        {

            InitializeComponent();

        }



        private void buttonUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentImagePath = ofd.FileName;

                    // Uvolní pøedchozí obrázek, pokud existuje
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                    }

                    // Bezpeèné naètení obrázku
                    using (var img = Image.FromFile(currentImagePath))
                    {
                        pictureBox1.Image = new Bitmap(img);
                    }
                }
            }
        }




        private void buttonSaveFlashcard_Click(object sender, EventArgs e)

        {

            if (string.IsNullOrEmpty(currentImagePath))

            {

                MessageBox.Show("Please upload an image.");

                return;

            }

            if (string.IsNullOrWhiteSpace(textBoxAnswer.Text))

            {

                MessageBox.Show("Please enter an answer.");

                return;

            }



            Flashcard fc = new Flashcard(currentImagePath, textBoxAnswer.Text.Trim());

            flashcards.Add(fc);

            listBox1.Items.Add(fc);



            // Reset for next entry

            pictureBox1.Image = null;

            currentImagePath = "";

            textBoxAnswer.Text = "";

        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)

        {

            if (listBox1.SelectedItem is Flashcard selectedFlashcard)

            {

                // Load image

                if (File.Exists(selectedFlashcard.ImagePath))

                {

                    pictureBox1.Image = Image.FromFile(selectedFlashcard.ImagePath);

                }

                else

                {

                    pictureBox1.Image = null;

                    MessageBox.Show("Image file not found.");

                }



                // Load answer

                textBoxAnswer.Text = selectedFlashcard.Answer;

                // Optionally, set currentImagePath if you want to allow editing

                currentImagePath = selectedFlashcard.ImagePath;

            }

        }



        private void pictureBox1_Click(object sender, EventArgs e)

        {



        }



        private void buttonLoadFromFolder_Click(object sender, EventArgs e)

        {

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())

            {

                if (fbd.ShowDialog() == DialogResult.OK)

                {

                    string folderPath = fbd.SelectedPath;

                    string[] imageFiles = Directory.GetFiles(folderPath, "*.*")

                        .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)

                                 || f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)

                                 || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)

                                 || f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))

                        .ToArray();



                    foreach (string imagePath in imageFiles)

                    {

                        string fileName = Path.GetFileNameWithoutExtension(imagePath);

                        Flashcard fc = new Flashcard(imagePath, fileName);

                        flashcards.Add(fc);

                        listBox1.Items.Add(fc);

                    }



                    MessageBox.Show($"Loaded {imageFiles.Length} flashcards from folder.");

                }

            }

        }

    }

}