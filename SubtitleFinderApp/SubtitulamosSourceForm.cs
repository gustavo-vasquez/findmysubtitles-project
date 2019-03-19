﻿using HtmlAgilityPack;
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
        private Dictionary<string, string> seasonURL;
        private IEnumerable<HtmlNode> tvShows;
        private TabControl tabCtrlResults;

        public SubtitulamosSourceForm()
        {
            InitializeComponent();
            
            var htmldoc = _web.Load("https://www.subtitulamos.tv/shows");
            var showsListDiv = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("container")).SingleOrDefault();            
            tvShows = showsListDiv.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("row"));            

            tabCtrlResults = new TabControl()
            {
                Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right))),
                Appearance = System.Windows.Forms.TabAppearance.FlatButtons,
                Location = new System.Drawing.Point(12, 69),
                Name = "tabCtrlResults",                
                Size = new System.Drawing.Size(860, 381)
            };

            tabCtrlResults.SelectedIndexChanged += tabCtrlResults_SelectedIndexChanged;
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
                    Location = new System.Drawing.Point(4, 25),
                    Size = new System.Drawing.Size(852, 352),
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

        private void RenderizeSeasonTab(object season)
        {
            var htmldoc = (season is string) ? _web.Load((string)season) : (HtmlAgilityPack.HtmlDocument)season;

            IEnumerable<HtmlNode> episodes = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("episodes")).SingleOrDefault().Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("episode"));
            List<SubtitulamosScraper> scraper = new List<SubtitulamosScraper>();

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

            Label lblTitle;
            DataGridView gridDetails;
            int labelOffsetY = 19;
            int gridViewOffsetY = 51;
            int selectedTabIndex = tabCtrlResults.SelectedIndex;

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
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    StandardTab = true
                };

                gridDetails.DefaultCellStyle.SelectionBackColor = gridDetails.DefaultCellStyle.BackColor;
                gridDetails.DefaultCellStyle.SelectionForeColor = gridDetails.DefaultCellStyle.ForeColor;

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

                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(lblTitle);
                gridDetails.Height = gridDetails.Rows.Count * gridDetails.RowTemplate.Height + 30;
                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(gridDetails);
                labelOffsetY = gridDetails.Location.Y + gridDetails.Height + 14;
                gridViewOffsetY = labelOffsetY + lblTitle.Height + 16;
            }

            Controls.Add(tabCtrlResults);
        }

        private void tabCtrlResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderizeSeasonTab(seasonURL[tabCtrlResults.SelectedTab.Text]);
        }

        private void SubtitulamosSourceForm_Load(object sender, EventArgs e)
        {
            
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            SubtitulamosSearch(textBox2.Text, tvShows);
        }
    }
}