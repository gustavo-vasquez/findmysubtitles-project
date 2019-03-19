namespace SubtitleFinderApp
{
    partial class SubtitleFinderForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubtitleFinderForm));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.comboSources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dialogSaveSubtitle = new System.Windows.Forms.SaveFileDialog();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDetails = new System.Windows.Forms.StatusStrip();
            this.statusbarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnProductInfo = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.statusDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(377, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "jigsaw 2017";
            // 
            // comboSources
            // 
            this.comboSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSources.FormattingEnabled = true;
            this.comboSources.Items.AddRange(new object[] {
            "SubDivX.com",
            "TuSubtitulo.com",
            "Subtitulamos.tv"});
            this.comboSources.Location = new System.Drawing.Point(555, 39);
            this.comboSources.Name = "comboSources";
            this.comboSources.Size = new System.Drawing.Size(121, 21);
            this.comboSources.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(506, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Fuente:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Escriba el nombre de la serie/película:";
            // 
            // dialogSaveSubtitle
            // 
            this.dialogSaveSubtitle.DefaultExt = "rar";
            this.dialogSaveSubtitle.Filter = "Archivos RAR (*.rar)|*.rar|Todos los archivos (*.*)|*.*";
            this.dialogSaveSubtitle.InitialDirectory = "%USERPROFILE%\\Downloads";
            this.dialogSaveSubtitle.RestoreDirectory = true;
            this.dialogSaveSubtitle.Title = "Guardar subtítulo como...";
            // 
            // gridResults
            // 
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.AllowUserToResizeRows = false;
            this.gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridResults.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.Description});
            this.gridResults.Location = new System.Drawing.Point(12, 100);
            this.gridResults.MultiSelect = false;
            this.gridResults.Name = "gridResults";
            this.gridResults.ReadOnly = true;
            this.gridResults.RowHeadersVisible = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridResults.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResults.Size = new System.Drawing.Size(860, 320);
            this.gridResults.TabIndex = 4;
            this.gridResults.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridResults_CellContentClick);
            this.gridResults.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.gridResults_RowStateChanged);
            // 
            // Title
            // 
            this.Title.FillWeight = 194.9239F;
            this.Title.HeaderText = "Titulo";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Title.Width = 200;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.FillWeight = 5.076141F;
            this.Description.HeaderText = "Descripcion";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // statusDetails
            // 
            this.statusDetails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusbarLabel});
            this.statusDetails.Location = new System.Drawing.Point(0, 440);
            this.statusDetails.Name = "statusDetails";
            this.statusDetails.Size = new System.Drawing.Size(884, 22);
            this.statusDetails.TabIndex = 14;
            // 
            // statusbarLabel
            // 
            this.statusbarLabel.Name = "statusbarLabel";
            this.statusbarLabel.Size = new System.Drawing.Size(32, 17);
            this.statusbarLabel.Text = "Listo";
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::SubtitleFinderApp.Properties.Resources.search;
            this.btnSearch.Location = new System.Drawing.Point(395, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnProductInfo
            // 
            this.btnProductInfo.Image = global::SubtitleFinderApp.Properties.Resources.info;
            this.btnProductInfo.Location = new System.Drawing.Point(797, 38);
            this.btnProductInfo.Name = "btnProductInfo";
            this.btnProductInfo.Size = new System.Drawing.Size(75, 23);
            this.btnProductInfo.TabIndex = 3;
            this.btnProductInfo.Text = "Info";
            this.btnProductInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProductInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProductInfo.UseVisualStyleBackColor = true;
            this.btnProductInfo.Click += new System.EventHandler(this.btnProductInfo_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(701, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SubtitleFinderForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 462);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnProductInfo);
            this.Controls.Add(this.statusDetails);
            this.Controls.Add(this.gridResults);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboSources);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "SubtitleFinderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buscador de subtítulos";
            this.Load += new System.EventHandler(this.SubtitleFinderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.statusDetails.ResumeLayout(false);
            this.statusDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox comboSources;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog dialogSaveSubtitle;
        private System.Windows.Forms.DataGridView gridResults;
        private System.Windows.Forms.StatusStrip statusDetails;
        private System.Windows.Forms.Button btnProductInfo;
        private System.Windows.Forms.ToolStripStatusLabel statusbarLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.Button button1;
    }
}

