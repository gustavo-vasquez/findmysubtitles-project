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
            EpisodeName = System.Web.HttpUtility.HtmlDecode(episode.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.Equals("episode-name")).SingleOrDefault().InnerText);

            foreach (var language in episode.Descendants("div").Where(e => e.Attributes.Contains("class") && e.Attributes["class"].Value.Equals("subtitle-language")))
            {
                IEnumerable<HtmlNode> details = language.NextSibling.NextSibling.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("sub"));

                foreach (HtmlNode detail in details)
                {
                    HtmlNode versionName = detail.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("version-name")).SingleOrDefault();
                    HtmlNode progressPercentage = detail.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("progress_percentage")).SingleOrDefault();
                    HtmlNode downloadUrl = detail.Descendants("a").Where(a => a.Attributes.Contains("href")).SingleOrDefault();

                    SubtitleDetails.Add(new SubtitleDetails()
                    {
                        SubtitleLanguage = language.InnerText,
                        VersionName = versionName.InnerText,
                        ProgressPercentage = progressPercentage.InnerText.Trim(),
                        DownloadUrl = (downloadUrl != null) ? sourceURL + downloadUrl.Attributes["href"].Value.Substring(1) : ""
                    });
                }
            }

            //return this;
        }
    }
}
