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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubtitleFinderForm));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnProductInfo = new System.Windows.Forms.Button();
            this.rdoBtnSubDivX = new System.Windows.Forms.RadioButton();
            this.rdoBtnTuSubtitulo = new System.Windows.Forms.RadioButton();
            this.rdoBtnSubtitulamos = new System.Windows.Forms.RadioButton();
            this.picBoxSubtitulamos = new System.Windows.Forms.PictureBox();
            this.picBoxTuSubtitulo = new System.Windows.Forms.PictureBox();
            this.picBoxSubDivX = new System.Windows.Forms.PictureBox();
            this.picBoxAppImage = new System.Windows.Forms.PictureBox();
            this.lblSearchExample = new System.Windows.Forms.Label();
            this.btnDownloadFolder = new System.Windows.Forms.Button();
            this.tltDownloadFolder = new System.Windows.Forms.ToolTip(this.components);
            this.tltProductInfo = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSubtitulamos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTuSubtitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSubDivX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAppImage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(12, 74);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(704, 20);
            this.txtSearch.TabIndex = 0;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(9, 58);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(277, 13);
            this.lblSearch.TabIndex = 8;
            this.lblSearch.Text = "Escriba el nombre de la serie/película (+palabras claves):";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(722, 72);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(150, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Buscar subtítulo";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnProductInfo
            // 
            this.btnProductInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProductInfo.FlatAppearance.BorderSize = 0;
            this.btnProductInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductInfo.Image = global::SubtitleFinderApp.Properties.Resources.info;
            this.btnProductInfo.Location = new System.Drawing.Point(845, 12);
            this.btnProductInfo.Name = "btnProductInfo";
            this.btnProductInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnProductInfo.Size = new System.Drawing.Size(27, 23);
            this.btnProductInfo.TabIndex = 6;
            this.btnProductInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProductInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProductInfo.UseVisualStyleBackColor = true;
            this.btnProductInfo.Click += new System.EventHandler(this.btnProductInfo_Click);
            // 
            // rdoBtnSubDivX
            // 
            this.rdoBtnSubDivX.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rdoBtnSubDivX.AutoSize = true;
            this.rdoBtnSubDivX.Checked = true;
            this.rdoBtnSubDivX.Location = new System.Drawing.Point(303, 40);
            this.rdoBtnSubDivX.Name = "rdoBtnSubDivX";
            this.rdoBtnSubDivX.Size = new System.Drawing.Size(90, 17);
            this.rdoBtnSubDivX.TabIndex = 3;
            this.rdoBtnSubDivX.TabStop = true;
            this.rdoBtnSubDivX.Text = "SubDivX.com";
            this.rdoBtnSubDivX.UseVisualStyleBackColor = true;
            // 
            // rdoBtnTuSubtitulo
            // 
            this.rdoBtnTuSubtitulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rdoBtnTuSubtitulo.AutoSize = true;
            this.rdoBtnTuSubtitulo.Location = new System.Drawing.Point(399, 40);
            this.rdoBtnTuSubtitulo.Name = "rdoBtnTuSubtitulo";
            this.rdoBtnTuSubtitulo.Size = new System.Drawing.Size(102, 17);
            this.rdoBtnTuSubtitulo.TabIndex = 4;
            this.rdoBtnTuSubtitulo.TabStop = true;
            this.rdoBtnTuSubtitulo.Text = "TuSubtitulo.com";
            this.rdoBtnTuSubtitulo.UseVisualStyleBackColor = true;
            // 
            // rdoBtnSubtitulamos
            // 
            this.rdoBtnSubtitulamos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rdoBtnSubtitulamos.AutoSize = true;
            this.rdoBtnSubtitulamos.Location = new System.Drawing.Point(507, 40);
            this.rdoBtnSubtitulamos.Name = "rdoBtnSubtitulamos";
            this.rdoBtnSubtitulamos.Size = new System.Drawing.Size(97, 17);
            this.rdoBtnSubtitulamos.TabIndex = 5;
            this.rdoBtnSubtitulamos.TabStop = true;
            this.rdoBtnSubtitulamos.Text = "Subtitulamos.tv";
            this.rdoBtnSubtitulamos.UseVisualStyleBackColor = true;
            // 
            // picBoxSubtitulamos
            // 
            this.picBoxSubtitulamos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picBoxSubtitulamos.Image = ((System.Drawing.Image)(resources.GetObject("picBoxSubtitulamos.Image")));
            this.picBoxSubtitulamos.Location = new System.Drawing.Point(548, 20);
            this.picBoxSubtitulamos.Name = "picBoxSubtitulamos";
            this.picBoxSubtitulamos.Size = new System.Drawing.Size(16, 16);
            this.picBoxSubtitulamos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxSubtitulamos.TabIndex = 19;
            this.picBoxSubtitulamos.TabStop = false;
            // 
            // picBoxTuSubtitulo
            // 
            this.picBoxTuSubtitulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picBoxTuSubtitulo.Image = global::SubtitleFinderApp.Properties.Resources.tusubtitulo;
            this.picBoxTuSubtitulo.Location = new System.Drawing.Point(441, 20);
            this.picBoxTuSubtitulo.Name = "picBoxTuSubtitulo";
            this.picBoxTuSubtitulo.Size = new System.Drawing.Size(16, 16);
            this.picBoxTuSubtitulo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxTuSubtitulo.TabIndex = 20;
            this.picBoxTuSubtitulo.TabStop = false;
            // 
            // picBoxSubDivX
            // 
            this.picBoxSubDivX.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picBoxSubDivX.Image = global::SubtitleFinderApp.Properties.Resources.subdivx;
            this.picBoxSubDivX.Location = new System.Drawing.Point(340, 20);
            this.picBoxSubDivX.Name = "picBoxSubDivX";
            this.picBoxSubDivX.Size = new System.Drawing.Size(16, 16);
            this.picBoxSubDivX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxSubDivX.TabIndex = 21;
            this.picBoxSubDivX.TabStop = false;
            // 
            // picBoxAppImage
            // 
            this.picBoxAppImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picBoxAppImage.Image = ((System.Drawing.Image)(resources.GetObject("picBoxAppImage.Image")));
            this.picBoxAppImage.Location = new System.Drawing.Point(386, 248);
            this.picBoxAppImage.Name = "picBoxAppImage";
            this.picBoxAppImage.Size = new System.Drawing.Size(128, 128);
            this.picBoxAppImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxAppImage.TabIndex = 22;
            this.picBoxAppImage.TabStop = false;
            // 
            // lblSearchExample
            // 
            this.lblSearchExample.AutoSize = true;
            this.lblSearchExample.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchExample.Location = new System.Drawing.Point(12, 96);
            this.lblSearchExample.Name = "lblSearchExample";
            this.lblSearchExample.Size = new System.Drawing.Size(248, 12);
            this.lblSearchExample.TabIndex = 23;
            this.lblSearchExample.Text = "Ejemplos: \"the walking dead s09e01\", \"batman begins 2005\"";
            // 
            // btnDownloadFolder
            // 
            this.btnDownloadFolder.FlatAppearance.BorderSize = 0;
            this.btnDownloadFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadFolder.Image = global::SubtitleFinderApp.Properties.Resources.open_download_folder;
            this.btnDownloadFolder.Location = new System.Drawing.Point(12, 12);
            this.btnDownloadFolder.Name = "btnDownloadFolder";
            this.btnDownloadFolder.Size = new System.Drawing.Size(27, 23);
            this.btnDownloadFolder.TabIndex = 2;
            this.btnDownloadFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownloadFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownloadFolder.UseVisualStyleBackColor = true;
            this.btnDownloadFolder.Click += new System.EventHandler(this.btnDownloadFolder_Click);
            // 
            // SubtitleFinderForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.btnDownloadFolder);
            this.Controls.Add(this.lblSearchExample);
            this.Controls.Add(this.picBoxAppImage);
            this.Controls.Add(this.picBoxSubDivX);
            this.Controls.Add(this.picBoxTuSubtitulo);
            this.Controls.Add(this.picBoxSubtitulamos);
            this.Controls.Add(this.rdoBtnSubtitulamos);
            this.Controls.Add(this.rdoBtnTuSubtitulo);
            this.Controls.Add(this.rdoBtnSubDivX);
            this.Controls.Add(this.btnProductInfo);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "SubtitleFinderForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find my subtitles";
            this.Load += new System.EventHandler(this.SubtitleFinderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSubtitulamos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTuSubtitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSubDivX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAppImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnProductInfo;
        private System.Windows.Forms.RadioButton rdoBtnSubDivX;
        private System.Windows.Forms.RadioButton rdoBtnTuSubtitulo;
        private System.Windows.Forms.RadioButton rdoBtnSubtitulamos;
        private System.Windows.Forms.PictureBox picBoxSubtitulamos;
        private System.Windows.Forms.PictureBox picBoxTuSubtitulo;
        private System.Windows.Forms.PictureBox picBoxSubDivX;
        private System.Windows.Forms.PictureBox picBoxAppImage;
        private System.Windows.Forms.Label lblSearchExample;
        private System.Windows.Forms.Button btnDownloadFolder;
        private System.Windows.Forms.ToolTip tltDownloadFolder;
        private System.Windows.Forms.ToolTip tltProductInfo;
    }
}

