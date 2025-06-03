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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                string input = txtCategory.Text.Trim();

                // Remove or replace invalid characters
                foreach (char c in Path.GetInvalidFileNameChars())
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
