using HtmlAgilityPack;
using SubtitleFinderApp.Scrapers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp
{
    public partial class SubtitulamosSourceForm : Form
    {
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.GetEncoding("ISO-8859-1"), AutoDetectEncoding = false };
        private const string sourceURL = "https://www.subtitulamos.tv/";        
        private Dictionary<string, string> seasonURL;
        private IEnumerable<HtmlNode> tvShows;
        private TabControl tabCtrlResults;

        public SubtitulamosSourceForm()
        {
            InitializeComponent();
            
            HtmlAgilityPack.HtmlDocument htmldoc = _web.Load("https://www.subtitulamos.tv/shows");
            HtmlNode showsListDiv = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("container")).SingleOrDefault();            
            tvShows = showsListDiv.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("row"));            

            tabCtrlResults = new TabControl()
            {
                Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right))),
                Appearance = TabAppearance.Normal,
                Location = new Point(12, 69),
                Name = "tabCtrlResults",
                Size = new Size(860, 420)
            };

            tabCtrlResults.Click += tabCtrlResults_Click;
        }

        public void SubtitulamosSearch(string search, IEnumerable<HtmlNode> tvShows)
        {
            tabCtrlResults.Controls.Clear();
            string tvShowURL = "";

            foreach (var show in tvShows)
            {
                if (search.ToLower() == show.Descendants("a").SingleOrDefault().InnerText.ToLower())
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

            RenderizeSeasonTab(tvShowHtml);
        }

        private void RenderizeSeasonTab(object htmlOrUrl)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = (htmlOrUrl is HtmlAgilityPack.HtmlDocument) ? (HtmlAgilityPack.HtmlDocument)htmlOrUrl : new HtmlWeb().Load((string)htmlOrUrl);

            IEnumerable<HtmlNode> episodes = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("episodes")).SingleOrDefault().Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("episode"));
            List<SubtitulamosScraperData> scraper = new List<SubtitulamosScraperData>();

            foreach (var episode in episodes)
            {
                SubtitulamosScraperData scraperItem = new SubtitulamosScraperData();
                scraperItem.EpisodeName = System.Web.HttpUtility.HtmlDecode(episode.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.Equals("episode-name")).SingleOrDefault().InnerText);

                foreach (var language in episode.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.Equals("subtitle-language")))
                {
                    IEnumerable<HtmlNode> details = language.NextSibling.NextSibling.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("sub"));

                    foreach (HtmlNode detail in details)
                    {
                        HtmlNode versionName = detail.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("version-name")).SingleOrDefault();
                        HtmlNode progressPercentage = detail.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("progress_percentage")).SingleOrDefault();
                        HtmlNode downloadUrl = detail.Descendants("a").Where(a => a.Attributes.Contains("href")).SingleOrDefault();

                        scraperItem.SubtitleDetails.Add(new SubtitleDetails()
                        {
                            SubtitleLanguage = language.InnerText,
                            VersionName = versionName.InnerText,
                            ProgressPercentage = progressPercentage.InnerText.Trim(),
                            DownloadUrl = (downloadUrl != null) ? sourceURL + downloadUrl.Attributes["href"].Value.Substring(1) : ""
                        });
                    }
                }

                scraper.Add(scraperItem);
            }

            Label lblTitle;
            DataGridView gridDetails;
            int labelOffsetY = 19;
            int gridViewOffsetY = 51;
            int selectedTabIndex = tabCtrlResults.SelectedIndex;

            foreach (SubtitulamosScraperData item in scraper)
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
                gridDetails.Height = gridDetails.Rows.Count * gridDetails.RowTemplate.Height + 30;
                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(gridDetails);
                labelOffsetY = gridDetails.Location.Y + gridDetails.Height + 14;
                gridViewOffsetY = labelOffsetY + lblTitle.Height + 16;
                gridDetails.CellContentClick += gridDetails_CellContentClick;
            }

            Controls.Add(tabCtrlResults);
        }

        private void tabCtrlResults_Click(object sender, EventArgs e)
        {
            if (tabCtrlResults.SelectedTab.Controls.Count <= 0)
                RenderizeSeasonTab(seasonURL[tabCtrlResults.SelectedTab.Text]);
        }

        private void SubtitulamosSourceForm_Load(object sender, EventArgs e)
        {
            
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            SubtitulamosSearch(textBox2.Text, tvShows);
        }

        private void gridDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DataGridView currentGridView = (DataGridView)sender;
                Label previousTitle = (Label)GetNextControl(currentGridView, false);
                saveFileDialog1.FileName = string.Join("_", previousTitle.Text, currentGridView.CurrentRow.Cells[1].Value.ToString().Replace('/', '_'), currentGridView.CurrentRow.Cells[0].Value.ToString());
                var result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var wClient = new System.Net.WebClient();
                    wClient.DownloadFile(currentGridView.CurrentRow.Cells[3].Value.ToString(), saveFileDialog1.FileName);
                }
            }
        }
    }
}