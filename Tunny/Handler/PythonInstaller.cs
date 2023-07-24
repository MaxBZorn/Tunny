using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using Python.Included;

namespace Tunny.Handler
{
    public static class PythonInstaller
    {
        public static string Path { get; set; } = ".";
        public static void Run(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            string[] packageList = GetTunnyPackageList();
            int installItems = packageList.Length + 2;

            Installer.InstallPath = Path;
            Installer.SetupPython();
            worker?.ReportProgress(100 / installItems, "Now installing Python runtime...");
            Installer.TryInstallPip();
            worker?.ReportProgress(200 / installItems, "Now installing pip...");

            InstallPackages(worker, packageList, installItems);

            worker?.ReportProgress(100, "Finish!!");
        }

        private static void InstallPackages(BackgroundWorker worker, IReadOnlyList<string> packageList, int installItems)
        {
            for (int i = 0; i < packageList.Count; i++)
            {
                string packageName = packageList[i].Split('=')[0] == "plotly"
                    ? packageList[i] + "... This package will take time to install. Please wait"
                    : packageList[i];
                worker.ReportProgress((i + 2) * 100 / installItems, "Now installing " + packageName + "...");
                Installer.PipInstallModule(packageList[i]);
            }
        }

        internal static bool CheckPackagesIsInstalled()
        {
            Installer.InstallPath = Path;
            string[] packageList = GetTunnyPackageList().Select(s => s.Split('=')[0]).ToArray();
            if (!Installer.IsPythonInstalled())
            {
                return false;
            }
            if (!Installer.IsPipInstalled())
            {
                return false;
            }
            foreach (string package in packageList)
            {
                string[] singleFilePackages = { "bottle", "optuna-dashboard", "six", "PyYAML", "scikit-learn", "threadpoolctl", "typing_extensions" };
                string[] useUnderLinePackages = { "opt-einsum", "pyro-api", "pyro-ppl" };
                if (!Installer.IsModuleInstalled(package) && !singleFilePackages.Contains(package) && !useUnderLinePackages.Contains(package))
                {
                    return false;
                }
            }
            return true;
        }

        internal static string GetEmbeddedPythonPath()
        {
            Installer.InstallPath = Path;
            return Installer.EmbeddedPythonHome;
        }

        private static string[] GetTunnyPackageList()
        {
            var pipPackages = new List<string>();

            using (var sr = new StreamReader(Path + "/Lib/requirements.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    pipPackages.Add(line);
                }
            }
            return pipPackages.ToArray();
        }
    }
}
