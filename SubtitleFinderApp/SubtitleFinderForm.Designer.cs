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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubtitleFinderForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.comboSources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tableResultLeft = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblComments = new System.Windows.Forms.LinkLabel();
            this.dialogSaveSubtitle = new System.Windows.Forms.SaveFileDialog();
            this.flowResultsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tableResultRight = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DownloadLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableResultLeft.SuspendLayout();
            this.flowResultsPanel.SuspendLayout();
            this.tableResultRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 163);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(589, 44);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(598, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Probar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(3, 109);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(589, 48);
            this.textBox2.TabIndex = 3;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(377, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "the walking dead s09e05";
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(395, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // comboSources
            // 
            this.comboSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSources.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
            // btnDownload
            // 
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Location = new System.Drawing.Point(3, 3);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(104, 26);
            this.btnDownload.TabIndex = 9;
            this.btnDownload.Text = "Descargar";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tableResultLeft
            // 
            this.tableResultLeft.ColumnCount = 1;
            this.tableResultLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableResultLeft.Controls.Add(this.lblTitle, 0, 0);
            this.tableResultLeft.Controls.Add(this.lblDescription, 0, 1);
            this.tableResultLeft.Controls.Add(this.lblDetails, 0, 2);
            this.tableResultLeft.Location = new System.Drawing.Point(3, 3);
            this.tableResultLeft.Name = "tableResultLeft";
            this.tableResultLeft.RowCount = 3;
            this.tableResultLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.79105F));
            this.tableResultLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.20895F));
            this.tableResultLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableResultLeft.Size = new System.Drawing.Size(545, 95);
            this.tableResultLeft.TabIndex = 10;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(545, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TITULO";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(3, 29);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(545, 40);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "DESCRIPCION";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetails
            // 
            this.lblDetails.AutoEllipsis = true;
            this.lblDetails.Location = new System.Drawing.Point(3, 69);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(545, 26);
            this.lblDetails.TabIndex = 2;
            this.lblDetails.Text = "DETALLES";
            this.lblDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblComments
            // 
            this.lblComments.Location = new System.Drawing.Point(3, 50);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(104, 26);
            this.lblComments.TabIndex = 11;
            this.lblComments.TabStop = true;
            this.lblComments.Text = "Comentarios (30)";
            this.lblComments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblComments.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblComments_LinkClicked);
            // 
            // dialogSaveSubtitle
            // 
            this.dialogSaveSubtitle.DefaultExt = "rar";
            this.dialogSaveSubtitle.Filter = "Archivos RAR (*.rar)|*.rar|Todos los archivos (*.*)|*.*";
            this.dialogSaveSubtitle.InitialDirectory = "F:\\Carpeta personal";
            this.dialogSaveSubtitle.RestoreDirectory = true;
            this.dialogSaveSubtitle.Title = "Guardar subtítulo como...";
            // 
            // flowResultsPanel
            // 
            this.flowResultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowResultsPanel.AutoScroll = true;
            this.flowResultsPanel.Controls.Add(this.tableResultLeft);
            this.flowResultsPanel.Controls.Add(this.tableResultRight);
            this.flowResultsPanel.Controls.Add(this.textBox2);
            this.flowResultsPanel.Controls.Add(this.textBox1);
            this.flowResultsPanel.Controls.Add(this.button1);
            this.flowResultsPanel.Location = new System.Drawing.Point(12, 83);
            this.flowResultsPanel.Name = "flowResultsPanel";
            this.flowResultsPanel.Size = new System.Drawing.Size(838, 224);
            this.flowResultsPanel.TabIndex = 12;
            // 
            // tableResultRight
            // 
            this.tableResultRight.ColumnCount = 1;
            this.tableResultRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResultRight.Controls.Add(this.btnDownload, 0, 0);
            this.tableResultRight.Controls.Add(this.lblComments, 0, 1);
            this.tableResultRight.Location = new System.Drawing.Point(554, 3);
            this.tableResultRight.Name = "tableResultRight";
            this.tableResultRight.RowCount = 2;
            this.tableResultRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResultRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResultRight.Size = new System.Drawing.Size(110, 100);
            this.tableResultRight.TabIndex = 13;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.Description,
            this.Comments,
            this.DownloadLink});
            this.dataGridView1.Location = new System.Drawing.Point(12, 313);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(860, 216);
            this.dataGridView1.TabIndex = 13;
            // 
            // Title
            // 
            this.Title.HeaderText = "Titulo";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "Descripcion";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Comments
            // 
            this.Comments.HeaderText = "Comentarios";
            this.Comments.Name = "Comments";
            this.Comments.ReadOnly = true;
            // 
            // DownloadLink
            // 
            this.DownloadLink.HeaderText = "Descargar";
            this.DownloadLink.Name = "DownloadLink";
            this.DownloadLink.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // SubtitleFinderForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.flowResultsPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboSources);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SubtitleFinderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buscador de subtítulos";
            this.Load += new System.EventHandler(this.SubtitleFinderForm_Load);
            this.tableResultLeft.ResumeLayout(false);
            this.flowResultsPanel.ResumeLayout(false);
            this.flowResultsPanel.PerformLayout();
            this.tableResultRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox comboSources;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TableLayoutPanel tableResultLeft;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.LinkLabel lblComments;
        private System.Windows.Forms.SaveFileDialog dialogSaveSubtitle;
        private System.Windows.Forms.FlowLayoutPanel flowResultsPanel;
        private System.Windows.Forms.TableLayoutPanel tableResultRight;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comments;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadLink;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}

