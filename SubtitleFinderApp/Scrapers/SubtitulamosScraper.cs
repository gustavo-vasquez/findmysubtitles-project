using HtmlAgilityPack;
using SubtitleFinderApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp.Scrapers
{
    public class SubtitulamosScraper : SourceScraper, ISourceScraper
    {
        public static IEnumerable<HtmlNode> TvShows { get; set; }
        protected override string ShowsCatalogUrl { get { return "https://www.subtitulamos.tv/shows"; } }
        protected override string UrlPrefix { get { return "https://www.subtitulamos.tv/"; } }

        public SubtitulamosScraper()
        {
            tabCtrlResults = base.NewTabControl();
            tabCtrlResults.Click += tabCtrlResults_Click;

            if (TvShows == null)
                this.SetTvShows();
        }

        protected override void SetTvShows()
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlWeb().Load(ShowsCatalogUrl);
            HtmlNode showsListDiv = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("container")).SingleOrDefault();
            TvShows = showsListDiv.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("row"));
        }

        public override string GetTvShowUrl(string text)
        {
            string tvShowUrl = "";

            foreach (var tvShow in TvShows)
            {
                if (text.ToLower() == tvShow.Descendants("a").SingleOrDefault().InnerText.ToLower())
                {
                    tvShowUrl = UrlPrefix + tvShow.Descendants("a").SingleOrDefault().Attributes["href"].Value;
                    break;
                }
            }

            return tvShowUrl;
        }

        public override TabControl GenerateResults(string tvShowUrl)
        {
            HtmlAgilityPack.HtmlDocument tvShowHtml = new HtmlWeb().Load(tvShowUrl);
            HtmlNode tabs = tvShowHtml.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("tabs")).SingleOrDefault();
            IEnumerable<HtmlNode> seasonsList = tabs.Descendants("ul").SingleOrDefault().Descendants("li");
            SeasonURL = new Dictionary<string, string>();

            foreach (var season in seasonsList)
            {
                string seasonTitle = season.Descendants("a").SingleOrDefault().InnerText;

                TabPage tab = base.NewTabPage(seasonTitle);
                tabCtrlResults.Controls.Add(tab);

                SeasonURL.Add(seasonTitle, UrlPrefix + season.Descendants("a").SingleOrDefault().Attributes["href"].Value.Substring(1));

                if (season.Attributes["class"].Value.Equals("is-active"))
                    tabCtrlResults.SelectTab(tab);
            }

            return base.RenderizeSeasonTab(SeasonURL.Last().Value, SearchSources.Subtitulamos);
        }

        protected override void tabCtrlResults_Click(object sender, EventArgs e)
        {
            if (tabCtrlResults.SelectedTab.Controls.Count <= 0)
                Application.OpenForms["SubtitleFinderForm"].Controls.Add(base.RenderizeSeasonTab(SeasonURL[tabCtrlResults.SelectedTab.Text], SearchSources.Subtitulamos));
        }
    }
}
