namespace Studdybuddy
{
    partial class CategoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtCategory = new TextBox();
            label1 = new Label();
            btnOK = new Button();
            SuspendLayout();
            // 
            // txtCategory
            // 
            txtCategory.Location = new Point(248, 166);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(250, 23);
            txtCategory.TabIndex = 0;
            //txtCategory.TextChanged += textcat;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(296, 122);
            label1.Name = "label1";
            label1.Size = new Size(156, 15);
            label1.TabIndex = 1;
            label1.Text = "Please Enter Category Name";
            label1.Click += label1_Click;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(338, 195);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // CategoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnOK);
            Controls.Add(label1);
            Controls.Add(txtCategory);
            Name = "CategoryForm";
            Text = "CategoryForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtCategory;
        private Label label1;
        private Button btnOK;
    }
}