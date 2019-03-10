using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public class SubDivXScraperSingle
    {
        public HtmlNode Title { get; set; }
        public HtmlNode Description { get; set; }
        public HtmlNode Details { get; set; }
        public HtmlNode Comments { get; set; }
        public HtmlNode DownloadLink { get; set; }
    }
}
