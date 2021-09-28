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
        private List<SubDivXResult> _SubDivXResults { get; set; }
        private DataGridView _GridResults { get; set; }
        private bool _isBusy { get; set; }
        private HtmlNode _wrapper { get; set; }

        private const string _SearchUrlStartPart = "https://www.subdivx.com/index.php?buscar=";
        private const string _SearchUrlEndPart = "&accion=5&masdesc=&subtitulos=1&realiza_b=1";
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.GetEncoding("utf-8") };

        public SubDivXScraper()
        {
            
        }

        private void InitGridViewControl()
        {
            Rectangle mainFormCR = Application.OpenForms["SubtitleFinderForm"].ClientRectangle;

            _GridResults = new DataGridView();
            _GridResults.AllowUserToAddRows = false;
            _GridResults.AllowUserToDeleteRows = false;
            _GridResults.AllowUserToResizeRows = false;
            _GridResults.AllowUserToOrderColumns = false;
            _GridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            _GridResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            _GridResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _GridResults.BackgroundColor = SystemColors.Control;
            _GridResults.BorderStyle = BorderStyle.None;
            _GridResults.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _GridResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _GridResults.AllowUserToResizeColumns = true;
            _GridResults.Location = new Point(12, 114);
            _GridResults.MultiSelect = false;
            _GridResults.Name = "gridResults";
            _GridResults.ReadOnly = true;
            _GridResults.RowHeadersVisible = false;
            _GridResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _GridResults.Size = new Size(mainFormCR.Width - 24, mainFormCR.Height - 150);
            _GridResults.MinimumSize = new Size(860, 412);
            _GridResults.TabIndex = 7;

            DataGridViewTextBoxColumn Title = new DataGridViewTextBoxColumn();
            Title.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Title.HeaderText = "Título";
            Title.Name = "Title";
            Title.ReadOnly = true;
            Title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Title.Width = 180;
            _GridResults.Columns.Add(Title);

            DataGridViewTextBoxColumn Description = new DataGridViewTextBoxColumn();
            Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Description.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Description.HeaderText = "Descripción";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Description.Width = 422;
            _GridResults.Columns.Add(Description);

            DataGridViewTextBoxColumn UploadBy = new DataGridViewTextBoxColumn();
            UploadBy.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            UploadBy.HeaderText = "Subido por";
            UploadBy.Name = "UploadBy";
            UploadBy.ReadOnly = true;
            UploadBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            UploadBy.Width = 80;
            _GridResults.Columns.Add(UploadBy);

            DataGridViewLinkColumn commentsColumn = new DataGridViewLinkColumn();
            commentsColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            commentsColumn.HeaderText = "Comentarios";
            commentsColumn.LinkBehavior = LinkBehavior.HoverUnderline;
            commentsColumn.Name = "Comments";
            commentsColumn.ReadOnly = true;
            commentsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            commentsColumn.Width = 70;
            _GridResults.Columns.Add(commentsColumn);

            DataGridViewLinkColumn downloadsColumn = new DataGridViewLinkColumn();
            downloadsColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            downloadsColumn.HeaderText = "Descargar";
            downloadsColumn.LinkBehavior = LinkBehavior.HoverUnderline;
            downloadsColumn.Name = "DownloadLink";
            downloadsColumn.ReadOnly = true;
            downloadsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            downloadsColumn.Width = 90;
            _GridResults.Columns.Add(downloadsColumn);

            DataGridViewLinkColumn commentsUrlColumn = new DataGridViewLinkColumn();
            commentsUrlColumn.HeaderText = "URL de comentarios";
            commentsUrlColumn.LinkBehavior = LinkBehavior.SystemDefault;
            commentsUrlColumn.Name = "CommentsUrl";
            commentsUrlColumn.Visible = false;
            _GridResults.Columns.Add(commentsUrlColumn);

            _GridResults.CellContentClick += this._GridResults_CellContentClick;
        }

        public async Task<IEnumerable<HtmlNode>> GetEpisodeNodes(string text)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = await _web.LoadFromWebAsync(_SearchUrlStartPart + HttpUtility.UrlEncode(text) + _SearchUrlEndPart);
            _wrapper = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("contenedor_izq")).SingleOrDefault();

            try
            {
                IEnumerable<HtmlNode> episodes = _wrapper.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("menu_detalle_buscador"));
                return episodes;
            }
            catch
            {
                return null;
            }           
        }

        private async void ChangePage(string url)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = await _web.LoadFromWebAsync("https://www.subdivx.com/" + url);
            _wrapper = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("contenedor_izq")).SingleOrDefault();
            IEnumerable<HtmlNode> episodes = _wrapper.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("menu_detalle_buscador"));
            _SubDivXResults.Clear();
            _GridResults.Rows.Clear();
            Application.OpenForms["SubtitleFinderForm"].Controls.Add(FillRowsGridResults(episodes));
            GenerateFooter(_wrapper);
        }

        private void GenerateFooter(HtmlNode wrapper)
        {
            Panel panel = new Panel()
            {
                Anchor = ((System.Windows.Forms.AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right))),
                Location = new Point(0, _GridResults.Location.Y + _GridResults.Size.Height),
                Name = "paginationContainer",
                MinimumSize = new Size(884, 23),
            };
            
            IEnumerable<HtmlNode> pagElements = wrapper.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("pagination")).FirstOrDefault().Descendants().Reverse();
            int coordX = 840;

            if (pagElements.Any())
            {
                foreach (var p in pagElements)
                {
                    switch (p.Name)
                    {
                        case "span":
                            if (p.Attributes["class"].Value == "current")
                            {
                                Button btnActive = new Button()
                                {
                                    Anchor = ((System.Windows.Forms.AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right))),
                                    Enabled = false,
                                    FlatStyle = FlatStyle.Standard,
                                    Location = new Point(coordX, 0),
                                    Name = "btnPagActive",
                                    AutoSize = true,
                                    Size = new Size(32, 23),
                                    MaximumSize = new Size(75, 23),
                                    Text = p.InnerText,
                                    UseVisualStyleBackColor = true
                                };

                                btnActive.FlatAppearance.BorderColor = SystemColors.ControlDark;
                                panel.Controls.Add(btnActive);
                                coordX -= 38;
                            }
                            break;
                        case "a":
                            switch (p.InnerText)
                            {
                                case "Siguiente &#187;":
                                    coordX = 797;

                                    Button btnNextPag = new Button()
                                    {
                                        Anchor = ((System.Windows.Forms.AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right))),
                                        FlatStyle = FlatStyle.Standard,
                                        Location = new Point(coordX, 0),
                                        Name = "btnPag" + p.InnerText,
                                        Size = new Size(75, 23),
                                        Text = "Siguiente",
                                        UseVisualStyleBackColor = true
                                    };
                                    btnNextPag.FlatAppearance.BorderColor = SystemColors.ControlDark;
                                    btnNextPag.Click += (sender, args) =>
                                    {
                                        ChangePage(p.Attributes["href"].Value);
                                    };
                                    panel.Controls.Add(btnNextPag);
                                    coordX -= 38;
                                    break;
                                case "&#171; Anterior":
                                    Button btnPrevPag = new Button()
                                    {
                                        Anchor = ((System.Windows.Forms.AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right))),
                                        FlatStyle = FlatStyle.Standard,
                                        Location = new Point(coordX - 43, 0),
                                        Name = "btnPag" + p.InnerText,
                                        Size = new Size(75, 23),
                                        Text = "Anterior",
                                        UseVisualStyleBackColor = true
                                    };
                                    btnPrevPag.FlatAppearance.BorderColor = SystemColors.ControlDark;
                                    btnPrevPag.Click += (sender, args) =>
                                    {
                                        ChangePage(p.Attributes["href"].Value);
                                    };
                                    panel.Controls.Add(btnPrevPag);
                                    break;
                                default:
                                    Button btnPag = new Button()
                                    {
                                        Anchor = ((System.Windows.Forms.AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right))),
                                        FlatStyle = FlatStyle.Standard,
                                        Location = new Point(coordX, 0),
                                        Name = "btnPag" + p.InnerText,
                                        AutoSize = true,
                                        Size = new Size(32, 23),
                                        MaximumSize = new Size(75, 23),
                                        Text = p.InnerText,
                                        UseVisualStyleBackColor = true
                                    };

                                    btnPag.FlatAppearance.BorderColor = SystemColors.ControlDark;
                                    btnPag.Click += (sender, args) =>
                                    {
                                        ChangePage(p.Attributes["href"].Value);
                                    };
                                    panel.Controls.Add(btnPag);
                                    coordX -= 38;
                                    break;
                            }
                            break;
                    }
                }
            }

            Application.OpenForms["SubtitleFinderForm"].Controls.RemoveByKey("paginationContainer");
            Application.OpenForms["SubtitleFinderForm"].Controls.Add(panel);
        }

        public DataGridView GenerateResults(IEnumerable<HtmlNode> episodes)
        {
            InitGridViewControl();
            GenerateFooter(_wrapper);
            this._SubDivXResults = new List<SubDivXResult>();

            return FillRowsGridResults(episodes);
        }

        private DataGridView FillRowsGridResults(IEnumerable<HtmlNode> episodes)
        {
            foreach (var episode in episodes)
            {
                HtmlNode detailsWrapper = episode.NextSibling;
                IEnumerable<HtmlNode> anchors = detailsWrapper.LastChild.Descendants("a");
                SubDivXResult result = new SubDivXResult();

                result.EpisodeName = episode.FirstChild.FirstChild.InnerText.Substring(14);
                result.Description = detailsWrapper.FirstChild.NextSibling.InnerText;

                var commentsNode = anchors.Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow") && a.Attributes.Contains("onclick")).SingleOrDefault();
                if (commentsNode != null)
                {
                    result.Comments = commentsNode.InnerText;
                    result.CommentsUrl = episode.FirstChild.FirstChild.Attributes["href"].Value;
                }

                result.UploadBy = anchors.Where(u => u.Attributes.Contains("class") && u.Attributes["class"].Value.Equals("link1")).SingleOrDefault().InnerText;
                result.DownloadUrl = episode.FirstChild.FirstChild.Attributes["href"].Value;
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

        private async void _GridResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView currentGridView = (DataGridView)sender;
            DataGridViewRow currentRow = currentGridView.Rows[e.RowIndex];

            if (e.ColumnIndex == 3)
            {
                string commentUrl = currentRow.Cells["CommentsUrl"].Value.ToString();
                var htmlComments = await _web.LoadFromWebAsync(commentUrl);
                var userComments = htmlComments.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("detalle_comentarios"));
                new SubDivXCommentsDialog().ShowDialog(userComments);
            }

            if (e.ColumnIndex == 4)
            {
                System.Diagnostics.Process.Start(currentRow.Cells["DownloadLink"].Value.ToString());
            }
        }
    }
}