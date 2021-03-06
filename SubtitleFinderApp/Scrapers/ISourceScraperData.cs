﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.Scrapers
{
    public interface ISourceScraperData
    {
        string EpisodeName { get; set; }
        List<SubtitleDetails> SubtitleDetails { get; set; }
        void SetEpisodeData(HtmlNode episode, string patchLink);
    }
}
