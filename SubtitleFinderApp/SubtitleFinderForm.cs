using HtmlAgilityPack;
using SubtitleFinderApp.WebScrapers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp
{
    public partial class SubtitleFinderForm : Form
    {
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.Default };

        public SubtitleFinderForm()
        {
            InitializeComponent();            
        }

        private void SubtitleFinderForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var htmldoc = _web.Load("https://www.subdivx.com/index.php?buscar=jigsaw+2017&accion=5&masdesc=&subtitulos=1&realiza_b=1");
            var anchors = htmldoc.DocumentNode.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("titulo_menu_izq"));
            var descriptions = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub"));

            foreach (var a in anchors)
            {
                textBox2.Text += a.InnerText;
            }

            foreach(var d in descriptions)
            {
                textBox1.Text += d.InnerText;
            }
        }
    }
}
