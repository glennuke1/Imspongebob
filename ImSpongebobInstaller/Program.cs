using Ionic.Zip;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using File = System.IO.File;

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

            foreach (Process process in Process.GetProcessesByName("imspongebob"))
            {
                process.Kill();
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

            foreach (string file in Directory.GetFiles(tempDirectory))
            {
                File.Copy(file, installationDirectory + "/" + Path.GetFileName(file), true);
            }

            foreach (string file in Directory.GetFiles(tempDirectory))
            {
                File.Delete(file);
            }

            Directory.Delete(tempDirectory);

            if (File.Exists(installationDirectory + "/Imspongebob.zip"))
            {
                File.Delete(installationDirectory + "/Imspongebob.zip");
            }

            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "ImSpongebob.lnk");
            string exePath = installationDirectory + "/Imspongebob.exe";

            if (!File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = exePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
                shortcut.Save();
            }

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.DeleteValue("Spongebob");

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
