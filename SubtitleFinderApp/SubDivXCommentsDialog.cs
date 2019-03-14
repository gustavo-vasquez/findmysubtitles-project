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
        private Label lblComment, lblUser;

        public void ShowDialog(string text, string caption, IEnumerable<HtmlNode> userCommentsDivs)
        {
            Form commentsForm = new Form();
            commentsForm.Width = 500;
            commentsForm.Height = 500;
            commentsForm.Text = caption;
            commentsForm.StartPosition = FormStartPosition.CenterParent;
            int offset = 30;

            //FlowLayoutPanel flowPanel = new FlowLayoutPanel()
            //{
            //    Left = 50,
            //    Width = 400,
            //    AutoScroll = true
            //};

            //TableLayoutPanel tablePanel = new TableLayoutPanel()
            //{
            //    Location = new System.Drawing.Point(3, 3),
            //    Width = 394,
            //    Height = 250,
            //    ColumnCount = 1,
            //    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            //};

            foreach (var div in userCommentsDivs)
            {
                lblUser = new Label()
                {
                    Left = 40,
                    Top = offset,
                    Text = div.Descendants("div").SingleOrDefault().Descendants("a").SingleOrDefault().InnerText,
                    Width = 400,
                    MaximumSize = new System.Drawing.Size(400, 0),
                    AutoSize = true,                    
                    Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold)
                };

                lblComment = new Label()
                {
                    Left = 40,
                    Top = offset,
                    Text = div.FirstChild.InnerText,
                    Width = 400,
                    MaximumSize = new System.Drawing.Size(400, 0),
                    AutoSize = true
                };

                lblUser.SizeChanged += new EventHandler(lblUser_SizeChanged);
                //tablePanel.Controls.Add(lblUser, 0, 0);
                //tablePanel.Controls.Add(lblComment, 0, 1);
                //commentsForm.Controls.Add(tablePanel);
                commentsForm.Controls.Add(lblUser);
                commentsForm.Controls.Add(lblComment);

                offset = lblUser.Top + lblComment.Height + 20;
            }

            //lblComment = new Label()
            //{
            //    Left = 40,
            //    Top = 0,
            //    Text = text,
            //    Width = 400,
            //    MaximumSize = new System.Drawing.Size(400, 0),
            //    AutoSize = true
            //};

            //lblUser = new Label()
            //{
            //    Left = 40,
            //    Top = 30,
            //    Text = "Pepe argento dijo:",
            //    Width = 400,
            //    MaximumSize = new System.Drawing.Size(400, 0),
            //    AutoSize = true,
            //    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
            //    Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold)
            //};

            //lblComment2 = new Label()
            //{
            //    Left = 40,
            //    Top = 0,
            //    Text = text,
            //    Width = 400,
            //    MaximumSize = new System.Drawing.Size(400, 0),
            //    AutoSize = true                
            //};

            //lblUser2 = new Label()
            //{
            //    Left = 40,
            //    Top = 100,
            //    Text = "Pepe argento dijo:",
            //    Width = 400,
            //    MaximumSize = new System.Drawing.Size(400, 0),
            //    AutoSize = true,
            //    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
            //    Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold)
            //};

            //NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 100, Width = 400 };
            //Button confirmation = new Button()
            //{
            //    Text = "Cerrar",
            //    Left = 190,
            //    Width = 100,
            //    Top = 150
            //};

            //confirmation.Click += (sender, e) => { commentsForm.Close(); };
            //commentsForm.Controls.Add(confirmation);
            //commentsForm.Controls.Add(lblComment);
            //commentsForm.Controls.Add(lblComment2);
            //lblUser.SizeChanged += new EventHandler(lblUser_SizeChanged);
            //commentsForm.Controls.Add(lblUser);
            //lblUser2.SizeChanged += new EventHandler(lblUser2_SizeChanged);
            //commentsForm.Controls.Add(lblUser2);
            //commentsForm.Controls.Add(inputBox);            
            commentsForm.ShowDialog();
        }

        private void lblUser_SizeChanged(object sender, EventArgs e)
        {
            //lblComment.Top = lblUser.Top + lblUser.Height + 10;
            //label2.Top = label1.Top;
        }

        //private void lblUser2_SizeChanged(object sender, EventArgs e)
        //{
        //    lblComment2.Top = lblUser2.Top + lblUser2.Height + 10;
        //    //label2.Top = label1.Top;
        //}
    }
}
