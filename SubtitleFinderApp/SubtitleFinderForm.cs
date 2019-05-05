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
        public SubtitleFinderForm()
        {
            InitializeComponent();
            rdoBtnSubDivX.Click += sourceRadioBtns_Click;
            rdoBtnTuSubtitulo.Click += sourceRadioBtns_Click;
            rdoBtnSubtitulamos.Click += sourceRadioBtns_Click;

            tltDownloadFolder.SetToolTip(this.btnDownloadFolder, "Abrir carpeta de descargas");
            tltProductInfo.SetToolTip(this.btnProductInfo, "Información del producto");
        }

        private void SubtitleFinderForm_Load(object sender, EventArgs e)
        {
            
        }

        private void sourceRadioBtns_Click(object sender, EventArgs e)
        {
            RadioButton currentRadioButton = sender as RadioButton;

            if (currentRadioButton != null)
            {
                if(currentRadioButton.Name == "rdoBtnSubDivX")
                {
                    lblSearch.Text = "Escriba el nombre de la serie/película (+palabras claves):";
                    lblSearchExample.Text = "Ejemplos: \"the walking dead s09e01\", \"batman begins 2005\"";
                }
                else
                {
                    lblSearch.Text = "Escriba el nombre de la serie:";
                    lblSearchExample.Text = "Ejemplos: \"the walking dead\", \"gotham\"";
                }
            }
        }

        private void SearchWithSubDivX(string text)
        {
            if(text.Count() > 3)
            {
                SubDivXScraper scraper = new SubDivXScraper();
                IEnumerable<HtmlNode> episodes = scraper.GetEpisodeNodes(text);

                if (episodes.Any())
                {
                    ClearResultsArea();
                    this.Controls.Add(scraper.GenerateResults(episodes));
                }
                else
                    NoResultsMessageBox();
            }
            else
            {
                MessageBox.Show("Debes ingresar más de 3 letras: puedes agregar palabras claves (" + lblSearchExample.Text + ")");
            }
        }

        private void SearchWithTuSubtitulo(string text)
        {
            TuSubtituloScraper scraper = new TuSubtituloScraper();
            string tvShowURL = scraper.GetTvShowUrl(text);

            if (!string.IsNullOrEmpty(tvShowURL))
            {
                ClearResultsArea();
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
                ClearResultsArea();
                this.Controls.Add(scraper.GenerateResults(tvShowURL));
            }
            else
                NoResultsMessageBox();
        }

        private void NoResultsMessageBox()
        {
            MessageBox.Show("No hay resultados que mostrar.");
        }

        private void ClearResultsArea()
        {
            if (this.Controls.ContainsKey("gridResults"))
            {
                this.Controls.RemoveByKey("gridResults");
                this.Controls.RemoveByKey("paginationContainer");
            }   

            if (this.Controls.ContainsKey("tabCtrlResults"))
                this.Controls.RemoveByKey("tabCtrlResults");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RadioButton checkedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

            if (checkedButton != null)
            {
                switch (checkedButton.Name)
                {
                    case "rdoBtnSubDivX":
                        SearchWithSubDivX(txtSearch.Text);
                        break;
                    case "rdoBtnTuSubtitulo":
                        SearchWithTuSubtitulo(txtSearch.Text);
                        break;
                    case "rdoBtnSubtitulamos":
                        SearchWithSubtitulamos(txtSearch.Text);
                        break;
                }

                this.Controls.Remove(picBoxAppImage);
            }
            else
                MessageBox.Show("No seleccionó ninguna fuente de subtítulos.");
        }

        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            string appInfoText = string.Join(
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

        private void btnDownloadFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads");
        }
    }
}