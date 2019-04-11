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
    public interface ISourceScraper
    {
        string GetTvShowUrl(string text);
        TabControl GenerateResults(string tvShowUrl);
        TabControl RenderizeSeasonTab(string lastSeasonUrl, SearchSources sourceName);
    }
}
