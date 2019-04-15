﻿using HtmlAgilityPack;
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
        private List<SubDivXResult> _SubDivXResults { get; set; }
        private DataGridView _GridResults { get; set; }

        private const string _SearchUrlStartPart = "https://www.subdivx.com/index.php?buscar=";
        private const string _SearchUrlEndPart = "&accion=5&masdesc=&subtitulos=1&realiza_b=1";
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.GetEncoding("ISO-8859-1") };

        public SubDivXScraper()
        {
            this._SubDivXResults = new List<SubDivXResult>();
        }

        private void InitGridViewControl()
        {
            _GridResults = new DataGridView();
            _GridResults.AllowUserToAddRows = false;
            _GridResults.AllowUserToDeleteRows = false;
            _GridResults.AllowUserToResizeRows = false;
            _GridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            _GridResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _GridResults.BackgroundColor = SystemColors.Control;
            _GridResults.BorderStyle = BorderStyle.None;
            _GridResults.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _GridResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _GridResults.Location = new Point(12, 114);
            _GridResults.MultiSelect = false;
            _GridResults.Name = "gridResults";
            _GridResults.ReadOnly = true;
            _GridResults.RowHeadersVisible = false;
            _GridResults.RowsDefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
            _GridResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _GridResults.Size = new Size(860, 442);
            _GridResults.TabIndex = 6;

            DataGridViewTextBoxColumn Title = new DataGridViewTextBoxColumn();
            Title.HeaderText = "Título";
            Title.Name = "Title";
            Title.ReadOnly = true;
            Title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Title.Width = 180;
            _GridResults.Columns.Add(Title);

            DataGridViewTextBoxColumn Description = new DataGridViewTextBoxColumn();
            Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Description.HeaderText = "Descripción";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Description.Width = 400;
            _GridResults.Columns.Add(Description);

            DataGridViewTextBoxColumn UploadBy = new DataGridViewTextBoxColumn();
            UploadBy.HeaderText = "Subido por";
            UploadBy.Name = "UploadBy";
            UploadBy.ReadOnly = true;
            UploadBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            UploadBy.Width = 80;
            _GridResults.Columns.Add(UploadBy);

            DataGridViewLinkColumn commentsColumn = new DataGridViewLinkColumn();
            commentsColumn.HeaderText = "Comentarios";
            commentsColumn.LinkBehavior = LinkBehavior.SystemDefault;
            commentsColumn.Name = "Comments";
            commentsColumn.Text = "Comentarios";
            commentsColumn.Width = 80;
            _GridResults.Columns.Add(commentsColumn);

            DataGridViewLinkColumn downloadsColumn = new DataGridViewLinkColumn();
            downloadsColumn.HeaderText = "Descargar";
            downloadsColumn.LinkBehavior = LinkBehavior.SystemDefault;
            downloadsColumn.Name = "DownloadLink";
            downloadsColumn.Text = "Descargar";
            //downloadsColumn.UseColumnTextForLinkValue = true;
            downloadsColumn.Width = 80;
            _GridResults.Columns.Add(downloadsColumn);

            DataGridViewLinkColumn commentsUrlColumn = new DataGridViewLinkColumn();
            commentsUrlColumn.HeaderText = "URL de comentarios";
            commentsUrlColumn.LinkBehavior = LinkBehavior.SystemDefault;
            commentsUrlColumn.Name = "CommentsUrl";
            commentsUrlColumn.Visible = false;
            _GridResults.Columns.Add(commentsUrlColumn);

            _GridResults.CellContentClick += this._GridResults_CellContentClick;
        }

        public IEnumerable<HtmlNode> GetEpisodeNodes(string text)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = _web.Load(_SearchUrlStartPart + HttpUtility.UrlEncode(text) + _SearchUrlEndPart);
            HtmlNode wrapper = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("contenedor_izq")).SingleOrDefault();
            IEnumerable<HtmlNode> episodes = wrapper.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("menu_detalle_buscador"));

            return episodes;
        }

        public DataGridView GenerateResults(IEnumerable<HtmlNode> episodes)
        {
            InitGridViewControl();

            foreach (var episode in episodes)
            {
                HtmlNode detailsWrapper = episode.NextSibling;
                IEnumerable<HtmlNode> anchors = detailsWrapper.LastChild.Descendants("a");
                SubDivXResult result = new SubDivXResult();

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
                _SubDivXResults.Add(result);
            }

            foreach (SubDivXResult result in _SubDivXResults)
            {
                _GridResults.Rows.Add(
                        result.EpisodeName,
                        result.Description,
                        result.UploadBy,
                        result.Comments,
                        result.DownloadUrl,
                        result.CommentsUrl
                    );
            }

            return _GridResults;
        }

        private void _GridResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView currentGridView = (DataGridView)sender;
            DataGridViewRow currentRow = currentGridView.Rows[e.RowIndex];

            if (e.ColumnIndex == 3)
            {
                string commentUrl = currentRow.Cells["CommentsUrl"].Value.ToString();
                string queryString = commentUrl.Substring(commentUrl.IndexOf('?'));
                string subtitleId = HttpUtility.ParseQueryString(queryString).Get("idsub");

                var htmlComments = new HtmlWeb() { OverrideEncoding = Encoding.Default }.Load("https://www.subdivx.com/popcoment.php?idsub=" + HttpUtility.UrlEncode(subtitleId));
                var userComments = htmlComments.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("pop_upcoment"));
                new SubDivXCommentsDialog().ShowDialog(userComments);
            }

            if (e.ColumnIndex == 4)
            {
                string downloadUrl = currentRow.Cells["DownloadLink"].Value.ToString();
                string queryString = downloadUrl.Substring(downloadUrl.IndexOf('?'));
                string subtitleId = HttpUtility.ParseQueryString(queryString).Get("id");

                SaveFileDialog dialogSaveSubtitle = new SaveFileDialog();
                dialogSaveSubtitle.FileName = string.Join(" ", currentRow.Cells["Title"].Value.ToString(), "-", subtitleId);
                dialogSaveSubtitle.DefaultExt = "rar";
                dialogSaveSubtitle.Filter = "Archivos RAR (*.rar)|*.rar|Todos los archivos (*.*)|*.*";
                dialogSaveSubtitle.InitialDirectory = "%USERPROFILE%\\Downloads";
                dialogSaveSubtitle.RestoreDirectory = true;
                dialogSaveSubtitle.Title = "Guardar subtítulo como...";

                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var wClient = new System.Net.WebClient();
                    wClient.DownloadFile(downloadUrl, dialogSaveSubtitle.FileName);
                }
            }
        }
    }
}