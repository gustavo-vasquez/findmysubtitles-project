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

            _TabCtrlResults = SetControls(scraperData, _TabCtrlResults);

            return _TabCtrlResults;
        }

        protected TabControl SetControls(List<ISourceScraperData> scraperData, TabControl _TabCtrlResults)
        {
            Label _lblTitle;
            DataGridView _gridDetails;
            int _labelOffsetY = 19;
            int _gridViewOffsetY = 51;

            int selectedTabIndex = _TabCtrlResults.SelectedIndex;

            foreach (var item in scraperData)
            {
                _lblTitle = new Label()
                {
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))),
                    Location = new Point(6, _labelOffsetY),
                    Text = item.EpisodeName
                };

                _gridDetails = new DataGridView()
                {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    AllowUserToOrderColumns = false,
                    BackgroundColor = SystemColors.ControlLightLight,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    Location = new Point(6, _gridViewOffsetY),
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    Size = new Size(800, 120),
                    StandardTab = true
                };

                _gridDetails.DefaultCellStyle.SelectionBackColor = _gridDetails.DefaultCellStyle.BackColor;
                _gridDetails.DefaultCellStyle.SelectionForeColor = _gridDetails.DefaultCellStyle.ForeColor;

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
                    Width = 60,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataGridViewLinkColumn DownloadLink = new DataGridViewLinkColumn()
                {
                    HeaderText = "Descarga",
                    Name = "DownloadLink",
                    ReadOnly = true,
                    Width = 110,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                _gridDetails.Columns.AddRange(new DataGridViewColumn[] { Language, Version, Progress, DownloadLink });

                foreach (SubtitleDetails detail in item.SubtitleDetails)
                {
                    _gridDetails.Rows.Add(detail.SubtitleLanguage, detail.VersionName, detail.ProgressPercentage, detail.DownloadUrl);
                }

                _TabCtrlResults.TabPages[selectedTabIndex].Controls.Add(_lblTitle);
                _gridDetails.Height = _gridDetails.Rows.Count * _gridDetails.RowTemplate.Height + 30;
                _TabCtrlResults.TabPages[selectedTabIndex].Controls.Add(_gridDetails);
                _labelOffsetY = _gridDetails.Location.Y + _gridDetails.Height + 14;
                _gridViewOffsetY = _labelOffsetY + _lblTitle.Height + 16;
                _gridDetails.CellContentClick += gridDetails_CellContentClick;
            }

            return _TabCtrlResults;
        }

        protected TabControl NewTabControl()
        {
            return new TabControl()
            {
                Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                Appearance = TabAppearance.Normal,
                Location = new Point(12, 114),
                Name = "tabCtrlResults",
                Size = new Size(860, 442),
                TabIndex = 6
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
                //TabControl tab = Application.OpenForms["SubtitleFinderForm"].Controls["tabCtrResults"] as TabControl;
                Label previousTitle = (Label)_TabCtrlResults.GetNextControl(currentGridView, false);

                SaveFileDialog dialogSaveSubtitle = new SaveFileDialog();
                dialogSaveSubtitle.FileName = string.Join("_", previousTitle.Text, currentGridView.CurrentRow.Cells[1].Value.ToString().Replace('/', '_'), currentGridView.CurrentRow.Cells[0].Value.ToString());
                dialogSaveSubtitle.DefaultExt = "srt";
                dialogSaveSubtitle.Filter = "Archivos SRT (*.srt)|*.srt|Todos los archivos (*.*)|*.*";
                dialogSaveSubtitle.InitialDirectory = "%USERPROFILE%\\Downloads";
                dialogSaveSubtitle.RestoreDirectory = true;
                dialogSaveSubtitle.Title = "Guardar subtítulo como...";

                var result = dialogSaveSubtitle.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var wClient = new System.Net.WebClient();
                    wClient.DownloadFile(currentGridView.CurrentRow.Cells[3].Value.ToString(), dialogSaveSubtitle.FileName);
                }
            }
        }

        protected abstract void _TabCtrlResults_Click(object sender, EventArgs e);
    }
}
