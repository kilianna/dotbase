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
            this.textHtmlDocxFrom = new System.Windows.Forms.TextBox();
            this.textHtmlDocxTo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkHtmlDocxForce = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // swiadHtmlVsDocxButton
            // 
            this.swiadHtmlVsDocxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.swiadHtmlVsDocxButton.Location = new System.Drawing.Point(15, 35);
            this.swiadHtmlVsDocxButton.Name = "swiadHtmlVsDocxButton";
            this.swiadHtmlVsDocxButton.Size = new System.Drawing.Size(241, 32);
            this.swiadHtmlVsDocxButton.TabIndex = 5;
            this.swiadHtmlVsDocxButton.Text = "Swiad. HTML vs DOCX";
            this.swiadHtmlVsDocxButton.UseVisualStyleBackColor = true;
            this.swiadHtmlVsDocxButton.Click += new System.EventHandler(this.swiadHtmlVsDocxButton_Click);
            // 
            // textHtmlDocxFrom
            // 
            this.textHtmlDocxFrom.Location = new System.Drawing.Point(185, 6);
            this.textHtmlDocxFrom.Name = "textHtmlDocxFrom";
            this.textHtmlDocxFrom.Size = new System.Drawing.Size(71, 22);
            this.textHtmlDocxFrom.TabIndex = 3;
            // 
            // textHtmlDocxTo
            // 
            this.textHtmlDocxTo.Location = new System.Drawing.Point(42, 6);
            this.textHtmlDocxTo.Name = "textHtmlDocxTo";
            this.textHtmlDocxTo.Size = new System.Drawing.Size(71, 22);
            this.textHtmlDocxTo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "od";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "w dół do";
            // 
            // checkHtmlDocxForce
            // 
            this.checkHtmlDocxForce.AutoSize = true;
            this.checkHtmlDocxForce.Location = new System.Drawing.Point(262, 8);
            this.checkHtmlDocxForce.Name = "checkHtmlDocxForce";
            this.checkHtmlDocxForce.Size = new System.Drawing.Size(230, 21);
            this.checkHtmlDocxForce.TabIndex = 4;
            this.checkHtmlDocxForce.Text = "załącz przetestowane wcześniej";
            this.checkHtmlDocxForce.UseVisualStyleBackColor = true;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 253);
            this.Controls.Add(this.checkHtmlDocxForce);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textHtmlDocxTo);
            this.Controls.Add(this.textHtmlDocxFrom);
            this.Controls.Add(this.swiadHtmlVsDocxButton);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button swiadHtmlVsDocxButton;
        private System.Windows.Forms.TextBox textHtmlDocxFrom;
        private System.Windows.Forms.TextBox textHtmlDocxTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkHtmlDocxForce;
    }
}