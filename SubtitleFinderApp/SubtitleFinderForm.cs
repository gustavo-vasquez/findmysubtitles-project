using HtmlAgilityPack;
using SubtitleFinderApp.Scrapers;
using SubtitleFinderApp.Enums;
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
        private DataGridView gridResults;

        //private HtmlWeb _web = new HtmlWeb() { OverrideEncoding = Encoding.GetEncoding("ISO-8859-1"), AutoDetectEncoding = false };

        public SubtitleFinderForm()
        {
            InitializeComponent();
        }

        private void SubtitleFinderForm_Load(object sender, EventArgs e)
        {
            this.rdoBtnSubDivX.PerformClick();            
        }

        private void DoSearch(string text)
        {
            RadioButton checkedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

            if(checkedButton != null)
            {
                switch (checkedButton.Name)
                {
                    case "rdoBtnSubDivX":
                        SearchWithSubDivX(text);
                        break;
                    case "rdoBtnTuSubtitulo":
                        SearchWithTuSubtitulo(text);
                        break;
                    case "rdoBtnSubtitulamos":
                        SearchWithSubtitulamos(text);
                        break;
                }
            }
            else
                MessageBox.Show("No seleccionó ninguna fuente de subtítulos.");
        }

        private void SearchWithSubDivX(string text)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = _web.Load("https://www.subdivx.com/index.php?buscar=" + HttpUtility.UrlEncode(text) + "&accion=5&masdesc=&subtitulos=1&realiza_b=1");
            HtmlNode wrapper = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("contenedor_izq")).SingleOrDefault();
            IEnumerable<HtmlNode> episodes = wrapper.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("menu_detalle_buscador"));

            if (episodes.Any())
            {
                if (this.Controls.ContainsKey("tabCtrlResults"))
                    this.Controls.RemoveByKey("tabCtrlResults");

                if (!this.Controls.ContainsKey("gridResults"))
                    this.Controls.Add(gridResults);
                else
                    gridResults.Rows.Clear();

                SubDivXScraper scraper = new SubDivXScraper();
                scraper.FillResults(ref gridResults, episodes);
            }
            else
                NoResultsMessageBox();
        }

        private void SearchWithTuSubtitulo(string text)
        {
            TuSubtituloScraper scraper = new TuSubtituloScraper();
            string tvShowURL = scraper.GetTvShowUrl(text);

            if (!string.IsNullOrEmpty(tvShowURL))
            {
                if (this.Controls.ContainsKey("gridResults"))
                {
                    this.Controls.Remove(gridResults);
                    gridResults.Rows.Clear();
                }

                if (this.Controls.ContainsKey("tabCtrlResults"))
                    this.Controls.RemoveByKey("tabCtrlResults");

                this.Controls.Add(scraper.GenerateResults(tvShowURL));
            }
            else
                NoResultsMessageBox();
        }

        private void SearchWithSubtitulamos(string text)
        {
            SubtitulamosScraper scraper = new SubtitulamosScraper();
            string tvShowURL = scraper.GetTvShowUrl(text);

            if (!string.IsNullOrEmpty(tvShowURL))
            {
                if (this.Controls.ContainsKey("gridResults"))
                {
                    this.Controls.Remove(gridResults);
                    gridResults.Rows.Clear();
                }

                if (this.Controls.ContainsKey("tabCtrlResults"))
                    this.Controls.RemoveByKey("tabCtrlResults");

                this.Controls.Add(scraper.GenerateResults(tvShowURL));
            }
            else
                NoResultsMessageBox();
        }

        private void NoResultsMessageBox()
        {
            MessageBox.Show("No hay resultados que mostrar.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch(txtSearch.Text);
            this.Controls.Remove(picBoxAppImage);
        }

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

        private void gridResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView currentGridView = (DataGridView)sender;
            DataGridViewRow currentRow = currentGridView.Rows[e.RowIndex];

            if (e.ColumnIndex == 3)
            {
                string commentUrl = currentRow.Cells["CommentsUrl"].Value.ToString();
                string queryString = commentUrl.Substring(commentUrl.IndexOf('?'));
                string subtitleId = HttpUtility.ParseQueryString(queryString).Get("idsub");

                var htmlComments = new HtmlWeb() { OverrideEncoding = Encoding.Default }.Load("https://www.subdivx.com/popcoment.php?idsub=" + HttpUtility.UrlEncode(subtitleId));
                var userComments = htmlComments.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("pop_upcoment"));
                new SubDivXCommentsDialog().ShowDialog(userComments);
            }

            if (e.ColumnIndex == 4)
            {
                string downloadUrl = currentRow.Cells["DownloadLink"].Value.ToString();
                string queryString = downloadUrl.Substring(downloadUrl.IndexOf('?'));
                string subtitleId = HttpUtility.ParseQueryString(queryString).Get("id");

                dialogSaveSubtitle.FileName = string.Join(" ", currentRow.Cells["Title"].Value.ToString(), "-", subtitleId);
                dialogSaveSubtitle.DefaultExt = "rar";
                dialogSaveSubtitle.Filter = "Archivos RAR (*.rar)|*.rar|Todos los archivos (*.*)|*.*";

                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WebClient wClient = new WebClient();
                    wClient.DownloadFile(downloadUrl, dialogSaveSubtitle.FileName);
                }
            }
        }

        private void picBoxSubDivX_Click(object sender, EventArgs e)
        {
            rdoBtnSubDivX.Checked = true;
        }

        private void picBoxTuSubtitulo_Click(object sender, EventArgs e)
        {
            rdoBtnTuSubtitulo.Checked = true;
        }

        private void picBoxSubtitulamos_Click(object sender, EventArgs e)
        {
            rdoBtnSubtitulamos.Checked = true;
        }

        private void radioBtnSources_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton currentRadioButton = sender as RadioButton;

            if(currentRadioButton != null && currentRadioButton.Checked)
            {
                switch(currentRadioButton.Name)
                {
                    case "rdoBtnSubDivX":
                        if(gridResults == null)
                        {
                            SubDivXScraper.InitGridViewControl(ref gridResults);
                            gridResults.CellContentClick += this.gridResults_CellContentClick;
                        }
                        break;
                    case "rdoBtnTuSubtitulo":
                        //if (tabCtrlResults == null)
                        //{
                        //    tabCtrlResults = new TabControl()
                        //    {
                        //        Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                        //        Appearance = TabAppearance.Normal,
                        //        Location = new Point(12, 114),
                        //        Name = "tabCtrlResults",
                        //        Size = new Size(860, 442)
                        //    };

                        //    tabCtrlResults.Click += tabCtrlResults_Click;
                        //}
                        break;
                    case "rdoBtnSubtitulamos":
                        //if(tabCtrlResults == null)
                        //{
                        //    tabCtrlResults = new TabControl()
                        //    {
                        //        Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                        //        Appearance = TabAppearance.Normal,
                        //        Location = new Point(12, 114),
                        //        Name = "tabCtrlResults",
                        //        Size = new Size(860, 442)
                        //    };

                        //    tabCtrlResults.Click += tabCtrlResults_Click;
                        //}
                        break;
                }
            }
        }
    }
}