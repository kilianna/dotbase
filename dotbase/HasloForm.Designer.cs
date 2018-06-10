namespace DotBase
{
    partial class HasloForm
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button anulujBtn;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            this.uzytkownikLabel = new System.Windows.Forms.Label();
            this.nazwaTextBox = new System.Windows.Forms.TextBox();
            this.hasloTextBox = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.hasloPowtTextBox = new System.Windows.Forms.TextBox();
            this.hasloAktTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            anulujBtn = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uzytkownikLabel
            // 
            this.uzytkownikLabel.AutoSize = true;
            this.uzytkownikLabel.Location = new System.Drawing.Point(12, 26);
            this.uzytkownikLabel.Name = "uzytkownikLabel";
            this.uzytkownikLabel.Size = new System.Drawing.Size(65, 13);
            this.uzytkownikLabel.TabIndex = 0;
            this.uzytkownikLabel.Text = "Użytkownik:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 78);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(68, 13);
            label2.TabIndex = 4;
            label2.Text = "Nowe hasło:";
            // 
            // anulujBtn
            // 
            anulujBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            anulujBtn.Location = new System.Drawing.Point(162, 142);
            anulujBtn.Name = "anulujBtn";
            anulujBtn.Size = new System.Drawing.Size(75, 23);
            anulujBtn.TabIndex = 9;
            anulujBtn.Text = "Anuluj";
            anulujBtn.UseVisualStyleBackColor = true;
            anulujBtn.Click += new System.EventHandler(this.anulujBtn_Click);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 104);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(78, 13);
            label3.TabIndex = 6;
            label3.Text = "Powtórz hasło:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(26, 184);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(244, 26);
            label4.TabIndex = 10;
            label4.Text = "Hasło musi być niekrótsze niż 8 znaków, zawierać\r\nco najmniej jedną literę i co n" +
                "ajmniej jendą cyfrę.";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(12, 52);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(82, 13);
            label5.TabIndex = 2;
            label5.Text = "Aktualne hasło:";
            // 
            // nazwaTextBox
            // 
            this.nazwaTextBox.Enabled = false;
            this.nazwaTextBox.Location = new System.Drawing.Point(100, 23);
            this.nazwaTextBox.Name = "nazwaTextBox";
            this.nazwaTextBox.Size = new System.Drawing.Size(184, 20);
            this.nazwaTextBox.TabIndex = 1;
            // 
            // hasloTextBox
            // 
            this.hasloTextBox.Location = new System.Drawing.Point(100, 75);
            this.hasloTextBox.Name = "hasloTextBox";
            this.hasloTextBox.Size = new System.Drawing.Size(184, 20);
            this.hasloTextBox.TabIndex = 5;
            this.hasloTextBox.UseSystemPasswordChar = true;
            this.hasloTextBox.TextChanged += new System.EventHandler(this.hasloTextBox_TextChanged);
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(59, 142);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 8;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // hasloPowtTextBox
            // 
            this.hasloPowtTextBox.Location = new System.Drawing.Point(100, 101);
            this.hasloPowtTextBox.Name = "hasloPowtTextBox";
            this.hasloPowtTextBox.Size = new System.Drawing.Size(184, 20);
            this.hasloPowtTextBox.TabIndex = 7;
            this.hasloPowtTextBox.UseSystemPasswordChar = true;
            this.hasloPowtTextBox.TextChanged += new System.EventHandler(this.hasloTextBox_TextChanged);
            // 
            // hasloAktTextBox
            // 
            this.hasloAktTextBox.Location = new System.Drawing.Point(100, 49);
            this.hasloAktTextBox.Name = "hasloAktTextBox";
            this.hasloAktTextBox.Size = new System.Drawing.Size(184, 20);
            this.hasloAktTextBox.TabIndex = 3;
            this.hasloAktTextBox.UseSystemPasswordChar = true;
            this.hasloAktTextBox.TextChanged += new System.EventHandler(this.hasloTextBox_TextChanged);
            // 
            // HasloForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = anulujBtn;
            this.ClientSize = new System.Drawing.Size(296, 227);
            this.Controls.Add(this.hasloAktTextBox);
            this.Controls.Add(label5);
            this.Controls.Add(label4);
            this.Controls.Add(this.hasloPowtTextBox);
            this.Controls.Add(label3);
            this.Controls.Add(anulujBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.hasloTextBox);
            this.Controls.Add(label2);
            this.Controls.Add(this.nazwaTextBox);
            this.Controls.Add(this.uzytkownikLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HasloForm";
            this.Text = "Zmiana hasła";
            this.Shown += new System.EventHandler(this.HasloForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nazwaTextBox;
        private System.Windows.Forms.TextBox hasloTextBox;
        private System.Windows.Forms.TextBox hasloPowtTextBox;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.TextBox hasloAktTextBox;
        private System.Windows.Forms.Label uzytkownikLabel;
    }
}