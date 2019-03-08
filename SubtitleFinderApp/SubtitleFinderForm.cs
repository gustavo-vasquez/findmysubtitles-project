using HtmlAgilityPack;
using SubtitleFinderApp.WebScrapers;
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
            var title = htmldoc.DocumentNode.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("titulo_menu_izq")).FirstOrDefault();
            var description = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub")).FirstOrDefault();
            var detail = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub_datos")).FirstOrDefault();
            
            lblTitle.Text = title.InnerText;
            lblDescription.Text = description.InnerText;
            lblDetails.Text = detail.InnerText;
            lblComments.Text = "Comentarios (" + detail.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).FirstOrDefault().InnerText + ")";
            lblComments.Links.Add(new LinkLabel.Link() { LinkData = detail.Descendants("a").Where(a => a.Attributes.Contains("href")).FirstOrDefault().Attributes["href"].Value });
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
            saveFileDialog1.FileName = lblTitle.Text;            
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {                
                var wClient = new WebClient();
                wClient.DownloadFile("http://www.subdivx.com/bajar.php?id=523910&u=8", saveFileDialog1.FileName);
            }
        }
    }
}
