namespace DotBase.Test
{
    partial class TestHtmlControl
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
            this.simpleHtmlTextBox1 = new DotBase.Controls.SimpleHtmlTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // simpleHtmlTextBox1
            // 
            this.simpleHtmlTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleHtmlTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.simpleHtmlTextBox1.Location = new System.Drawing.Point(16, 16);
            this.simpleHtmlTextBox1.Margin = new System.Windows.Forms.Padding(7);
            this.simpleHtmlTextBox1.Name = "simpleHtmlTextBox1";
            this.simpleHtmlTextBox1.Size = new System.Drawing.Size(675, 279);
            this.simpleHtmlTextBox1.TabIndex = 0;
            this.simpleHtmlTextBox1.HtmlChanged += new System.EventHandler(this.simpleHtmlTextBox1_HtmlChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(16, 305);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(675, 142);
            this.textBox1.TabIndex = 1;
            // 
            // TestHtmlControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 459);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.simpleHtmlTextBox1);
            this.Name = "TestHtmlControl";
            this.Text = "TestHtmlControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.SimpleHtmlTextBox simpleHtmlTextBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}