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
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.Default };
        private const string sourceURL = "https://www.subtitulamos.tv/";
        private List<SubtitulamosScraper> scraper = new List<SubtitulamosScraper>();

        public SubtitulamosSourceForm()
        {
            InitializeComponent();

            string search = "the walking dead";
            var htmldoc = _web.Load("https://www.subtitulamos.tv/shows");
            var showsListDiv = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("container")).SingleOrDefault();
            //var tvShows = showsListDiv.SelectNodes("//div[@class=\"row\"]").ToList();
            var tvShows = showsListDiv.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("row"));            
            string tvShowURL = "";

            foreach (var show in tvShows)
            {
                if (search.ToLower() == show.Descendants("a").SingleOrDefault().InnerText.ToLower())
                {
                    tvShowURL = sourceURL + show.Descendants("a").SingleOrDefault().Attributes["href"].Value;
                    break;
                }
            }

            textBox1.Text = string.Join("<>", search, tvShowURL);

            var tvShowHtml = new HtmlWeb().Load(tvShowURL);            
            var tabs = tvShowHtml.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("tabs")).SingleOrDefault();
            var seasonsList = tabs.Descendants("ul").SingleOrDefault().Descendants("li");            

            foreach (var season in seasonsList)
            {
                if(season.Attributes["class"].Value.Equals("is-active"))
                {
                    textBox2.Text = season.Descendants("a").SingleOrDefault().InnerText;
                    tabControl1.SelectTab("tabPage9");
                }
            }

            var episodes = tvShowHtml.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("episodes")).SingleOrDefault().Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("episode"));            

            foreach (var episode in episodes)
            {
                var scraperItem = new SubtitulamosScraper();
                scraperItem.EpisodeName = episode.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.Equals("episode-name")).SingleOrDefault().InnerText;                

                foreach (var language in episode.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.Equals("subtitle-language")))
                {
                    var details = language.NextSibling.NextSibling.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("sub"));

                    foreach (var detail in details)
                    {
                        var versionName = detail.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("version-name")).SingleOrDefault();
                        var progressPercentage = detail.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("progress_percentage")).SingleOrDefault();
                        var downloadUrl = detail.Descendants("a").Where(a => a.Attributes.Contains("href")).SingleOrDefault();

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
        }

        private void SubtitulamosSourceForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Label lblTitle;
            DataGridView gridDetails;
            int labelOffsetY = 19;
            int gridViewOffsetY = 51;
            tabPage9.AutoScroll = true;

            foreach (var item in scraper)
            {
                lblTitle = new Label()
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Location = new System.Drawing.Point(6, labelOffsetY),
                    Text = item.EpisodeName
                };                

                gridDetails = new DataGridView()
                {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    AllowUserToOrderColumns = false,                    
                    BackgroundColor = System.Drawing.SystemColors.Control,
                    BorderStyle = System.Windows.Forms.BorderStyle.None,
                    ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    Location = new System.Drawing.Point(6, gridViewOffsetY),
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    Size = new System.Drawing.Size(800, 120),
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect                    
                };

                DataGridViewTextBoxColumn Language = new System.Windows.Forms.DataGridViewTextBoxColumn()
                {
                    HeaderText = "Idioma",
                    Name = "Language",
                    ReadOnly = true,
                    Width = 190,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewTextBoxColumn Version = new System.Windows.Forms.DataGridViewTextBoxColumn()
                {
                    HeaderText = "Versión",
                    Name = "Version",
                    ReadOnly = true,
                    Width = 250,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewTextBoxColumn Progress = new System.Windows.Forms.DataGridViewTextBoxColumn()
                {
                    HeaderText = "Progreso",
                    Name = "Progress",
                    ReadOnly = true,
                    Width = 60,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewLinkColumn DownloadLink = new System.Windows.Forms.DataGridViewLinkColumn()
                {
                    HeaderText = "Descarga",
                    Name = "DownloadLink",
                    ReadOnly = true,
                    Width = 110,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                gridDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Language, Version, Progress, DownloadLink });

                foreach (var detail in item.SubtitleDetails)
                {                                        
                    gridDetails.Rows.Add(detail.SubtitleLanguage, detail.VersionName, detail.ProgressPercentage, detail.DownloadUrl);
                }

                tabPage9.Controls.Add(lblTitle);
                gridDetails.Height = gridDetails.Rows.Count * gridDetails.RowTemplate.Height + 30;
                tabPage9.Controls.Add(gridDetails);
                labelOffsetY = gridDetails.Location.Y + gridDetails.Height + 14;
                gridViewOffsetY = labelOffsetY + lblTitle.Height + 16;
            }
        }
    }
}
