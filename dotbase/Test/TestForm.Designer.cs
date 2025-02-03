namespace DotBase.Test
{
    partial class TestForm
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
            this.swiadHtmlVsDocxButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // swiadHtmlVsDocxButton
            // 
            this.swiadHtmlVsDocxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.swiadHtmlVsDocxButton.Location = new System.Drawing.Point(12, 12);
            this.swiadHtmlVsDocxButton.Name = "swiadHtmlVsDocxButton";
            this.swiadHtmlVsDocxButton.Size = new System.Drawing.Size(258, 32);
            this.swiadHtmlVsDocxButton.TabIndex = 0;
            this.swiadHtmlVsDocxButton.Text = "Swiad. HTML vs DOCX";
            this.swiadHtmlVsDocxButton.UseVisualStyleBackColor = true;
            this.swiadHtmlVsDocxButton.Click += new System.EventHandler(this.swiadHtmlVsDocxButton_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.swiadHtmlVsDocxButton);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button swiadHtmlVsDocxButton;
    }
}