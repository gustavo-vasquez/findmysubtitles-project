using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public class SubDivXScraper
    {
        public List<HtmlNode> Title { get; set; }
        public List<HtmlNode> Description { get; set; }
        public List<HtmlNode> Details { get; set; }
        public List<HtmlNode> Comments { get; set; }        
        public List<HtmlNode> DownloadLink { get; set; }        
    }
}
