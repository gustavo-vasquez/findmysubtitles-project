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
        public static IEnumerable<HtmlNode> TvShows { get; set; }
        public string tvShowId { get; set; }
        protected override string ShowsCatalogUrl { get { return "https://www.tusubtitulo.com/series.php"; } }
        protected override string UrlPrefix { get { return "https://www.tusubtitulo.com"; } }

        public TuSubtituloScraper()
        {
            this.tabCtrlResults = base.NewTabControl();
            tabCtrlResults.Click += tabCtrlResults_Click;

            if (TvShows == null)
                this.SetTvShows();
        }

        protected override void SetTvShows()
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlWeb().Load(ShowsCatalogUrl);
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
                    tvShowUrl = UrlPrefix + anchorTag.Attributes["href"].Value;
                    tvShowId = anchorTag.Attributes["href"].Value.Substring(anchorTag.Attributes["href"].Value.LastIndexOf('/') + 1);
                    break;
                }
            }

            return tvShowUrl;
        }

        public override TabControl GenerateResults(string tvShowUrl)
        {
            HtmlAgilityPack.HtmlDocument tvShowHtml = new HtmlWeb().Load(tvShowUrl);
            HtmlNode tabs = tvShowHtml.DocumentNode.Descendants("span").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "titulo").FirstOrDefault();
            List<HtmlNode> seasonsList = tabs.Descendants("a").ToList();
            int totalIndexSeasons = seasonsList.Count() - 1;
            SeasonURL = new Dictionary<string, string>();

            for (int i = 0; i <= totalIndexSeasons; i++)
            {
                string seasonTitle = "Temporada " + seasonsList[i].InnerText;

                TabPage tab = base.NewTabPage(seasonTitle);
                tabCtrlResults.Controls.Add(tab);

                SeasonURL.Add(seasonTitle, UrlPrefix + "/ajax_loadShow.php?show=" + tvShowId + "&season=" + seasonsList[i].InnerText);

                if (i == totalIndexSeasons)
                    tabCtrlResults.SelectTab(tab);
            }

            return base.RenderizeSeasonTab(SeasonURL.Last().Value, SearchSources.TuSubtitulo);
        }

        protected override void tabCtrlResults_Click(object sender, EventArgs e)
        {
            if (tabCtrlResults.SelectedTab.Controls.Count <= 0)
                Application.OpenForms["SubtitleFinderForm"].Controls.Add(base.RenderizeSeasonTab(SeasonURL[tabCtrlResults.SelectedTab.Text], SearchSources.TuSubtitulo));
        }
    }
}
