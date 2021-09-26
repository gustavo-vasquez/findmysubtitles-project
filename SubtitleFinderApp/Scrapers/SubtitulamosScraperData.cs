using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public class SubtitulamosScraperData : ISourceScraperData
    {
        public string EpisodeName { get; set; }
        public List<SubtitleDetails> SubtitleDetails { get; set; }


        public SubtitulamosScraperData()
        {
            this.SubtitleDetails = new List<Scrapers.SubtitleDetails>();
        }

        public void SetEpisodeData(HtmlNode episode, string sourceURL)
        {
            HtmlNode EpisodeNameWrapper = episode.Descendants("div").SingleOrDefault(e => e.Id == "episode-name");
            EpisodeName = System.Web.HttpUtility.HtmlDecode(string.Concat(EpisodeNameWrapper.Element("h3").InnerText, " ", EpisodeNameWrapper.Element("div").InnerText));

            var languagesWrapper = episode.Descendants("div").SingleOrDefault(x => x.Id == "languages");
            var languages = languagesWrapper.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.StartsWith("language-container"));

            foreach (var language in languages)
            {
                IEnumerable<HtmlNode> details = language.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.StartsWith("version-container"));

                foreach (HtmlNode detail in details)
                {
                    HtmlNode versionName = detail.Descendants("p").ElementAt(1);
                    HtmlNode progressPercentage = detail.Descendants("div").SingleOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("more-info")).Element("span");
                    HtmlNode downloadUrl = detail.Descendants("a").FirstOrDefault();

                    SubtitleDetails.Add(new SubtitleDetails()
                    {
                        SubtitleLanguage = language.Element("div").InnerText,
                        VersionName = versionName.InnerText,
                        ProgressPercentage = progressPercentage.InnerText.Trim(),
                        DownloadUrl = (downloadUrl != null) ? sourceURL + downloadUrl.Attributes["href"].Value : ""
                    });
                }
            }
        }
    }
}
