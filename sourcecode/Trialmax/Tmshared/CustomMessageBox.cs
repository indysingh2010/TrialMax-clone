using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Drawing;

namespace FTI.Shared
{
    public enum CustomMessageButtonType
    {
        YesYesToAllNo = 0,
    }

    public enum CustomMessageDilaogResult
    {
        Yes = 0,
        YesToAll = 1,
        No = 2
    }

    /// <summary> This class is used to generate custom message box with custom buttons</summary>
    public static class CustomMesssageBox
    {
        public static CustomMessageDilaogResult ShowDialog(string text, string caption, CustomMessageButtonType btnType)
        {
            CustomMessageDilaogResult result = 0;

            Form prompt = new Form();
            prompt.MinimizeBox = false;
            prompt.MaximizeBox = false;
            prompt.ShowIcon = false;
            prompt.Width = 396;
            prompt.Height = 160;
            prompt.Text = caption;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            prompt.AutoSize = true;
            prompt.StartPosition = FormStartPosition.CenterParent;

            TextBox textLabel = new TextBox() { Size = new Size(397, 72), Location = new Point(-5, 0), Multiline = true, ReadOnly = true, TabStop = false, TextAlign = HorizontalAlignment.Center };
            textLabel.BackColor = Color.White;

            textLabel.Text += System.Environment.NewLine;
            textLabel.Text += System.Environment.NewLine;

            textLabel.Text += text;

            textLabel.Text += System.Environment.NewLine;
            textLabel.Text += System.Environment.NewLine;

            using (Graphics gfx = prompt.CreateGraphics())
            {
                // Get the size given the string and the font
                var size = gfx.MeasureString(textLabel.Text, textLabel.Font);
                textLabel.Height = (int)size.Height + 20;
                prompt.Height = textLabel.Height + 90;
            }


            switch (btnType)
            {
                case CustomMessageButtonType.YesYesToAllNo:
                    {
                        Button yes = new Button() { Text = "Yes", Left = 150, Width = 82, Height = 23, Location = new Point(110, textLabel.Size.Height + 20) };
                        Button yesToAll = new Button() { Text = "Yes To All", Left = 210, Width = 82, Height = 23, Location = new Point(198, textLabel.Size.Height + 20) };
                        Button no = new Button() { Text = "No", Left = 270, Width = 82, Height = 23, Location = new Point(286, textLabel.Size.Height + 20) };
                        yes.Click += (sender, e) => { result = CustomMessageDilaogResult.Yes; prompt.Close(); };
                        yesToAll.Click += (sender, e) => { result = CustomMessageDilaogResult.YesToAll; prompt.Close(); };
                        no.Click += (sender, e) => { result = CustomMessageDilaogResult.No; prompt.Close(); };
                        prompt.Controls.Add(textLabel);
                        prompt.Controls.Add(yes);
                        prompt.Controls.Add(yesToAll);
                        prompt.Controls.Add(no);
                    } break;
            }


            prompt.ShowDialog();
            return result;
        }
    }

}
