namespace DotBase
{
    partial class TranslacjaForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.polskiBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.angielskiBox = new System.Windows.Forms.TextBox();
            this.tlumaczRadio = new System.Windows.Forms.RadioButton();
            this.brakRadio = new System.Windows.Forms.RadioButton();
            this.zapamietajCheck = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nie można przetłumaczyć:";
            // 
            // polskiBox
            // 
            this.polskiBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.polskiBox.Location = new System.Drawing.Point(202, 12);
            this.polskiBox.Name = "polskiBox";
            this.polskiBox.ReadOnly = true;
            this.polskiBox.Size = new System.Drawing.Size(524, 22);
            this.polskiBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.angielskiBox);
            this.groupBox1.Controls.Add(this.tlumaczRadio);
            this.groupBox1.Controls.Add(this.brakRadio);
            this.groupBox1.Location = new System.Drawing.Point(12, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(714, 83);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Co zrobić?";
            // 
            // angielskiBox
            // 
            this.angielskiBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.angielskiBox.Enabled = false;
            this.angielskiBox.Location = new System.Drawing.Point(157, 53);
            this.angielskiBox.Name = "angielskiBox";
            this.angielskiBox.Size = new System.Drawing.Size(551, 22);
            this.angielskiBox.TabIndex = 3;
            this.angielskiBox.TextChanged += new System.EventHandler(this.somethingChanged);
            // 
            // tlumaczRadio
            // 
            this.tlumaczRadio.AutoSize = true;
            this.tlumaczRadio.Location = new System.Drawing.Point(6, 53);
            this.tlumaczRadio.Name = "tlumaczRadio";
            this.tlumaczRadio.Size = new System.Drawing.Size(130, 21);
            this.tlumaczRadio.TabIndex = 1;
            this.tlumaczRadio.TabStop = true;
            this.tlumaczRadio.Text = "Przetłumacz na:";
            this.tlumaczRadio.UseVisualStyleBackColor = true;
            this.tlumaczRadio.CheckedChanged += new System.EventHandler(this.somethingChanged);
            // 
            // brakRadio
            // 
            this.brakRadio.AutoSize = true;
            this.brakRadio.Location = new System.Drawing.Point(6, 26);
            this.brakRadio.Name = "brakRadio";
            this.brakRadio.Size = new System.Drawing.Size(102, 21);
            this.brakRadio.TabIndex = 0;
            this.brakRadio.TabStop = true;
            this.brakRadio.Text = "Nie tłumacz";
            this.brakRadio.UseVisualStyleBackColor = true;
            this.brakRadio.CheckedChanged += new System.EventHandler(this.somethingChanged);
            // 
            // zapamietajCheck
            // 
            this.zapamietajCheck.AutoSize = true;
            this.zapamietajCheck.Enabled = false;
            this.zapamietajCheck.Location = new System.Drawing.Point(12, 132);
            this.zapamietajCheck.Name = "zapamietajCheck";
            this.zapamietajCheck.Size = new System.Drawing.Size(167, 21);
            this.zapamietajCheck.TabIndex = 3;
            this.zapamietajCheck.Text = "Zapamiętaj mój wybór";
            this.zapamietajCheck.UseVisualStyleBackColor = true;
            this.zapamietajCheck.CheckedChanged += new System.EventHandler(this.somethingChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(276, 167);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(187, 36);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // TranslacjaForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 215);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.zapamietajCheck);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.polskiBox);
            this.Controls.Add(this.label1);
            this.Name = "TranslacjaForm";
            this.Text = "Tłumaczenie słowa";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox polskiBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox angielskiBox;
        private System.Windows.Forms.RadioButton tlumaczRadio;
        private System.Windows.Forms.RadioButton brakRadio;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox zapamietajCheck;
    }
}