using SubtitleFinderApp.Scrapers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp.Wrappers
{
    public class TabControlWrapper
    {
        private Label _lblTitle;
        private DataGridView _gridDetails;
        private int _labelOffsetY = 19;
        private int _gridViewOffsetY = 51;
        public TabControl tabCtrlResults;

        public TabControlWrapper(TabControl tabCtrlResults)
        {
            this.tabCtrlResults = tabCtrlResults;
        }

        public TabControl SetControls(List<ISourceScraper> scraper)
        {
            int selectedTabIndex = tabCtrlResults.SelectedIndex;

            foreach (var item in scraper)
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

                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(_lblTitle);
                _gridDetails.Height = _gridDetails.Rows.Count * _gridDetails.RowTemplate.Height + 30;
                tabCtrlResults.TabPages[selectedTabIndex].Controls.Add(_gridDetails);
                _labelOffsetY = _gridDetails.Location.Y + _gridDetails.Height + 14;
                _gridViewOffsetY = _labelOffsetY + _lblTitle.Height + 16;
                _gridDetails.CellContentClick += gridDetails_CellContentClick;
            }

            return tabCtrlResults;
        }

        private void gridDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DataGridView currentGridView = (DataGridView)sender;
                TabControl tab = Application.OpenForms["SubtitleFinderForm"].Controls["tabCtrResults"] as TabControl;
                Label previousTitle = (Label)tabCtrlResults.GetNextControl(currentGridView, false);

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
    }
}