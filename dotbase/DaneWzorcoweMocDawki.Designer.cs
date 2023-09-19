namespace DotBase
{
    partial class DaneWzorcoweMocDawki
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Odleglosc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zrodlo3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zrodlo2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zrodlo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Odleglosc,
            this.Zrodlo3,
            this.Zrodlo2,
            this.Zrodlo1});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(633, 951);
            this.dataGridView1.TabIndex = 0;
            // 
            // Odleglosc
            // 
            this.Odleglosc.HeaderText = "Odległość";
            this.Odleglosc.Name = "Odleglosc";
            this.Odleglosc.ReadOnly = true;
            // 
            // Zrodlo3
            // 
            this.Zrodlo3.HeaderText = "Źródło 3";
            this.Zrodlo3.Name = "Zrodlo3";
            this.Zrodlo3.ReadOnly = true;
            // 
            // Zrodlo2
            // 
            this.Zrodlo2.HeaderText = "Źródło 2";
            this.Zrodlo2.Name = "Zrodlo2";
            this.Zrodlo2.ReadOnly = true;
            // 
            // Zrodlo1
            // 
            this.Zrodlo1.HeaderText = "Źródło 1";
            this.Zrodlo1.Name = "Zrodlo1";
            this.Zrodlo1.ReadOnly = true;
            // 
            // DaneWzorcoweMocDawki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 951);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DaneWzorcoweMocDawki";
            this.Text = "Dane wzorcowe dla mocy dawki";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Odleglosc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zrodlo3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zrodlo2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zrodlo1;
    }
}