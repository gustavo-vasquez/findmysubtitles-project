using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp.Scrapers
{
    public class SubDivXScraper2
    {
        public List<HtmlNode> Title { get; set; }
        public List<HtmlNode> Description { get; set; }
        public List<HtmlNode> Details { get; set; }
        public List<HtmlNode> Comments { get; set; }        
        public List<HtmlNode> DownloadLink { get; set; }

        public DataGridView InitSubDivXGridResults()
        {
            DataGridView gridResults = new DataGridView();
            gridResults.AllowUserToAddRows = false;
            gridResults.AllowUserToDeleteRows = false;
            gridResults.AllowUserToResizeRows = false;
            gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            gridResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridResults.BackgroundColor = SystemColors.Control;
            gridResults.BorderStyle = BorderStyle.None;
            gridResults.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridResults.Location = new Point(12, 110);
            gridResults.MultiSelect = false;
            gridResults.Name = "gridResults";
            gridResults.ReadOnly = true;
            gridResults.RowHeadersVisible = false;
            gridResults.RowsDefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
            gridResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridResults.Size = new Size(860, 420);

            DataGridViewTextBoxColumn Title = new DataGridViewTextBoxColumn();
            //Title.FillWeight = 194.9239F;
            Title.HeaderText = "Titulo";
            Title.Name = "Title";
            Title.ReadOnly = true;
            Title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Title.Width = 180;
            gridResults.Columns.Add(Title);

            DataGridViewTextBoxColumn Description = new DataGridViewTextBoxColumn();
            Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            //Description.FillWeight = 5.076141F;
            Description.HeaderText = "Descripcion";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Description.Width = 400;
            gridResults.Columns.Add(Description);

            DataGridViewTextBoxColumn UploadBy = new DataGridViewTextBoxColumn();            
            UploadBy.HeaderText = "Subido por";
            UploadBy.Name = "UploadBy";
            UploadBy.ReadOnly = true;
            UploadBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            UploadBy.Width = 80;
            gridResults.Columns.Add(UploadBy);

            DataGridViewLinkColumn commentsColumn = new DataGridViewLinkColumn();
            commentsColumn.HeaderText = "Comentarios";
            commentsColumn.LinkBehavior = LinkBehavior.SystemDefault;
            commentsColumn.Name = "Comments";
            commentsColumn.Text = "Comentarios";
            commentsColumn.Width = 80;
            gridResults.Columns.Add(commentsColumn);

            DataGridViewLinkColumn downloadsColumn = new DataGridViewLinkColumn();
            downloadsColumn.HeaderText = "Descargar";
            downloadsColumn.LinkBehavior = LinkBehavior.SystemDefault;
            downloadsColumn.Name = "DownloadLink";
            downloadsColumn.Text = "Descargar";
            //downloadsColumn.UseColumnTextForLinkValue = true;
            downloadsColumn.Width = 80;
            gridResults.Columns.Add(downloadsColumn);

            return gridResults;
        }
    }
}
