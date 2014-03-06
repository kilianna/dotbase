namespace DotBase
{
    partial class LogowanieForm
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
            this.button1 = new System.Windows.Forms.Button();
            this._Kierownik = new System.Windows.Forms.RadioButton();
            this._PracownikBiurowy = new System.Windows.Forms.RadioButton();
            this._PracownikPomiarowy = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 218);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Zaloguj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _Kierownik
            // 
            this._Kierownik.AutoSize = true;
            this._Kierownik.Checked = true;
            this._Kierownik.Location = new System.Drawing.Point(6, 19);
            this._Kierownik.Name = "_Kierownik";
            this._Kierownik.Size = new System.Drawing.Size(71, 17);
            this._Kierownik.TabIndex = 1;
            this._Kierownik.TabStop = true;
            this._Kierownik.Text = "Kierownik";
            this._Kierownik.UseVisualStyleBackColor = true;
            // 
            // _PracownikBiurowy
            // 
            this._PracownikBiurowy.AutoSize = true;
            this._PracownikBiurowy.Location = new System.Drawing.Point(6, 42);
            this._PracownikBiurowy.Name = "_PracownikBiurowy";
            this._PracownikBiurowy.Size = new System.Drawing.Size(115, 17);
            this._PracownikBiurowy.TabIndex = 2;
            this._PracownikBiurowy.Text = "Pracownik Biurowy";
            this._PracownikBiurowy.UseVisualStyleBackColor = true;
            // 
            // _PracownikPomiarowy
            // 
            this._PracownikPomiarowy.AutoSize = true;
            this._PracownikPomiarowy.Location = new System.Drawing.Point(6, 65);
            this._PracownikPomiarowy.Name = "_PracownikPomiarowy";
            this._PracownikPomiarowy.Size = new System.Drawing.Size(129, 17);
            this._PracownikPomiarowy.TabIndex = 3;
            this._PracownikPomiarowy.Text = "Pracownik Pomiarowy";
            this._PracownikPomiarowy.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._Kierownik);
            this.groupBox1.Controls.Add(this._PracownikPomiarowy);
            this.groupBox1.Controls.Add(this._PracownikBiurowy);
            this.groupBox1.Location = new System.Drawing.Point(18, 198);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 96);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pracownicy";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(18, 117);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(115, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Lokalizacja bazy danych:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Hasło do bazy danych:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Hasło dla danego pracownika:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(18, 168);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(115, 20);
            this.textBox3.TabIndex = 10;
            this.textBox3.UseSystemPasswordChar = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(177, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 34);
            this.button2.TabIndex = 11;
            this.button2.Text = "Zakończ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(21, 26);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(271, 58);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            this.richTextBox1.Click += new System.EventHandler(this.richTextBox1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "(*.mdb)|*.mdb";
            this.openFileDialog1.Tag = "";
            // 
            // LogowanieForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 308);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.Name = "LogowanieForm";
            this.Text = "Logowanie do bazy danych.";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton _Kierownik;
        private System.Windows.Forms.RadioButton _PracownikBiurowy;
        private System.Windows.Forms.RadioButton _PracownikPomiarowy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

