using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public class SubtitulamosScraper
    {
        public string EpisodeName { get; set; }
        public List<SubtitleDetails> SubtitleDetails { get; set; }


        public SubtitulamosScraper()
        {
            this.SubtitleDetails = new List<Scrapers.SubtitleDetails>();
        }
    }

    public class SubtitleDetails
    {
        public string SubtitleLanguage { get; set; }
        public string VersionName { get; set; }
        public string ProgressPercentage { get; set; }
        public string DownloadUrl { get; set; }
    }
}
