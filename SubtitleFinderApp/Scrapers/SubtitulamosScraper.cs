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
            HtmlAgilityPack.HtmlDocument htmldoc = _web.Load(_ShowsCatalogUrl);
            IEnumerable<HtmlNode> showsWrapper = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("shows-in-letter"));
            TvShows = showsWrapper;
        }

        public override string GetTvShowUrl(string text)
        {
            string tvShowUrl = string.Empty;
            text = text.ToLower();

            foreach (var tvShow in TvShows)
            {
                var show = tvShow.Descendants("a").FirstOrDefault(x => x.Element("span").InnerText.ToLower() == text);

                if (show != null)
                {
                    tvShowUrl = _UrlPrefix + show.Attributes["href"].Value;
                    break;
                }
            }

            return tvShowUrl;
        }

        public override TabControl GenerateResults(string tvShowUrl)
        {
            HtmlAgilityPack.HtmlDocument tvShowHtml = _web.Load(tvShowUrl);
            HtmlNode tabs = tvShowHtml.DocumentNode.Descendants("div").Where(d => d.Id == "season-choices").SingleOrDefault();
            IEnumerable<HtmlNode> seasonsList = tabs.Descendants("a");
            _SeasonUrl = new Dictionary<string, string>();

            foreach (var season in seasonsList)
            {
                string seasonTitle = "Temporada " + season.InnerText;

                TabPage tab = base.NewTabPage(seasonTitle);
                _TabCtrlResults.Controls.Add(tab);

                _SeasonUrl.Add(seasonTitle, _UrlPrefix + season.Attributes["href"].Value.Substring(1));

                if (season.Attributes["class"].Value.Equals("choice selected"))
                    _TabCtrlResults.SelectTab(tab);
            }

            return base.RenderizeSeasonTab(_SeasonUrl.Last().Value, SearchSources.Subtitulamos);
        }

        protected override void _TabCtrlResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control.ControlCollection controls = Application.OpenForms["SubtitleFinderForm"].Controls;
            controls["pbSearchingSubs"].Visible = true;

            if (_TabCtrlResults.SelectedTab.Controls.Count <= 0)
                controls.Add(base.RenderizeSeasonTab(_SeasonUrl[_TabCtrlResults.SelectedTab.Text], SearchSources.Subtitulamos));

            controls["pbSearchingSubs"].Visible = false;
        }
    }
}
