namespace DotBase
{
    partial class CennikForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.sprawdzenieCheckBox = new System.Windows.Forms.CheckBox();
            this.zepsutyCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.ostrzerzenieLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.skazeniaDlugieBox = new System.Windows.Forms.CheckBox();
            this.dawkaDlugaBox = new System.Windows.Forms.CheckBox();
            this.wariantRozszerzonyBox = new System.Windows.Forms.CheckBox();
            this.wariantTrudnyBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDown4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(17, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(217, 202);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rodzaje wzorcowań";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 169);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Sygnalizacja";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(111, 166);
            this.numericUpDown4.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown4.TabIndex = 6;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 123);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Dawka";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 67);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 34);
            this.label2.TabIndex = 3;
            this.label2.Text = "Emisja\r\npowierzchniowa";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(111, 121);
            this.numericUpDown3.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown3.TabIndex = 4;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(111, 75);
            this.numericUpDown2.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown2.TabIndex = 2;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Moc dawki";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(111, 30);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 26);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(207, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "Liczba progów sygnalizatora >3";
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.Location = new System.Drawing.Point(241, 23);
            this.numericUpDown8.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown8.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown8.TabIndex = 0;
            this.numericUpDown8.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 492);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Rabat [%]";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.DecimalPlaces = 2;
            this.numericUpDown5.Location = new System.Drawing.Point(116, 489);
            this.numericUpDown5.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(92, 22);
            this.numericUpDown5.TabIndex = 4;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(8, 23);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(81, 21);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Ekspres";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.LiczSume);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(308, 509);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(111, 22);
            this.textBox1.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(232, 510);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "SUMA";
            // 
            // sprawdzenieCheckBox
            // 
            this.sprawdzenieCheckBox.AutoSize = true;
            this.sprawdzenieCheckBox.Location = new System.Drawing.Point(8, 52);
            this.sprawdzenieCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.sprawdzenieCheckBox.Name = "sprawdzenieCheckBox";
            this.sprawdzenieCheckBox.Size = new System.Drawing.Size(111, 21);
            this.sprawdzenieCheckBox.TabIndex = 9;
            this.sprawdzenieCheckBox.Text = "Sprawdzenie";
            this.sprawdzenieCheckBox.UseVisualStyleBackColor = true;
            this.sprawdzenieCheckBox.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // zepsutyCheckBox
            // 
            this.zepsutyCheckBox.AutoSize = true;
            this.zepsutyCheckBox.Location = new System.Drawing.Point(8, 80);
            this.zepsutyCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.zepsutyCheckBox.Name = "zepsutyCheckBox";
            this.zepsutyCheckBox.Size = new System.Drawing.Size(81, 21);
            this.zepsutyCheckBox.TabIndex = 10;
            this.zepsutyCheckBox.Text = "Zepsuty";
            this.zepsutyCheckBox.UseVisualStyleBackColor = true;
            this.zepsutyCheckBox.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.zepsutyCheckBox);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.sprawdzenieCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(249, 16);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(171, 202);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inne";
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.DecimalPlaces = 2;
            this.numericUpDown6.Location = new System.Drawing.Point(116, 530);
            this.numericUpDown6.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(92, 22);
            this.numericUpDown6.TabIndex = 13;
            this.numericUpDown6.ValueChanged += new System.EventHandler(this.LiczSume);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 532);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 17);
            this.label9.TabIndex = 12;
            this.label9.Text = "Transport";
            // 
            // ostrzerzenieLabel
            // 
            this.ostrzerzenieLabel.AutoSize = true;
            this.ostrzerzenieLabel.ForeColor = System.Drawing.Color.Red;
            this.ostrzerzenieLabel.Location = new System.Drawing.Point(141, 23);
            this.ostrzerzenieLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ostrzerzenieLabel.Name = "ostrzerzenieLabel";
            this.ostrzerzenieLabel.Size = new System.Drawing.Size(210, 34);
            this.ostrzerzenieLabel.TabIndex = 14;
            this.ostrzerzenieLabel.Text = "<-- Najprawdopodobniej jest\r\n      to rozszerzone wzorcowanie";
            this.ostrzerzenieLabel.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.wariantTrudnyBox);
            this.groupBox3.Controls.Add(this.wariantRozszerzonyBox);
            this.groupBox3.Controls.Add(this.ostrzerzenieLabel);
            this.groupBox3.Location = new System.Drawing.Point(17, 238);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(403, 84);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Wariant wzorcowania mocy dawki";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.skazeniaDlugieBox);
            this.groupBox4.Controls.Add(this.dawkaDlugaBox);
            this.groupBox4.Controls.Add(this.numericUpDown8);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(17, 346);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(404, 117);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Opcje dodatkowe";
            // 
            // skazeniaDlugieBox
            // 
            this.skazeniaDlugieBox.AutoSize = true;
            this.skazeniaDlugieBox.Location = new System.Drawing.Point(8, 84);
            this.skazeniaDlugieBox.Margin = new System.Windows.Forms.Padding(4);
            this.skazeniaDlugieBox.Name = "skazeniaDlugieBox";
            this.skazeniaDlugieBox.Size = new System.Drawing.Size(130, 21);
            this.skazeniaDlugieBox.TabIndex = 12;
            this.skazeniaDlugieBox.Text = "Skażenia długie";
            this.skazeniaDlugieBox.UseVisualStyleBackColor = true;
            this.skazeniaDlugieBox.CheckedChanged += new System.EventHandler(this.LiczSume);
            // 
            // dawkaDlugaBox
            // 
            this.dawkaDlugaBox.AutoSize = true;
            this.dawkaDlugaBox.Location = new System.Drawing.Point(8, 55);
            this.dawkaDlugaBox.Margin = new System.Windows.Forms.Padding(4);
            this.dawkaDlugaBox.Name = "dawkaDlugaBox";
            this.dawkaDlugaBox.Size = new System.Drawing.Size(111, 21);
            this.dawkaDlugaBox.TabIndex = 11;
            this.dawkaDlugaBox.Text = "Dawka długa";
            this.dawkaDlugaBox.UseVisualStyleBackColor = true;
            this.dawkaDlugaBox.CheckedChanged += new System.EventHandler(this.LiczSume);
            // 
            // wariantRozszerzonyBox
            // 
            this.wariantRozszerzonyBox.AutoSize = true;
            this.wariantRozszerzonyBox.Location = new System.Drawing.Point(7, 22);
            this.wariantRozszerzonyBox.Name = "wariantRozszerzonyBox";
            this.wariantRozszerzonyBox.Size = new System.Drawing.Size(112, 21);
            this.wariantRozszerzonyBox.TabIndex = 15;
            this.wariantRozszerzonyBox.Text = "Rozszerzony";
            this.wariantRozszerzonyBox.UseVisualStyleBackColor = true;
            this.wariantRozszerzonyBox.CheckedChanged += new System.EventHandler(this.LiczSume);
            // 
            // wariantTrudnyBox
            // 
            this.wariantTrudnyBox.AutoSize = true;
            this.wariantTrudnyBox.Location = new System.Drawing.Point(7, 49);
            this.wariantTrudnyBox.Name = "wariantTrudnyBox";
            this.wariantTrudnyBox.Size = new System.Drawing.Size(75, 21);
            this.wariantTrudnyBox.TabIndex = 16;
            this.wariantTrudnyBox.Text = "Trudny";
            this.wariantTrudnyBox.UseVisualStyleBackColor = true;
            this.wariantTrudnyBox.CheckedChanged += new System.EventHandler(this.LiczSume);
            // 
            // CennikForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 569);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.numericUpDown6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numericUpDown5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CennikForm";
            this.Text = "Cennik";
            this.Load += new System.EventHandler(this.CennikForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox sprawdzenieCheckBox;
        private System.Windows.Forms.CheckBox zepsutyCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label ostrzerzenieLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox skazeniaDlugieBox;
        private System.Windows.Forms.CheckBox dawkaDlugaBox;
        private System.Windows.Forms.CheckBox wariantTrudnyBox;
        private System.Windows.Forms.CheckBox wariantRozszerzonyBox;
    }
}