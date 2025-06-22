using Microsoft.Win32;
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

        static void Main(string[] args)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string exePath = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"";
            rk.SetValue("Spongebob", exePath);

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

            player.Play();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            Application.Run(form);
            Run();
        }

        static void Run()
        {
            neededToWaitFor = random.Next(min * 1000, max * 1000);
            while (waitedFor < neededToWaitFor)
            {
                Thread.Sleep(100);
                waitedFor += 100;
                if (cancelWaiting)
                {
                    waitedFor = neededToWaitFor;
                }
            }
            waitedFor = 0;
            cancelWaiting = false;
            player.Play();
            Form1 form = new Form1();
            Application.Run(form);
            Run();
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
