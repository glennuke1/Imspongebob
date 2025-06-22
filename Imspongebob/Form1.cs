using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace Imspongebob
{
    public partial class Form1 : Form
    {
        List<string> contents = new List<string>();
        string title = "Spongebob";
        string font = "Times New Roman";
        float titleFont = 30f;
        float contentFont = 15f;

        public Form1()
        {
            InitializeComponent();

            if (File.Exists("config.txt"))
            {
                string[] _contents = File.ReadAllLines("config.txt");
                title = _contents[2].Trim().Split('\t')[0];

                if (!float.TryParse(_contents[3].Trim().Split('\t')[0], out titleFont))
                {
                    if (!float.TryParse(_contents[3].Trim().Split(' ')[0], out titleFont))
                    {
                        titleFont = 30f;
                    }
                }

                if (!float.TryParse(_contents[4].Trim().Split('\t')[0], out contentFont))
                {
                    if (!float.TryParse(_contents[4].Trim().Split(' ')[0], out contentFont))
                    {
                        contentFont = 30f;
                    }
                }

                font = _contents[5].Trim().Split('\t')[0];

                InstalledFontCollection fonts = new InstalledFontCollection();
                bool fontExists = fonts.Families.Any(f => string.Equals(f.Name, font, StringComparison.OrdinalIgnoreCase));

                if (!fontExists)
                {
                    font = "Times New Roman";
                }
            }
            else
            {
                title = "Spongebob";
                titleFont = 30f;
                contentFont = 15f;
            }

            if (File.Exists("contents.txt"))
            {
                string[] _contents = File.ReadAllLines("contents.txt");
                for (int i = 0; i < _contents.Length; i++)
                {
                    contents.Add(_contents[i]);
                }
            }

            var popupNotifier = new PopupNotifier();
            popupNotifier.TitleText = title;
            popupNotifier.ContentText = contents[new Random().Next(0, contents.Count)];
            popupNotifier.IsRightToLeft = false;
            popupNotifier.TitleFont = new Font(font, titleFont);
            popupNotifier.ContentFont = new Font(font, contentFont);
            popupNotifier.Image = Image.FromFile("hqdefault.jpg");
            popupNotifier.ImageSize = new Size(120, 100);
            popupNotifier.Delay = 1000000000;
            popupNotifier.Popup();
            popupNotifier.Close += PopupNotifier_Close;
        }

        private void PopupNotifier_Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
