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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubtitleFinderForm));
            Form commentsForm = new Form();
            commentsForm.Width = 500;
            commentsForm.Height = 500;
            commentsForm.Text = caption;
            commentsForm.StartPosition = FormStartPosition.CenterParent;
            commentsForm.Icon = Properties.Resources.comment_edit;
            //int offset = 30;
            int rowIndex = 0;

            FlowLayoutPanel flowPanel = new FlowLayoutPanel()
            {
                //Left = 50,
                Width = 420,
                Height = 390,
                Location = new System.Drawing.Point(30, 20),
                AutoScroll = true,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)))
            };

            TableLayoutPanel tablePanel = new TableLayoutPanel()
            {
                Location = new System.Drawing.Point(32, 20),
                //Left = 32,
                //Top = 20,
                //Width = 420,
                //Height = 390,
                ColumnCount = 1,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                AutoScroll = true,                
                //AutoSize = true,
                Size = new System.Drawing.Size(420, 390),                
                Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)))
            };

            foreach (var div in userCommentsDivs)
            {
                lblUser = new Label()
                {
                    Text = div.Descendants("div").SingleOrDefault().Descendants("a").SingleOrDefault().InnerText,
                    Margin = new System.Windows.Forms.Padding(3, 0, 3, 0),
                    //Width = 400,
                    //MaximumSize = new System.Drawing.Size(400, 0),
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

                //lblUser.SizeChanged += new EventHandler(lblUser_SizeChanged);
                
                tablePanel.Controls.Add(lblComment, 0, rowIndex + 1);
                tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                lblComment.Width = tablePanel.Width - 20;                
                tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));

                //offset = lblUser.Top + lblComment.Height + 20;
                rowIndex += 3;
            }            

            //NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 100, Width = 400 };
            Button btnClose = new Button()
            {
                Text = "Cerrar",                
                Width = 100,
                Location = new System.Drawing.Point(190, 420),
                Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)))
            };

            btnClose.Click += (sender, e) => { commentsForm.Close(); };
            commentsForm.Controls.Add(btnClose);            
            //lblUser.SizeChanged += new EventHandler(lblUser_SizeChanged);
            //commentsForm.Controls.Add(lblUser);                        
            //commentsForm.Controls.Add(inputBox);
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));            
            commentsForm.Controls.Add(tablePanel);
            commentsForm.ShowDialog();            
        }

        //private void lblUser_SizeChanged(object sender, EventArgs e)
        //{
        //    //lblComment.Top = lblUser.Top + lblUser.Height + 10;
        //    //label2.Top = label1.Top;
        //}
    }
}
