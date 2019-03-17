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

            var tvShowHtml = new HtmlWeb() { OverrideEncoding = Encoding.Default }.Load(tvShowURL);            
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
            var scraper = new List<SubtitulamosScraper>();

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
    }
}
