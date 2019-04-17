using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleFinderApp
{
    public class SubDivXCommentsDialog
    {
        private Form _commentsForm;
        private TableLayoutPanel _tablePanel;
        private Label _lblComment, _lblUser;
        private Button _btnClose;

        public SubDivXCommentsDialog()
        {
            _commentsForm = new Form()
            {
                Width = 500,
                Height = 500,
                Text = "Ver comentarios",
                StartPosition = FormStartPosition.CenterParent,
                Icon = Properties.Resources.comments_form,
                MinimumSize = new System.Drawing.Size(500, 500)
            };

            _tablePanel = new TableLayoutPanel()
            {
                Location = new System.Drawing.Point(32, 20),
                ColumnCount = 1,
                AutoScroll = true,
                Size = new System.Drawing.Size(420, 390),
                TabIndex = 1,
                Anchor = ((System.Windows.Forms.AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)))
            };

            _tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tablePanel.MouseHover += TablePanel_MouseHover;

            _btnClose = new Button()
            {
                Text = "Cerrar",
                Width = 100,
                Location = new System.Drawing.Point(190, 420),
                TabIndex = 0,
                Anchor = ((System.Windows.Forms.AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right)))
            };

            _btnClose.Click += (sender, e) => { _commentsForm.Close(); };
            _commentsForm.Controls.Add(_btnClose);
            _commentsForm.CancelButton = _btnClose;            
        }

        public void ShowDialog(IEnumerable<HtmlNode> userCommentsDivs)
        {
            int rowIndex = 0;

            foreach (var div in userCommentsDivs)
            {
                _lblUser = new Label()
                {
                    Text = div.Descendants("div").SingleOrDefault().Descendants("a").SingleOrDefault().InnerText,
                    Margin = new Padding(3, 0, 3, 0),                    
                    AutoSize = true,
                    Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold)
                };

                _tablePanel.Controls.Add(_lblUser, 0, rowIndex);
                _tablePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));                

                _lblComment = new Label()
                {                    
                    Text = div.FirstChild.InnerText.Trim(),                                     
                    Margin = new Padding(3, 0, 3, 0),                    
                    AutoSize = true
                };
                
                _tablePanel.Controls.Add(_lblComment, 0, rowIndex + 1);
                _tablePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                _lblComment.Width = _tablePanel.Width - 20;                
                _tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));

                _lblUser.MouseHover += TablePanel_MouseHover;
                _lblComment.MouseHover += TablePanel_MouseHover;
                rowIndex += 3;
            }

            _commentsForm.Controls.Add(_tablePanel);
            _commentsForm.ShowDialog();
        }

        private void TablePanel_MouseHover(object sender, EventArgs e)
        {
            if (!_tablePanel.Focused && _tablePanel.VerticalScroll.Visible)
                _tablePanel.Focus();
        }
    }
}