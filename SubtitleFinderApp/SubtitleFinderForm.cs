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
        private SubDivXScraper2 subdivx = new SubDivXScraper2();
        private DataGridView gridResults;

        //private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.GetEncoding("ISO-8859-1"), AutoDetectEncoding = false };
        private const string sourceURL = "https://www.subtitulamos.tv/";
        private const string sourceTuSubtituloURL = "https://www.tusubtitulo.com";
        private Dictionary<string, string> seasonURL;
        private IEnumerable<HtmlNode> tvShows;
        private TabControl tabCtrlResults;

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
            //if (e.StateChanged != DataGridViewElementStates.Selected)
            //    return;

            //statusbarLabel.Text = subdivx.Details[e.Row.Index].InnerText;
        }        

        private void gridResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                dialogSaveSubtitle.FileName = string.Join(" ", currentRow.Cells["Title"].Value.ToString(), "-", subtitleId);
                dialogSaveSubtitle.DefaultExt = "rar";
                dialogSaveSubtitle.Filter = "Archivos RAR (*.rar)|*.rar|Todos los archivos (*.*)|*.*";

                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WebClient wClient = new WebClient();
                    wClient.DownloadFile(downloadUrl, dialogSaveSubtitle.FileName);
                }
            }
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
                            SubDivXScraper.InitGridViewControl(ref gridResults);
                            gridResults.CellContentClick += this.gridResults_CellContentClick;
                            gridResults.RowStateChanged += this.gridResults_RowStateChanged;
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
            HtmlNode wrapper = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("contenedor_izq")).SingleOrDefault();
            IEnumerable<HtmlNode> episodes = wrapper.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("menu_detalle_buscador"));

            SubDivXScraper scraper = new SubDivXScraper();
            scraper.FillResults(ref gridResults, episodes);
        }

        private void searchThroughSubtitulamos(string text)
        {
            if (this.Controls.ContainsKey("gridResults"))
            {
                this.Controls.Remove(gridResults);
                gridResults.Rows.Clear();
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
            IEnumerable<HtmlNode> episodes;

            switch (sourceName)
            {
                case SearchSources.TuSubtitulo:
                    episodes = htmldoc.DocumentNode.Descendants("table");

                    foreach (HtmlNode episode in episodes)
                    {
                        TuSubtituloScraper scraperItem = new TuSubtituloScraper();
                        scraperItem.SetEpisodeData(episode, "https:");
                        scraper.Add(scraperItem);
                    }
                    break;
                case SearchSources.Subtitulamos:
                    episodes = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("episodes")).SingleOrDefault().Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("episode"));

                    foreach (HtmlNode episode in episodes)
                    {
                        SubtitulamosScraper scraperItem = new SubtitulamosScraper();
                        scraperItem.SetEpisodeData(episode, sourceURL);
                        scraper.Add(scraperItem);
                    }
                    break;
                default:
                    MessageBox.Show("No se pudo generar los resultados. Pruebe de nuevo.");
                    break;
            }

            tabCtrlResults = new Wrappers.TabControlWrapper(tabCtrlResults).SetControls(scraper);

            this.Controls.Add(tabCtrlResults);
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