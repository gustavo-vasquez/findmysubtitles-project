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
            //this.MouseWheel += flowResultsPanel_MouseWheel;
            this.comboSources.SelectedItem = this.comboSources.Items[0];
            gridResults.Columns["Title"].Width = 200;
            gridResults.Columns["Description"].Width = 450;
            DataGridViewLinkColumn commentsColumn = new DataGridViewLinkColumn();
            commentsColumn.HeaderText = "Comentarios";
            commentsColumn.LinkBehavior = LinkBehavior.SystemDefault;
            commentsColumn.Name = "Comments";
            commentsColumn.Text = "Comentarios";
            gridResults.Columns.Add(commentsColumn);            
            gridResults.Columns["Comments"].Width = 80;
            DataGridViewLinkColumn downloadsColumn = new DataGridViewLinkColumn();
            downloadsColumn.HeaderText = "";
            downloadsColumn.LinkBehavior = LinkBehavior.SystemDefault;
            downloadsColumn.Name = "DownloadLink";
            downloadsColumn.Text = "Descargar";
            downloadsColumn.UseColumnTextForLinkValue = true;
            gridResults.Columns.Add(downloadsColumn);
            gridResults.Columns["DownloadLink"].Width = 80;            
        }

        private void SubtitleFinderForm_Load(object sender, EventArgs e)
        {
            
        }

        private void DoSearch(string text)
        {
            gridResults.Rows.Clear();
            var htmldoc = _web.Load("https://www.subdivx.com/index.php?buscar=" + HttpUtility.UrlEncode(text) + "&accion=5&masdesc=&subtitulos=1&realiza_b=1");
            subdivx.Title = htmldoc.DocumentNode.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("titulo_menu_izq")).ToList();
            subdivx.Description = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub")).ToList();
            subdivx.Details = htmldoc.DocumentNode.Descendants("div").Where(a => a.Attributes.Contains("id") && a.Attributes["id"].Value.Equals("buscador_detalle_sub_datos")).ToList();

            subdivx.Comments = new List<HtmlNode>();
            subdivx.DownloadLink = new List<HtmlNode>();

            foreach (HtmlNode detail in subdivx.Details)
            {
                subdivx.Comments.Add(detail.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).FirstOrDefault());
                subdivx.DownloadLink.Add(detail.Descendants("a").Where(a => a.Attributes.Contains("rel") && a.Attributes["rel"].Value.Equals("nofollow")).LastOrDefault());
            }

            //for (var i = 0; i < titles.Count; i++)
            //{
            //    subdivxs.Add(new SubDivXScraper()
            //    {
            //        Title = titles[i],
            //        Description = descriptions[i],
            //        Details = details[i],
            //        Comments = comments[i],
            //        DownloadLink = downloadLink[i]
            //    });
            //}

            foreach (var title in subdivx.Title)
            {
                gridResults.Rows.Add(title.InnerText.Substring(13));
            }

            for (var i = 0; i < subdivx.Description.Count; i++)
            {
                gridResults.Rows[i].Cells["Description"].Value = subdivx.Description[i].InnerText;
            }

            for (var i = 0; i < subdivx.Comments.Count; i++)
            {
                gridResults.Rows[i].Cells["Comments"].Value = subdivx.Comments[i].InnerText;
            }

            //for (var i = 0; i < subdivx.DownloadLink.Count; i++)
            //{
            //    gridResults.Rows[i].Cells["DownloadLink"].Value = subdivx.DownloadLink[i].Attributes["href"].Value;
            //}

            //gridResults.Height = this.gridResults.Rows. * gridResults.Rows.Count;
            gridResults.Refresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch(txtSearch.Text);
        }        

        //private void flowResultsPanel_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    if(flowResultsPanel.VerticalScroll.Visible || flowResultsPanel.HorizontalScroll.Visible)
        //        if(e.X > 12 && e.X < 709 && e.Y > 83 && e.Y < 289)
        //            flowResultsPanel.Select();            
        //}

        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            string appInfoText = String.Join(
                    null,
                    ProductInfo.Product,
                    Environment.NewLine,
                    ProductInfo.Description,
                    Environment.NewLine,
                    Environment.NewLine,
                    ProductInfo.Copyright,
                    Environment.NewLine,
                    Environment.NewLine,
                    "Versión: ",
                    System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
                );
            DialogResult AppInfoWindow = MessageBox.Show(appInfoText, "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gridResults_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
                return;

            statusbarLabel.Text = subdivx.Details[e.Row.Index].InnerText;
        }        

        private void gridResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string commentUrl = subdivx.Comments[e.RowIndex].Attributes["href"].Value;
            string queryString = commentUrl.Substring(commentUrl.IndexOf('?'));
            string subtitleId = HttpUtility.ParseQueryString(queryString).Get("idsub");

            if (e.ColumnIndex == 2)
            {                
                var htmlComments = new HtmlWeb() { OverrideEncoding = Encoding.Default }.Load("https://www.subdivx.com/popcoment.php?idsub=" + HttpUtility.UrlEncode(subtitleId));
                var userComments = htmlComments.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("pop_upcoment"));                
                new SubDivXCommentsDialog().ShowDialog("traducidos por argenteam para gotham s05e05 pena dura 720p web x264-tbs, también sirve para 1080p web x264-tbs y web x264-tbs los créditos a ellos! enjoy!", "Ver comentarios", userComments);
            }

            if (e.ColumnIndex == 3)
            {
                dialogSaveSubtitle.FileName = string.Join(" ", subtitleId, "-", subdivx.Title[e.RowIndex].InnerText);
                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var wClient = new WebClient();
                    wClient.DownloadFile(subdivx.DownloadLink[e.RowIndex].Attributes["href"].Value, dialogSaveSubtitle.FileName);
                }
            }
        }        
    }
}
