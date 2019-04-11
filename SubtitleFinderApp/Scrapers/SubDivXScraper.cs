using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SubtitleFinderApp.Scrapers
{
    public class SubDivXScraper
    {
        public List<SubDivXSResult> SubDivXSResults { get; set; }

        public SubDivXScraper()
        {
            this.SubDivXSResults = new List<SubDivXSResult>();
        }

        public static void InitGridViewControl(ref DataGridView gridResults)
        {
            gridResults = new DataGridView();
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
            gridResults.Location = new Point(12, 114);
            gridResults.MultiSelect = false;
            gridResults.Name = "gridResults";
            gridResults.ReadOnly = true;
            gridResults.RowHeadersVisible = false;
            gridResults.RowsDefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
            gridResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridResults.Size = new Size(860, 442);

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

            DataGridViewLinkColumn commentsUrlColumn = new DataGridViewLinkColumn();
            commentsUrlColumn.HeaderText = "URL de comentarios";
            commentsUrlColumn.LinkBehavior = LinkBehavior.SystemDefault;
            commentsUrlColumn.Name = "CommentsUrl";
            commentsUrlColumn.Visible = false;
            gridResults.Columns.Add(commentsUrlColumn);

            //return gridResults;
        }

        public void FillResults(ref DataGridView gridResults, IEnumerable<HtmlNode> episodes)
        {
            foreach (var episode in episodes)
            {
                HtmlNode detailsWrapper = episode.NextSibling;
                IEnumerable<HtmlNode> anchors = detailsWrapper.LastChild.Descendants("a");
                SubDivXSResult result = new SubDivXSResult();

                result.EpisodeName = episode.FirstChild.FirstChild.InnerText.Substring(13);
                result.Description = detailsWrapper.FirstChild.NextSibling.InnerText;

                var commentsNode = anchors.Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow") && a.Attributes.Contains("onclick")).SingleOrDefault();
                if (commentsNode != null)
                {
                    result.Comments = commentsNode.InnerText;
                    result.CommentsUrl = commentsNode.Attributes["href"].Value;
                }

                result.UploadBy = anchors.Where(u => u.Attributes.Contains("class") && u.Attributes["class"].Value.Equals("link1")).SingleOrDefault().InnerText;
                result.DownloadUrl = anchors.Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow") && a.Attributes.Contains("target")).SingleOrDefault().Attributes["href"].Value;
                SubDivXSResults.Add(result);
            }

            foreach (SubDivXSResult result in SubDivXSResults)
            {
                gridResults.Rows.Add(
                        result.EpisodeName,
                        result.Description,
                        result.UploadBy,
                        result.Comments,
                        result.DownloadUrl,
                        result.CommentsUrl
                    );
            }
        }
    }

    public class SubDivXSResult
    {
        public string EpisodeName { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string CommentsUrl { get; set; }
        public string UploadBy { get; set; }
        public string DownloadUrl { get; set; }
    }
}
