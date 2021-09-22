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
    public class TuSubtituloScraper : SourceScraper, ISourceScraper
    {
        public static IEnumerable<HtmlNode> TvShows { get; private set; }
        private string _TvShowId { get; set; }
        protected override string _ShowsCatalogUrl { get { return "https://www.tusubtitulo.com/series.php"; } }
        protected override string _UrlPrefix { get { return "https://www.tusubtitulo.com"; } }

        public TuSubtituloScraper()
        {
            _TabCtrlResults = base.NewTabControl();
            _TabCtrlResults.SelectedIndexChanged += _TabCtrlResults_SelectedIndexChanged;

            if (TvShows == null)
                this.SetTvShows();
        }

        protected override void SetTvShows()
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlWeb().LoadFromBrowser(_ShowsCatalogUrl);
            TvShows = htmldoc.DocumentNode.Descendants("td").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "line0").ToList();
        }

        public override string GetTvShowUrl(string text)
        {
            string tvShowUrl = "";

            foreach (var tvShow in TvShows)
            {
                HtmlNode anchorTag = tvShow.Descendants("a").SingleOrDefault();

                if (text.ToLower() == anchorTag.InnerText.ToLower())
                {
                    tvShowUrl = _UrlPrefix + anchorTag.Attributes["href"].Value;
                    _TvShowId = anchorTag.Attributes["href"].Value.Substring(anchorTag.Attributes["href"].Value.LastIndexOf('/') + 1);
                    break;
                }
            }

            return tvShowUrl;
        }

        public override TabControl GenerateResults(string tvShowUrl)
        {
            HtmlAgilityPack.HtmlDocument tvShowHtml = new HtmlWeb().LoadFromBrowser(tvShowUrl);
            HtmlNode tabs = tvShowHtml.DocumentNode.Descendants("span").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "titulo").FirstOrDefault();
            List<HtmlNode> seasonsList = tabs.Descendants("a").ToList();
            int totalIndexSeasons = seasonsList.Count() - 1;
            _SeasonUrl = new Dictionary<string, string>();

            for (int i = 0; i <= totalIndexSeasons; i++)
            {
                string seasonTitle = "Temporada " + seasonsList[i].InnerText;

                TabPage tab = base.NewTabPage(seasonTitle);
                _TabCtrlResults.Controls.Add(tab);

                _SeasonUrl.Add(seasonTitle, string.Concat(_UrlPrefix, "/show/", _TvShowId, "/", seasonsList[i].InnerText));

                if (i == totalIndexSeasons)
                    _TabCtrlResults.SelectTab(tab);
            }

            return base.RenderizeSeasonTab(_SeasonUrl.Last().Value, SearchSources.TuSubtitulo);
        }

        protected override void _TabCtrlResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_TabCtrlResults.SelectedTab.Controls.Count <= 0)
                Application.OpenForms["SubtitleFinderForm"].Controls.Add(base.RenderizeSeasonTab(_SeasonUrl[_TabCtrlResults.SelectedTab.Text], SearchSources.TuSubtitulo));
        }
    }
}
