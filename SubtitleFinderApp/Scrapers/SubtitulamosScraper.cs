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
        public static IEnumerable<HtmlNode> TvShows { get; private set; }
        protected override string _ShowsCatalogUrl { get { return "https://www.subtitulamos.tv/shows"; } }
        protected override string _UrlPrefix { get { return "https://www.subtitulamos.tv/"; } }

        public SubtitulamosScraper()
        {
            _TabCtrlResults = base.NewTabControl();
            _TabCtrlResults.SelectedIndexChanged += _TabCtrlResults_SelectedIndexChanged;

            if (TvShows == null)
                this.SetTvShows();
        }

        protected override void SetTvShows()
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlWeb().Load(_ShowsCatalogUrl);
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
                    tvShowUrl = _UrlPrefix + tvShow.Descendants("a").SingleOrDefault().Attributes["href"].Value;
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
            _SeasonUrl = new Dictionary<string, string>();

            foreach (var season in seasonsList)
            {
                string seasonTitle = season.Descendants("a").SingleOrDefault().InnerText;

                TabPage tab = base.NewTabPage(seasonTitle);
                _TabCtrlResults.Controls.Add(tab);

                _SeasonUrl.Add(seasonTitle, _UrlPrefix + season.Descendants("a").SingleOrDefault().Attributes["href"].Value.Substring(1));

                if (season.Attributes["class"].Value.Equals("is-active"))
                    _TabCtrlResults.SelectTab(tab);
            }

            return base.RenderizeSeasonTab(_SeasonUrl.Last().Value, SearchSources.Subtitulamos);
        }

        protected override void _TabCtrlResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_TabCtrlResults.SelectedTab.Controls.Count <= 0)
                Application.OpenForms["SubtitleFinderForm"].Controls.Add(base.RenderizeSeasonTab(_SeasonUrl[_TabCtrlResults.SelectedTab.Text], SearchSources.Subtitulamos));
        }
    }
}
