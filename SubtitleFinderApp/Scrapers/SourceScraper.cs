using HtmlAgilityPack;
using SubtitleFinderApp.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp.Scrapers
{
    public abstract class SourceScraper
    {
        protected TabControl _TabCtrlResults { get; set; }
        protected Dictionary<string, string> _SeasonUrl { get; set; }

        protected abstract string _ShowsCatalogUrl { get; }
        protected abstract string _UrlPrefix { get; }
        private SearchSources _sourceName { get; set; }
        private bool _isBusy { get; set; }

        protected abstract void SetTvShows();

        public abstract string GetTvShowUrl(string text);

        public abstract TabControl GenerateResults(string tvShowUrl);

        public TabControl RenderizeSeasonTab(string lastSeasonUrl, SearchSources sourceName)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlWeb().Load(lastSeasonUrl);
            List<ISourceScraperData> scraperData = new List<ISourceScraperData>();
            IEnumerable<HtmlNode> episodes;

            switch (sourceName)
            {
                case SearchSources.TuSubtitulo:
                    episodes = htmldoc.DocumentNode.Descendants("table");

                    foreach (HtmlNode episode in episodes)
                    {
                        TuSubtituloScraperData scraperItem = new TuSubtituloScraperData();
                        scraperItem.SetEpisodeData(episode, "https:");
                        scraperData.Add(scraperItem);
                    }
                    break;
                case SearchSources.Subtitulamos:
                    episodes = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Equals("episodes")).SingleOrDefault().Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("episode"));

                    foreach (HtmlNode episode in episodes)
                    {
                        SubtitulamosScraperData scraperItem = new SubtitulamosScraperData();
                        scraperItem.SetEpisodeData(episode, "https://www.subtitulamos.tv/");
                        scraperData.Add(scraperItem);
                    }
                    break;
                default:
                    MessageBox.Show("No se pudo generar los resultados. Pruebe de nuevo.");
                    break;
            }

            _sourceName = sourceName;
            _TabCtrlResults = SetControls(scraperData, _TabCtrlResults);

            return _TabCtrlResults;
        }

        protected TabControl SetControls(List<ISourceScraperData> scraperData, TabControl _TabCtrlResults)
        {
            Label lblTitle;
            DataGridView gridDetails;
            int labelOffsetY = 19;
            int gridViewOffsetY = 51;

            int selectedTabIndex = _TabCtrlResults.SelectedIndex;

            foreach (var item in scraperData)
            {
                lblTitle = new Label()
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))),
                    Location = new Point(6, labelOffsetY),
                    Text = item.EpisodeName
                };

                gridDetails = new DataGridView()
                {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    AllowUserToOrderColumns = false,
                    BackgroundColor = SystemColors.ControlLightLight,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    Location = new Point(6, gridViewOffsetY),
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    Size = new Size(800, 120),
                    StandardTab = true
                };

                gridDetails.DefaultCellStyle.SelectionBackColor = gridDetails.DefaultCellStyle.BackColor;
                gridDetails.DefaultCellStyle.SelectionForeColor = gridDetails.DefaultCellStyle.ForeColor;

                DataGridViewTextBoxColumn Language = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Idioma",
                    Name = "Language",
                    ReadOnly = true,
                    Width = 190,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewTextBoxColumn Version = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Versión",
                    Name = "Version",
                    ReadOnly = true,
                    Width = 250,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewTextBoxColumn Progress = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Progreso",
                    Name = "Progress",
                    ReadOnly = true,
                    Width = 80,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewLinkColumn DownloadLink = new DataGridViewLinkColumn()
                {
                    HeaderText = "Descargar",
                    Name = "DownloadLink",
                    ReadOnly = true,
                    LinkBehavior = LinkBehavior.HoverUnderline,
                    Width = 110,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                gridDetails.Columns.AddRange(new DataGridViewColumn[] { Language, Version, Progress, DownloadLink });

                foreach (SubtitleDetails detail in item.SubtitleDetails)
                {
                    gridDetails.Rows.Add(detail.SubtitleLanguage, detail.VersionName, detail.ProgressPercentage, detail.DownloadUrl);
                }

                _TabCtrlResults.TabPages[selectedTabIndex].Controls.Add(lblTitle);
                gridDetails.Height = gridDetails.Rows.Count * gridDetails.RowTemplate.Height + 30;
                _TabCtrlResults.TabPages[selectedTabIndex].Controls.Add(gridDetails);
                labelOffsetY = gridDetails.Location.Y + gridDetails.Height + 14;
                gridViewOffsetY = labelOffsetY + lblTitle.Height + 16;
                gridDetails.CellContentClick += gridDetails_CellContentClick;
            }

            return _TabCtrlResults;
        }

        protected TabControl NewTabControl()
        {
            Rectangle mainFormCR = Application.OpenForms["SubtitleFinderForm"].ClientRectangle;

            return new TabControl()
            {
                Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                Appearance = TabAppearance.Normal,
                Location = new Point(12, 114),
                Name = "tabCtrlResults",
                Size = new Size(mainFormCR.Width - 24, mainFormCR.Height - 120),
                MinimumSize = new Size(860, 442),
                TabIndex = 7
            };
        }

        protected TabPage NewTabPage(string seasonTitle)
        {
            return new TabPage()
            {
                Location = new Point(4, 25),
                Size = new Size(852, 352),
                Text = seasonTitle,
                UseVisualStyleBackColor = true,
                AutoScroll = true
            };
        }

        protected void gridDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DataGridView currentGridView = (DataGridView)sender;
                Label previousTitle = (Label)_TabCtrlResults.GetNextControl(currentGridView, false);
                var pattern = new System.Text.RegularExpressions.Regex("[\\/:*?\"<>|]");

                SaveFileDialog dialogSaveSubtitle = new SaveFileDialog();
                dialogSaveSubtitle.FileName = string.Join("_", pattern.Replace(previousTitle.Text, "_"), currentGridView.CurrentRow.Cells[1].Value.ToString().Replace('/', '_'), currentGridView.CurrentRow.Cells[0].Value.ToString());
                dialogSaveSubtitle.DefaultExt = "srt";
                dialogSaveSubtitle.Filter = "Archivos SRT (*.srt)|*.srt|Todos los archivos (*.*)|*.*";
                dialogSaveSubtitle.InitialDirectory = "%USERPROFILE%\\Downloads";
                dialogSaveSubtitle.RestoreDirectory = true;
                dialogSaveSubtitle.Title = "Guardar subtítulo como...";

                if (_isBusy) return;

                if (dialogSaveSubtitle.ShowDialog() == DialogResult.OK)
                {
                    _isBusy = true;
                    var wClient = new System.Net.WebClient();

                    if(_sourceName == SearchSources.TuSubtitulo)
                        wClient.Headers.Add("referer", "https://www.tusubtitulo.com/show/2199");

                    wClient.DownloadFileCompleted += (webClientSender, args) =>
                    {
                        MessageBox.Show($"\"{dialogSaveSubtitle.FileName}\" se ha descargado correctamente.");
                        _isBusy = false;
                    };

                    wClient.DownloadFileAsync(new Uri(currentGridView.CurrentRow.Cells[3].Value.ToString()), dialogSaveSubtitle.FileName);
                }
            }
        }

        protected abstract void _TabCtrlResults_SelectedIndexChanged(object sender, EventArgs e);
    }
}