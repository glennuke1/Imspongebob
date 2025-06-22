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

        static void Main(string[] args)
        {
            installationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "/ImSpongebob";

            if (!Directory.Exists(installationDirectory))
            {
                Directory.CreateDirectory(installationDirectory);
            }

            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/glennuke1/Imspongebob/raw/refs/heads/master/Imspongebob/Builds/FinishedZip/Imspongebob.zip", "Imspongebob.zip");
            }

            ZipFile zip = ZipFile.Read("Imspongebob.zip");
            zip.ExtractAll(installationDirectory, ExtractExistingFileAction.OverwriteSilently);
            zip.Dispose();

            Process.Start(installationDirectory + "/Imspongebob.exe");
        }
    }
}
