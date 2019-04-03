using HtmlAgilityPack;
using SubtitleFinderApp.Scrapers;
using SubtitleFinderApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SubtitleFinderApp
{
    public partial class SubtitleFinderForm : Form
    {
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.Default };        
        private SubDivXScraper subdivx = new SubDivXScraper();
        private DataGridView gridResults;

        //private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.GetEncoding("ISO-8859-1"), AutoDetectEncoding = false };
        private const string sourceURL = "https://www.subtitulamos.tv/";
        private const string sourceTuSubtituloURL = "https://www.tusubtitulo.com";
        private Dictionary<string, string> seasonURL;
        private IEnumerable<HtmlNode> tvShows;
        private TabControl tabCtrlResults;
        private TabControl tabCtrl_tuSubtitulo;

        public SubtitleFinderForm()
        {
            InitializeComponent();
        }

        private void SubtitleFinderForm_Load(object sender, EventArgs e)
        {
            this.rdoBtnSubDivX.PerformClick();            
        }

        private void DoSearch(string text)
        {
            RadioButton checkedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

            if(checkedButton != null)
            {
                switch (checkedButton.Name)
                {
                    case "rdoBtnSubDivX":
                        searchThroughSubDivX(text);
                        break;
                    case "rdoBtnTuSubtitulo":
                        searchThroughTuSubtitulo(text);
                        break;
                    case "rdoBtnSubtitulamos":                        
                        searchThroughSubtitulamos(text);
                        break;
                }
            }
            else
                MessageBox.Show("Elija alguno de los 3 proveedores de subtítulos.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch(txtSearch.Text);
            this.Controls.Remove(picBoxAppImage);
        }

        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            string appInfoText = String.Join(
                    null,
                    ProductInfo.Product,
                    Environment.NewLine,
                    ProductInfo.Description,
                    Environment.NewLine,
                    Environment.NewLine,
                    ProductInfo.Copyright,
                    Environment.NewLine,
                    Environment.NewLine,
                    "Versión: ",
                    System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
                );
            DialogResult AppInfoWindow = MessageBox.Show(appInfoText, "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gridResults_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
                return;

            statusbarLabel.Text = subdivx.Details[e.Row.Index].InnerText;
        }        

        private void gridResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string commentUrl = subdivx.Comments[e.RowIndex].Attributes["href"].Value;
            string queryString = commentUrl.Substring(commentUrl.IndexOf('?'));
            string subtitleId = HttpUtility.ParseQueryString(queryString).Get("idsub");

            if (e.ColumnIndex == 2)
            {                
                var htmlComments = new HtmlWeb() { OverrideEncoding = Encoding.Default }.Load("https://www.subdivx.com/popcoment.php?idsub=" + HttpUtility.UrlEncode(subtitleId));
                var userComments = htmlComments.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("pop_upcoment"));                
                new SubDivXCommentsDialog().ShowDialog(userComments);
            }

            if (e.ColumnIndex == 3)
            {
                dialogSaveSubtitle.FileName = string.Join(" ", subtitleId, "-", subdivx.Title[e.RowIndex].InnerText);
                dialogSaveSubtitle.DefaultExt = "rar";
                dialogSaveSubtitle.Filter = "Archivos RAR (*.rar)|*.rar|Todos los archivos (*.*)|*.*";

                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var wClient = new WebClient();
                    wClient.DownloadFile(subdivx.DownloadLink[e.RowIndex].Attributes["href"].Value, dialogSaveSubtitle.FileName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var myForm = new SubtitulamosSourceForm();
            myForm.Show();
        }

        private void picBoxSubDivX_Click(object sender, EventArgs e)
        {
            rdoBtnSubDivX.Checked = true;
        }

        private void picBoxTuSubtitulo_Click(object sender, EventArgs e)
        {
            rdoBtnTuSubtitulo.Checked = true;
        }

        private void picBoxSubtitulamos_Click(object sender, EventArgs e)
        {
            rdoBtnSubtitulamos.Checked = true;
        }

        private void radioBtnSources_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton currentRadioButton = sender as RadioButton;

            if(currentRadioButton != null && currentRadioButton.Checked)
            {
                switch(currentRadioButton.Name)
                {
                    case "rdoBtnSubDivX":
                        if(gridResults == null)
                        {
                            gridResults = subdivx.InitSubDivXGridResults();
                            gridResults.CellContentClick += new DataGridViewCellEventHandler(this.gridResults_CellContentClick);
                            gridResults.RowStateChanged += new DataGridViewRowStateChangedEventHandler(this.gridResults_RowStateChanged);
                        }
                        break;
                    case "rdoBtnTuSubtitulo":
                        if (tabCtrlResults == null)
                        {
                            tabCtrlResults = new TabControl()
                            {
                                Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                                Appearance = TabAppearance.Normal,
                                Location = new Point(12, 110),
                                Name = "tabCtrlResults",
                                Size = new Size(860, 420)
                            };

                            tabCtrlResults.Click += tabCtrlResults_Click;
                        }
                        break;
                    case "rdoBtnSubtitulamos":
                        if(tabCtrlResults == null)
                        {
                            tabCtrlResults = new TabControl()
                            {
                                Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                                Appearance = TabAppearance.Normal,
                                Location = new Point(12, 110),
                                Name = "tabCtrlResults",
                                Size = new Size(860, 420)
                            };

                            tabCtrlResults.Click += tabCtrlResults_Click;
                        }
                        break;
                }
            }
        }

        private void searchThroughSubDivX(string text)
        {
            if (this.Controls.ContainsKey("tabCtrl_tuSubtitulo"))
            {
                this.Controls.Remove(tabCtrl_tuSubtitulo);
                tabCtrl_tuSubtitulo.Controls.Clear();
            }

            if (this.Controls.ContainsKey("tabCtrlResults"))
            {
                this.Controls.Remove(tabCtrlResults);
                tabCtrlResults.Controls.Clear();
            }                           

            if (!this.Controls.ContainsKey("gridResults"))
                this.Controls.Add(gridResults);
            else
                gridResults.Rows.Clear();

            HtmlAgilityPack.HtmlDocument htmldoc = _web.Load("https://www.subdivx.com/index.php?buscar=" + HttpUtility.UrlEncode(text) + "&accion=5&masdesc=&subtitulos=1&realiza_b=1");
            subdivx.Title = htmldoc.DocumentNode.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("titulo_menu_izq")).ToList();
            subdivx.Description = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub")).ToList();
            subdivx.Details = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub_datos")).ToList();

            subdivx.Comments = new List<HtmlNode>();
            subdivx.DownloadLink = new List<HtmlNode>();

            foreach (HtmlNode detail in subdivx.Details)
            {
                subdivx.Comments.Add(detail.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).FirstOrDefault());
                subdivx.DownloadLink.Add(detail.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).LastOrDefault());
            }

            foreach (HtmlNode title in subdivx.Title)
            {
                gridResults.Rows.Add(title.InnerText.Substring(13));
            }

            for (int i = 0; i < subdivx.Description.Count; i++)
            {
                gridResults.Rows[i].Cells["Description"].Value = subdivx.Description[i].InnerText;
            }

            for (int i = 0; i < subdivx.Comments.Count; i++)
            {
                gridResults.Rows[i].Cells["Comments"].Value = subdivx.Comments[i].InnerText;
            }

            //gridResults.Refresh();
        }

        private void searchThroughSubtitulamos(string text)
        {
            if (this.Controls.ContainsKey("gridResults"))
            {
                this.Controls.Remove(gridResults);
                gridResults.Rows.Clear();
            }

            if (this.Controls.ContainsKey("tabCtrl_tuSubtitulo"))
            {
                this.Controls.Remove(tabCtrl_tuSubtitulo);
                tabCtrl_tuSubtitulo.Controls.Clear();
            }

            if (!this.Controls.ContainsKey("tabCtrlResults"))
                this.Controls.Add(tabCtrlResults);
            else
                tabCtrlResults.Controls.Clear();

            if(tvShows == null)
            {
                HtmlAgilityPack.HtmlDocument htmldoc = _web.Load("https://www.subtitulamos.tv/shows");
                HtmlNode showsListDiv = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("container")).SingleOrDefault();
                tvShows = showsListDiv.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("row"));
            }

            string tvShowURL = "";

            foreach (var show in tvShows)
            {
                if (text.ToLower() == show.Descendants("a").SingleOrDefault().InnerText.ToLower())
                {
                    tvShowURL = sourceURL + show.Descendants("a").SingleOrDefault().Attributes["href"].Value;
                    break;
                }
            }

            var tvShowHtml = new HtmlWeb().Load(tvShowURL);
            var tabs = tvShowHtml.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("tabs")).SingleOrDefault();
            var seasonsList = tabs.Descendants("ul").SingleOrDefault().Descendants("li");
            seasonURL = new Dictionary<string, string>();

            foreach (var season in seasonsList)
            {
                TabPage tab = new TabPage()
                {
                    Location = new Point(4, 25),
                    Size = new Size(852, 352),
                    Text = season.Descendants("a").SingleOrDefault().InnerText,
                    UseVisualStyleBackColor = true,
                    AutoScroll = true
                };

                tabCtrlResults.Controls.Add(tab);

                if (!season.Attributes["class"].Value.Equals("is-active"))
                    seasonURL.Add(season.Descendants("a").SingleOrDefault().InnerText, sourceURL + season.Descendants("a").SingleOrDefault().Attributes["href"].Value.Substring(1));
                else
                    tabCtrlResults.SelectTab(tab);
            }

            RenderizeSeasonTab(tvShowHtml, SearchSources.Subtitulamos);
        }

        private void searchThroughTuSubtitulo(string text)
        {
            if (this.Controls.ContainsKey("gridResults"))
            {
                this.Controls.Remove(gridResults);
                gridResults.Rows.Clear();
            }

            //if (this.Controls.ContainsKey("tabCtrlResults"))
            //{
            //    this.Controls.Remove(tabCtrlResults);
            //    tabCtrlResults.Controls.Clear();
            //}

            if (!this.Controls.ContainsKey("tabCtrlResults"))
                this.Controls.Add(tabCtrlResults);
            else
                tabCtrlResults.Controls.Clear();

            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlWeb().Load("https://www.tusubtitulo.com/series.php");
            var tvShows = htmldoc.DocumentNode.Descendants("td").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "line0").ToList();
            string tvShowURL = "";
            string tvShowId = "";

            foreach (var tvShow in tvShows)
            {
                HtmlNode anchorTag = tvShow.Descendants("a").SingleOrDefault();

                if (text.ToLower() == anchorTag.InnerText.ToLower())
                {
                    tvShowURL = sourceTuSubtituloURL + anchorTag.Attributes["href"].Value;
                    tvShowId = anchorTag.Attributes["href"].Value.Substring(anchorTag.Attributes["href"].Value.LastIndexOf('/') + 1);
                    break;
                }
            }

            var tvShowHtml = new HtmlWeb().Load(tvShowURL);
            var tabs = tvShowHtml.DocumentNode.Descendants("span").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "titulo").FirstOrDefault();
            List<HtmlNode> seasonsList = tabs.Descendants("a").ToList();
            int totalIndexSeasons = seasonsList.Count() - 1;
            seasonURL = new Dictionary<string, string>();

            for(int i = 0; i <= totalIndexSeasons; i++)
            {
                string seasonTitle = "Temporada " + seasonsList[i].InnerText;

                TabPage tab = new TabPage()
                {
                    Location = new Point(4, 25),
                    Size = new Size(852, 352),
                    Text = seasonTitle,
                    UseVisualStyleBackColor = true,
                    AutoScroll = true
                };

                tabCtrlResults.Controls.Add(tab);

                seasonURL.Add(seasonTitle, "https://www.tusubtitulo.com/ajax_loadShow.php?show=" + tvShowId + "&season=" + seasonsList[i].InnerText);

                if (i == totalIndexSeasons)
                    tabCtrlResults.SelectTab(tab);
            }

            RenderizeSeasonTab(seasonURL.Last().Value, SearchSources.TuSubtitulo);
        }


        private void RenderizeSeasonTab(object htmlOrUrl, SearchSources sourceName)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = (htmlOrUrl is HtmlAgilityPack.HtmlDocument) ? (HtmlAgilityPack.HtmlDocument)htmlOrUrl : new HtmlWeb().Load((string)htmlOrUrl);
            List<ISourceScraper> scraper = new List<ISourceScraper>();

            if (sourceName == SearchSources.TuSubtitulo)
            {
                IEnumerable<HtmlNode> episodes = htmldoc.DocumentNode.Descendants("table");                

                foreach (HtmlNode episode in episodes)
                {
                    TuSubtituloScraper scraperItem = new TuSubtituloScraper();
                    scraperItem.SetEpisodeData(episode, "https:");
                    scraper.Add(scraperItem);
                }
            }
            else
            {
                IEnumerable<HtmlNode> episodes = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("episodes")).SingleOrDefault().Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("episode"));                

                foreach (HtmlNode episode in episodes)
                {
                    SubtitulamosScraper scraperItem = new SubtitulamosScraper();
                    scraperItem.SetEpisodeData(episode, sourceURL);
                    scraper.Add(scraperItem);
                }
            }

            Label lblTitle;
            DataGridView gridDetails;
            int labelOffsetY = 19;
            int gridViewOffsetY = 51;
            int selectedTabIndex = tabCtrlResults.SelectedIndex; 
            //int selectedTabIndex = tabCtrl_tuSubtitulo.SelectedIndex;

            foreach (var item in scraper)
            {
                lblTitle = new Label()
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))),
                    Location = new Point(6, labelOffsetY),
                    Text = item.EpisodeName
                };

                gridDetails = new DataGridView()
                {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    AllowUserToOrderColumns = false,
                    BackgroundColor = SystemColors.ControlLightLight,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    Location = new Point(6, gridViewOffsetY),
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    Size = new Size(800, 120),
                    StandardTab = true
                };

                gridDetails.DefaultCellStyle.SelectionBackColor = gridDetails.DefaultCellStyle.BackColor;
                gridDetails.DefaultCellStyle.SelectionForeColor = gridDetails.DefaultCellStyle.ForeColor;

                DataGridViewTextBoxColumn Language = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Idioma",
                    Name = "Language",
                    ReadOnly = true,
                    Width = 190,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewTextBoxColumn Version = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Versión",
                    Name = "Version",
                    ReadOnly = true,
                    Width = 250,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewTextBoxColumn Progress = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Progreso",
                    Name = "Progress",
                    ReadOnly = true,
                    Width = 60,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewLinkColumn DownloadLink = new DataGridViewLinkColumn()
                {
                    HeaderText = "Descarga",
                    Name = "DownloadLink",
                    ReadOnly = true,
                    Width = 110,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                gridDetails.Columns.AddRange(new DataGridViewColumn[] { Language, Version, Progress, DownloadLink });

                foreach (SubtitleDetails detail in item.SubtitleDetails)
                {
                    gridDetails.Rows.Add(detail.SubtitleLanguage, detail.VersionName, detail.ProgressPercentage, detail.DownloadUrl);
                }

                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(lblTitle);
                //tabCtrl_tuSubtitulo.TabPages[selectedTabIndex].Controls.Add(lblTitle);
                gridDetails.Height = gridDetails.Rows.Count * gridDetails.RowTemplate.Height + 30;
                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(gridDetails);
                //tabCtrl_tuSubtitulo.TabPages[selectedTabIndex].Controls.Add(gridDetails);
                labelOffsetY = gridDetails.Location.Y + gridDetails.Height + 14;
                gridViewOffsetY = labelOffsetY + lblTitle.Height + 16;
                gridDetails.CellContentClick += gridDetails_CellContentClick;
            }

            Controls.Add(tabCtrlResults);
            //Controls.Add(tabCtrl_tuSubtitulo);
        }

        private void tabCtrlResults_Click(object sender, EventArgs e)
        {
            if (tabCtrlResults.SelectedTab.Controls.Count <= 0)
                if(rdoBtnTuSubtitulo.Checked)
                    RenderizeSeasonTab(seasonURL[tabCtrlResults.SelectedTab.Text], SearchSources.TuSubtitulo);
                else
                    RenderizeSeasonTab(seasonURL[tabCtrlResults.SelectedTab.Text], SearchSources.Subtitulamos);
        }

        private void gridDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DataGridView currentGridView = (DataGridView)sender;
                Label previousTitle = (Label)GetNextControl(currentGridView, false);
                dialogSaveSubtitle.FileName = string.Join("_", previousTitle.Text, currentGridView.CurrentRow.Cells[1].Value.ToString().Replace('/', '_'), currentGridView.CurrentRow.Cells[0].Value.ToString());
                dialogSaveSubtitle.DefaultExt = "srt";
                dialogSaveSubtitle.Filter = "Archivos SRT (*.srt)|*.srt|Todos los archivos (*.*)|*.*";

                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var wClient = new WebClient();
                    wClient.DownloadFile(currentGridView.CurrentRow.Cells[3].Value.ToString(), dialogSaveSubtitle.FileName);
                }
            }
        }
    }
}