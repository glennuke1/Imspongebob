using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace ImSpongebobInstaller
{
    class Program
    {
        static string installationDirectory;
        static string tempDirectory;

        static void Main(string[] args)
        {
            installationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ImSpongebob";
            tempDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ImSpongebob/Temp";

            if (!Directory.Exists(installationDirectory))
            {
                Directory.CreateDirectory(installationDirectory);
            }

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/glennuke1/Imspongebob/raw/refs/heads/master/Imspongebob/Builds/FinishedZip/Imspongebob.zip", tempDirectory + "/Imspongebob.zip");
            }

            ZipFile zip = ZipFile.Read(tempDirectory + "/Imspongebob.zip");
            zip.ExtractAll(tempDirectory, ExtractExistingFileAction.OverwriteSilently);
            zip.Dispose();

            if (File.Exists(installationDirectory + "/config.txt"))
            {
                File.Delete(tempDirectory + "/config.txt");
            }

            if (File.Exists(installationDirectory + "/contents.txt"))
            {
                File.Delete(tempDirectory + "/contents.txt");
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                WorkingDirectory = installationDirectory,
                FileName = installationDirectory + "/Imspongebob.exe",
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
    }
}
