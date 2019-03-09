using HtmlAgilityPack;
using SubtitleFinderApp.Scrapers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SubtitleFinderApp
{
    public partial class SubtitleFinderForm : Form
    {
        private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.Default };
        private SubDivXScraper subdivx = new SubDivXScraper();

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

        private void DoSearch(string text)
        {
            var htmldoc = _web.Load("https://www.subdivx.com/index.php?buscar=" + HttpUtility.UrlEncode(text) + "&accion=5&masdesc=&subtitulos=1&realiza_b=1");
            subdivx.Title = htmldoc.DocumentNode.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("titulo_menu_izq")).FirstOrDefault();
            subdivx.Description = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub")).FirstOrDefault();
            subdivx.Details = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub_datos")).FirstOrDefault();
            subdivx.Comments = subdivx.Details.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).FirstOrDefault();
            subdivx.DownloadLink = subdivx.Details.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).LastOrDefault();

            lblTitle.Text = subdivx.Title.InnerText;
            lblDescription.Text = subdivx.Description.InnerText;
            lblDetails.Text = subdivx.Details.InnerText;
            lblComments.Text = "Comentarios (" + subdivx.Comments.InnerText + ")";
        }

        private void DoSearchAll(string text)
        {
            var htmldoc = _web.Load("https://www.subdivx.com/index.php?buscar=" + HttpUtility.UrlEncode(text) + "&accion=5&masdesc=&subtitulos=1&realiza_b=1");
            var anchors = htmldoc.DocumentNode.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("titulo_menu_izq"));
            var descriptions = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub"));
            var details = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub_datos"));

            foreach (var a in anchors)
            {
                lblTitle.Text += a.InnerText;
            }

            foreach (var d in descriptions)
            {
                lblDescription.Text += d.InnerText;
            }

            foreach (var det in details)
            {
                lblDetails.Text += det.InnerText;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch(txtSearch.Text);
        }

        private void lblComments_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.subdivx.com/" + (e.Link.LinkData as string));
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            dialogSaveSubtitle.FileName = lblTitle.Text;
            var result = dialogSaveSubtitle.ShowDialog();
            if (result == DialogResult.OK)
            {                
                var wClient = new WebClient();
                wClient.DownloadFile(subdivx.DownloadLink.Attributes["href"].Value, dialogSaveSubtitle.FileName);
            }
        }        
    }
}
