namespace DotBase
{
    partial class AdministracjaForm
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
            System.Windows.Forms.Button cancelBtn;
            this.listaView = new System.Windows.Forms.DataGridView();
            this.nazwaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hasloColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.adminColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.okBtn = new System.Windows.Forms.Button();
            this.usunBtn = new System.Windows.Forms.Button();
            this.hasloBazyBtn = new System.Windows.Forms.Button();
            cancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listaView)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelBtn.Location = new System.Drawing.Point(427, 400);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new System.Drawing.Size(93, 28);
            cancelBtn.TabIndex = 2;
            cancelBtn.Text = "Anuluj";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // listaView
            // 
            this.listaView.AllowUserToDeleteRows = false;
            this.listaView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listaView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listaView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nazwaColumn,
            this.hasloColumn,
            this.adminColumn});
            this.listaView.Location = new System.Drawing.Point(12, 12);
            this.listaView.Name = "listaView";
            this.listaView.Size = new System.Drawing.Size(508, 372);
            this.listaView.TabIndex = 0;
            this.listaView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.listaView_CellContentClick);
            this.listaView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.listaView_CellValueChanged);
            this.listaView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.listaView_RowsAdded);
            // 
            // nazwaColumn
            // 
            this.nazwaColumn.HeaderText = "Nazwa";
            this.nazwaColumn.Name = "nazwaColumn";
            this.nazwaColumn.Width = 150;
            // 
            // hasloColumn
            // 
            this.hasloColumn.HeaderText = "Hasło";
            this.hasloColumn.Name = "hasloColumn";
            this.hasloColumn.ReadOnly = true;
            this.hasloColumn.Width = 90;
            // 
            // adminColumn
            // 
            this.adminColumn.HeaderText = "Administrator";
            this.adminColumn.Name = "adminColumn";
            this.adminColumn.Width = 90;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(328, 400);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(93, 28);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // usunBtn
            // 
            this.usunBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.usunBtn.Location = new System.Drawing.Point(12, 400);
            this.usunBtn.Name = "usunBtn";
            this.usunBtn.Size = new System.Drawing.Size(125, 28);
            this.usunBtn.TabIndex = 3;
            this.usunBtn.Text = "Usuń użytkownika";
            this.usunBtn.UseVisualStyleBackColor = true;
            this.usunBtn.Click += new System.EventHandler(this.usunBtn_Click);
            // 
            // hasloBazyBtn
            // 
            this.hasloBazyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hasloBazyBtn.Location = new System.Drawing.Point(143, 400);
            this.hasloBazyBtn.Name = "hasloBazyBtn";
            this.hasloBazyBtn.Size = new System.Drawing.Size(125, 28);
            this.hasloBazyBtn.TabIndex = 4;
            this.hasloBazyBtn.Text = "Hasło do bazy";
            this.hasloBazyBtn.UseVisualStyleBackColor = true;
            this.hasloBazyBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // AdministracjaForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cancelBtn;
            this.ClientSize = new System.Drawing.Size(532, 440);
            this.Controls.Add(this.hasloBazyBtn);
            this.Controls.Add(this.usunBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(cancelBtn);
            this.Controls.Add(this.listaView);
            this.Name = "AdministracjaForm";
            this.Text = "Użytkownicy";
            this.Load += new System.EventHandler(this.AdministracjaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listaView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView listaView;
        private System.Windows.Forms.DataGridViewTextBoxColumn nazwaColumn;
        private System.Windows.Forms.DataGridViewButtonColumn hasloColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn adminColumn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button usunBtn;
        private System.Windows.Forms.Button hasloBazyBtn;
    }
}