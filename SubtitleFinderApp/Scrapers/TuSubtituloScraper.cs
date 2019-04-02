using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public class TuSubtituloScraper
    {
        public string EpisodeName { get; set; }
        public List<SubtitleDetails> SubtitleDetails { get; set; }


        public TuSubtituloScraper()
        {
            this.SubtitleDetails = new List<Scrapers.SubtitleDetails>();
        }

        public TuSubtituloScraper GetEpisodeInformation(HtmlNode episode, string prefix)
        {
            EpisodeName = episode.Descendants("tr").FirstOrDefault().Descendants("td").Where(c => c.Attributes.Contains("class") && c.Attributes["class"].Value.Equals("NewsTitle")).LastOrDefault().Descendants("a").FirstOrDefault().InnerText;
            string versionName = "", subtitleLanguage = "", progressPercentage = "", downloadUrl = "";

            foreach(var row in episode.Descendants("tr"))
            {
                bool isTitleRow = row.Descendants("td").Any(c => c.Attributes.Contains("class") && c.Attributes["class"].Value.Equals("NewsTitle"));

                if(!isTitleRow && (row.Descendants("td").Count() > 1))
                {
                    HtmlNode versionNameColumn = row.Descendants("td").Where(c => c.Attributes.Contains("colspan") && c.Attributes["class"].Value.Equals("newsClaro")).SingleOrDefault();

                    if (versionNameColumn != null)
                    {
                        versionName = versionNameColumn.InnerText;
                        continue;
                    }

                    HtmlNode languageColumn = row.Descendants("td").Where(c => c.Attributes.Contains("class") && c.Attributes["class"].Value.Equals("language")).SingleOrDefault();

                    if (languageColumn != null)
                    {
                        subtitleLanguage = languageColumn.InnerText;
                        progressPercentage = languageColumn.NextSibling.NextSibling.InnerText;

                        HtmlNode downloadUrlNode = languageColumn.NextSibling.NextSibling.NextSibling.NextSibling.Descendants("a").SingleOrDefault();
                        downloadUrl = (downloadUrlNode != null) ? prefix + downloadUrlNode.Attributes["href"].Value : "";

                        SubtitleDetails.Add(new SubtitleDetails()
                        {
                            VersionName = versionName,
                            SubtitleLanguage = subtitleLanguage,
                            ProgressPercentage = progressPercentage,
                            DownloadUrl = downloadUrl
                        });
                    }
                }
            }

            return this;
        }
    }
}
