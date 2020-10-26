using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public class TuSubtituloScraperData : ISourceScraperData
    {
        public string EpisodeName { get; set; }
        public List<SubtitleDetails> SubtitleDetails { get; set; }


        public TuSubtituloScraperData()
        {
            this.SubtitleDetails = new List<Scrapers.SubtitleDetails>();
        }

        public void SetEpisodeData(HtmlNode episode, string prefix)
        {
            IEnumerable<HtmlNode> allRows = episode.Descendants("tr");
            HtmlNode episodeHeader = allRows.First().Descendants("td").Where(c => c.Attributes.Contains("class") && c.Attributes["class"].Value.Equals("NewsTitle")).Last().Descendants("a").First();
            EpisodeName = episodeHeader.InnerText;
            string downloadUrl = episodeHeader.Attributes["href"].Value;
            string versionName = "", subtitleLanguage = "", progressPercentage = "";

            foreach(HtmlNode row in allRows.Skip(1))
            {
                IEnumerable<HtmlNode> currentColumns = row.Descendants("td");
                if (currentColumns.Count() > 1)
                {
                    HtmlNode versionNameColumn = currentColumns.Where(c => c.Attributes.Contains("colspan") && c.Attributes["class"].Value.Equals("newsClaro")).SingleOrDefault();

                    if (versionNameColumn != null)
                    {
                        versionName = versionNameColumn.InnerText;
                        continue;
                    }

                    HtmlNode languageColumn = currentColumns.Where(c => c.Attributes.Contains("class") && c.Attributes["class"].Value.Equals("language")).SingleOrDefault();

                    if (languageColumn != null)
                    {
                        subtitleLanguage = languageColumn.InnerText;
                        progressPercentage = languageColumn.NextSibling.NextSibling.InnerText;

                        SubtitleDetails.Add(new SubtitleDetails()
                        {
                            VersionName = System.Web.HttpUtility.HtmlDecode(versionName.Trim()),
                            SubtitleLanguage = subtitleLanguage.Trim(),
                            ProgressPercentage = progressPercentage.Trim(),
                            DownloadUrl = downloadUrl
                        });
                    }
                }
            }
        }
    }
}
