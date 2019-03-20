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
        private Form commentsForm;
        private TableLayoutPanel tablePanel;
        private Label lblComment, lblUser;
        private Button btnClose;

        public SubDivXCommentsDialog()
        {
            commentsForm = new Form()
            {
                Width = 500,
                Height = 500,
                Text = "Ver comentarios",
                StartPosition = FormStartPosition.CenterParent,
                Icon = Properties.Resources.comments_form,
                MinimumSize = new System.Drawing.Size(500, 500)
            };

            tablePanel = new TableLayoutPanel()
            {
                Location = new System.Drawing.Point(32, 20),
                ColumnCount = 1,
                AutoScroll = true,
                Size = new System.Drawing.Size(420, 390),
                TabIndex = 1,
                Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)))
            };

            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tablePanel.MouseHover += TablePanel_MouseHover;

            btnClose = new Button()
            {
                Text = "Cerrar",
                Width = 100,
                Location = new System.Drawing.Point(190, 420),
                TabIndex = 0,
                Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)))
            };

            btnClose.Click += (sender, e) => { commentsForm.Close(); };
            commentsForm.Controls.Add(btnClose);
            commentsForm.CancelButton = btnClose;            
        }

        public void ShowDialog(IEnumerable<HtmlNode> userCommentsDivs)
        {
            int rowIndex = 0;

            foreach (var div in userCommentsDivs)
            {
                lblUser = new Label()
                {
                    Text = div.Descendants("div").SingleOrDefault().Descendants("a").SingleOrDefault().InnerText,
                    Margin = new System.Windows.Forms.Padding(3, 0, 3, 0),                    
                    AutoSize = true,
                    Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold)
                };

                tablePanel.Controls.Add(lblUser, 0, rowIndex);
                tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));                

                lblComment = new Label()
                {                    
                    Text = div.FirstChild.InnerText.Trim(),                                     
                    Margin = new System.Windows.Forms.Padding(3, 0, 3, 0),                    
                    AutoSize = true
                };
                
                tablePanel.Controls.Add(lblComment, 0, rowIndex + 1);
                tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                lblComment.Width = tablePanel.Width - 20;                
                tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));

                lblUser.MouseHover += TablePanel_MouseHover;
                lblComment.MouseHover += TablePanel_MouseHover;
                rowIndex += 3;
            }

            commentsForm.Controls.Add(tablePanel);
            commentsForm.ShowDialog();
        }

        private void TablePanel_MouseHover(object sender, EventArgs e)
        {
            if (!tablePanel.Focused && tablePanel.VerticalScroll.Visible)
                tablePanel.Focus();
        }
    }
}
