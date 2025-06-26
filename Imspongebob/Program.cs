using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Imspongebob
{
    class Program
    {
        static Random random = new Random();
        static System.Media.SoundPlayer player = new System.Media.SoundPlayer("spongebob.wav");

        static int min, max;
        static string allFonts;

        static int waitedFor;
        static int neededToWaitFor;

        static bool cancelWaiting;
        static NotifyIcon notifyIcon;

        static Form hiddenForm;

        static void Main(string[] args)
        {
            if (File.Exists("config.txt"))
            {
                string[] contents = File.ReadAllLines("config.txt");
                if (!int.TryParse(contents[0].Trim().Split('\t')[0], out min))
                {
                    if (!int.TryParse(contents[0].Trim().Split(' ')[0], out min))
                    {
                        min = 60;
                    }
                }

                if (!int.TryParse(contents[1].Trim().Split('\t')[0], out max))
                {
                    if (!int.TryParse(contents[1].Trim().Split(' ')[0], out max))
                    {
                        max = 3600;
                    }
                }
            }
            else
            {
                min = 60;
                max = 3600;
            }

            string configPath = Path.GetFullPath("config.txt");

            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(configPath),
                Filter = Path.GetFileName(configPath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Attributes
            };

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily font in fonts.Families)
            {
                allFonts += font.Name + "\n";
            }

            File.WriteAllText("available_fonts.txt", allFonts);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            notifyIcon = new NotifyIcon()
            {
                Icon = new Icon("hqdefault.ico"),
                Text = "Im Spongebob",
                Visible = true
            };

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Exit", null, (s, e) => Application.Exit());
            notifyIcon.ContextMenuStrip = menu;

            Thread logicThread = new Thread(LoopLogic) { IsBackground = true };
            logicThread.Start();

            hiddenForm = new Form();
            hiddenForm.ShowInTaskbar = false;
            hiddenForm.WindowState = FormWindowState.Minimized;
            hiddenForm.Load += (s, e) => hiddenForm.Hide();

            hiddenForm.Show();
            var handle = hiddenForm.Handle;
            hiddenForm.Hide();

            player.Play();
            ShowWindow();

            Application.Run(hiddenForm);
        }

        static void ShowWindow()
        {
            Form1 form = new Form1();
            form.Shown += (s, e) => form.Close();
            form.Show();
        }

        static void LoopLogic()
        {
            while (true)
            {
                neededToWaitFor = random.Next(min * 1000, max * 1000);
                waitedFor = 0;
                cancelWaiting = false;

                while (waitedFor < neededToWaitFor)
                {
                    Thread.Sleep(100);
                    waitedFor += 100;
                    if (cancelWaiting) break;
                }

                if (!cancelWaiting)
                {
                    player.Play();
                    hiddenForm.Invoke(new Action(() =>
                    {
                        Form1 form = new Form1();
                        form.Shown += (s, e) => form.Close();
                        form.Show();
                    }));
                }
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            cancelWaiting = true;
            string[] contents = File.ReadAllLines("config.txt");
            if (!int.TryParse(contents[0].Trim().Split('\t')[0], out min))
            {
                if (!int.TryParse(contents[0].Trim().Split(' ')[0], out min))
                {
                    min = 60;
                }
            }

            if (!int.TryParse(contents[1].Trim().Split('\t')[0], out max))
            {
                if (!int.TryParse(contents[1].Trim().Split(' ')[0], out max))
                {
                    max = 3600;
                }
            }
        }
    }
}
