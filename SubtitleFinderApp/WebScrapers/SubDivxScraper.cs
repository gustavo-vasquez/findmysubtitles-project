using IronWebScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFinderApp.WebScrapers
{
    public class SubDivxScraper : WebScraper
    {
        //public List<string> titles;

        public override void Init()
        {
            License.LicenseKey = "IRONWEBSCRAPER-143290-839020-5117AB04-6F50881A-F3BC5BD8-VALID-2017"; // Write License Key
            this.LoggingLevel = WebScraper.LogLevel.All; // All Events Are Logged                        
            this.WorkingDirectory = @"C:\Users\Gustavo\Documents\Visual Studio 2015\Projects\SubtitleFinderApp\SubtitleFinderApp\bin\Debug\HelloScraperSample\Output\";
            this.Request("https://www.subdivx.com/index.php?buscar=jigsaw+2017&accion=5&masdesc=&subtitulos=1&realiza_b=1", Parse);
        }

        public override void Parse(Response response)
        {
            foreach (var title_link in response.Css("a"))
            {
                // Read Link Text
                var title = title_link.TextContentClean;
                var link = title_link.Attributes["href"];
                Scrape(new ScrapedData() { { "MovieId", title }, { "MovieTitle", link } }, "subdivx.Jsonl");
            }
        }
    }
}
