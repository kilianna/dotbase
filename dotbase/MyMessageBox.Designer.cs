namespace DotBase
{
    partial class MyMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyMessageBox));
            this.messageBox = new System.Windows.Forms.TextBox();
            this.errorBox = new System.Windows.Forms.PictureBox();
            this.questionBox = new System.Windows.Forms.PictureBox();
            this.infoBox = new System.Windows.Forms.PictureBox();
            this.warningBox = new System.Windows.Forms.PictureBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.retryButton = new System.Windows.Forms.Button();
            this.ignoreButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningBox)).BeginInit();
            this.SuspendLayout();
            // 
            // messageBox
            // 
            this.messageBox.AcceptsReturn = true;
            this.messageBox.AcceptsTab = true;
            this.messageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageBox.Location = new System.Drawing.Point(113, 12);
            this.messageBox.MaxLength = 1048572;
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.ReadOnly = true;
            this.messageBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageBox.Size = new System.Drawing.Size(718, 242);
            this.messageBox.TabIndex = 0;
            this.messageBox.Text = resources.GetString("messageBox.Text");
            // 
            // errorBox
            // 
            this.errorBox.Image = global::DotBase.Properties.Resources.error;
            this.errorBox.Location = new System.Drawing.Point(12, 12);
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(56, 56);
            this.errorBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.errorBox.TabIndex = 1;
            this.errorBox.TabStop = false;
            this.errorBox.Visible = false;
            // 
            // questionBox
            // 
            this.questionBox.Image = global::DotBase.Properties.Resources.question;
            this.questionBox.Location = new System.Drawing.Point(12, 12);
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new System.Drawing.Size(56, 56);
            this.questionBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.questionBox.TabIndex = 2;
            this.questionBox.TabStop = false;
            this.questionBox.Visible = false;
            // 
            // infoBox
            // 
            this.infoBox.Image = global::DotBase.Properties.Resources.info;
            this.infoBox.Location = new System.Drawing.Point(12, 12);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(56, 56);
            this.infoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.infoBox.TabIndex = 3;
            this.infoBox.TabStop = false;
            this.infoBox.Visible = false;
            // 
            // warningBox
            // 
            this.warningBox.Image = global::DotBase.Properties.Resources.warning;
            this.warningBox.Location = new System.Drawing.Point(12, 12);
            this.warningBox.Name = "warningBox";
            this.warningBox.Size = new System.Drawing.Size(56, 56);
            this.warningBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.warningBox.TabIndex = 4;
            this.warningBox.TabStop = false;
            this.warningBox.Visible = false;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(704, 260);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(127, 33);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            // 
            // noButton
            // 
            this.noButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.noButton.Location = new System.Drawing.Point(571, 260);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(127, 33);
            this.noButton.TabIndex = 6;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            this.noButton.Visible = false;
            // 
            // yesButton
            // 
            this.yesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.yesButton.Location = new System.Drawing.Point(438, 260);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(127, 33);
            this.yesButton.TabIndex = 7;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Visible = false;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(571, 260);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(127, 33);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Visible = false;
            // 
            // retryButton
            // 
            this.retryButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.retryButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.retryButton.Location = new System.Drawing.Point(571, 260);
            this.retryButton.Name = "retryButton";
            this.retryButton.Size = new System.Drawing.Size(127, 33);
            this.retryButton.TabIndex = 9;
            this.retryButton.Text = "Retry";
            this.retryButton.UseVisualStyleBackColor = true;
            this.retryButton.Visible = false;
            // 
            // ignoreButton
            // 
            this.ignoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreButton.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.ignoreButton.Location = new System.Drawing.Point(704, 260);
            this.ignoreButton.Name = "ignoreButton";
            this.ignoreButton.Size = new System.Drawing.Size(127, 33);
            this.ignoreButton.TabIndex = 10;
            this.ignoreButton.Text = "Ignore";
            this.ignoreButton.UseVisualStyleBackColor = true;
            this.ignoreButton.Visible = false;
            // 
            // abortButton
            // 
            this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.abortButton.Location = new System.Drawing.Point(438, 260);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(127, 33);
            this.abortButton.TabIndex = 11;
            this.abortButton.Text = "Abort";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Visible = false;
            // 
            // MyMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 305);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.ignoreButton);
            this.Controls.Add(this.retryButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.warningBox);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.questionBox);
            this.Controls.Add(this.errorBox);
            this.Controls.Add(this.messageBox);
            this.Name = "MyMessageBox";
            this.Text = "MyMessageBox";
            ((System.ComponentModel.ISupportInitialize)(this.errorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.PictureBox errorBox;
        private System.Windows.Forms.PictureBox questionBox;
        private System.Windows.Forms.PictureBox infoBox;
        private System.Windows.Forms.PictureBox warningBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button retryButton;
        private System.Windows.Forms.Button ignoreButton;
        private System.Windows.Forms.Button abortButton;
    }
}