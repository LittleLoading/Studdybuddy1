using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Studdybuddy
{
    /// <summary>
    /// A simple dialog form for creating new categories 
    /// This is basically a popup window that asks the user to type in a category name
    /// then does some basic validation to make sure it's usable as a folder name
    /// </summary>
    public partial class CategoryForm : Form
    {

        public string CategoryName { get; set; }
        public CategoryForm()
        {
            InitializeComponent();
            this.Text = "Choose Category";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Handles when the user clicks the OK button to create the category
        /// validates the input, cleans up any problematic
        /// </summary>
        /// <param name="sender">The OK button</param>
        /// <param name="e">Click event details</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                string input = txtCategory.Text.Trim();

                // Clean up the input by replacing characters that would break file system operations
                // Things like slashes, colons, etc. get turned into underscores
                foreach (char c in Path.GetInvalidFileNameChars()) //funny method :D
                {
                    input = input.Replace(c, '_');
                }

                CategoryName = input;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter name of category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {

        }
    }
}