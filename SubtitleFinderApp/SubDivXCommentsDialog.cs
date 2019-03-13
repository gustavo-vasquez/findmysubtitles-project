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
        Label lblComment;
        Label lblUser;

        public void ShowDialog(string text, string caption, IEnumerable<HtmlNode> userCommentsNodes)
        {
            Form commentsForm = new Form();
            commentsForm.Width = 500;
            commentsForm.Height = 500;
            commentsForm.Text = caption;
            commentsForm.StartPosition = FormStartPosition.CenterParent;

            lblComment = new Label()
            {
                Left = 40,
                Top = 0,
                Text = text,
                Width = 400,
                MaximumSize = new System.Drawing.Size(400, 0),
                AutoSize = true
            };

            lblUser = new Label()
            {
                Left = 40,
                Top = 30,
                Text = "Pepe argento dijo:",
                Width = 400,
                MaximumSize = new System.Drawing.Size(400, 0),
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold)                
            };

            //NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 100, Width = 400 };
            Button confirmation = new Button()
            {
                Text = "Cerrar",
                Left = 190,
                Width = 100,
                Top = 150
            };

            confirmation.Click += (sender, e) => { commentsForm.Close(); };
            commentsForm.Controls.Add(confirmation);            
            commentsForm.Controls.Add(lblComment);
            lblUser.SizeChanged += new EventHandler(lblUser_SizeChanged);
            commentsForm.Controls.Add(lblUser);
            //commentsForm.Controls.Add(inputBox);
            commentsForm.ShowDialog();
        }

        private void lblUser_SizeChanged(object sender, EventArgs e)
        {
            lblComment.Top = lblUser.Top + lblUser.Height + 10;
            //label2.Top = label1.Top;
        }
    }
}
