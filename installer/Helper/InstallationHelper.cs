using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace LL.Installer.Helper
{
    public static class InstallationHelper
    {
        public static void InstallWindows()
        {
            string systemDrive = GetSystemDriveVariable();
            string home = GetHomeVariable();
            string path = GetPathVariable();

            bool alreadyInstalled = CheckIfInstalled(path);
            if (alreadyInstalled)
            {
                UninstallWindows(path);
                return;
            }

            // create default install location
            string defaultInstallLocation = systemDrive + home + Path.DirectorySeparatorChar + Constants.INSTALL_FOLDER;
            string installLocation = GetInstallationLocation(defaultInstallLocation);
            Console.WriteLine($"{Environment.NewLine}Installing the compiler under:{Environment.NewLine}\t{installLocation}");
#if RELEASE
            CopyFiles(installLocation, path);
#endif
            ConsoleHelper.WriteSuccess($"llCompiler successfully installed!{Environment.NewLine}");
            bool wslInstalled = CheckWslInstallation();
            bool gccInstalled = false;
            if (wslInstalled)
                gccInstalled = CheckGccInstallation();

            if (!wslInstalled)
                ConsoleHelper.WriteWarning("Warning: wsl is not installed; Please consider installing");
            if (!gccInstalled)
                ConsoleHelper.WriteWarning("Warning: gcc is not installed; Please consider installing");
        }

        public static void UninstallWindows() => UninstallWindows(GetPathVariable());

        private static void UninstallWindows(string path)
        {
            string[] pathEntries = path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            string installPath = pathEntries.FirstOrDefault(s => s.Contains(Constants.INSTALL_FOLDER));
            if (string.IsNullOrWhiteSpace(installPath))
            {
                ConsoleHelper.WriteError("Could not find path entry for installation location");
                Environment.Exit(Constants.EXIT_FAILURE);
            }

            Console.WriteLine("Do you really want to uninstall LL? (y/n):");
            string confirmation;
            bool first = true;
            do
            {
                if (first)
                    first = false;
                else
                    ConsoleHelper.WriteWarning("Please confirm with \'y\' or decline with \'n\'");
                Console.Write("$ ");
                confirmation = Console.ReadLine();

            } while (confirmation != "y" && confirmation != "n");

            if (confirmation == "n")
            {
                ConsoleHelper.WriteSuccess("llCompiler won\'t be uninstalled");
                return;
            }

            Console.WriteLine(path);
            path = path.Replace(installPath + Path.PathSeparator, string.Empty);
            Console.WriteLine(path);
# if RELEASE
            Directory.Delete(installPath, true);
            Environment.SetEnvironmentVariable(Constants.PATH_VAR_NAME, path, EnvironmentVariableTarget.User);
# endif
        }

        public static bool CheckIfInstalled(string path)
        {
            bool isInstalled = path.Contains(Constants.INSTALL_FOLDER);
            // compiler is already installed
            if (isInstalled)
                ConsoleHelper.WriteWarning("llCompiler already installed");

            return isInstalled;
        }

        private static string GetInstallationLocation(string defaultLocation)
        {
            Console.WriteLine($"Please enter an installation location (default: {defaultLocation})");
            bool first = true;
            string installLocation = string.Empty;
            bool useDefault = false;

            // request a custom installation location from the user
            do
            {
                if (first)
                    first = false;
                else
                    ConsoleHelper.WriteWarning("Invalid location");

                Console.Write("$ ");
                installLocation = Console.ReadLine();
                useDefault = string.IsNullOrWhiteSpace(installLocation);
            } while (!Directory.Exists(installLocation) && !useDefault);

            // user did not enter custom location
            if (useDefault)
                installLocation = defaultLocation;
            else
            {
                // make sure that the folder the compiler will be installed in is called "llCompiler"
                if (installLocation.EndsWith(Constants.INSTALL_FOLDER))
                    installLocation += Path.DirectorySeparatorChar + Constants.INSTALL_FOLDER;
            }

            return installLocation;
        }

        private static string GetPathVariable()
        {
            string path = Environment.GetEnvironmentVariable(Constants.PATH_VAR_NAME, EnvironmentVariableTarget.User);
            if (string.IsNullOrWhiteSpace(path))
            {
                ConsoleHelper.WriteError("Could not retrieve path variable");
                Environment.Exit(Constants.EXIT_FAILURE);
            }

            return path;
        }

        private static string GetSystemDriveVariable()
        {
            string systemDrive = Environment.GetEnvironmentVariable(Constants.SYSTEM_DRIVE_VAR_NAME);
            if (string.IsNullOrWhiteSpace(systemDrive))
            {
                ConsoleHelper.WriteError("Could not retrieve the system drive");
                Environment.Exit(Constants.EXIT_FAILURE);
            }

            return systemDrive;
        }

        private static string GetHomeVariable()
        {
            string home = Environment.GetEnvironmentVariable(Constants.HOME_VAR_NAME);
            if (string.IsNullOrWhiteSpace(home))
            {
                ConsoleHelper.WriteError("Could not retrieve the home path");
                Environment.Exit(Constants.EXIT_FAILURE);
            }

            return home;
        }

        private static void CopyFiles(string installLocation, string path)
        {
            string headerFolder = installLocation + Path.DirectorySeparatorChar + Constants.HEADER_FOLDER;
            string libFolder = installLocation + Path.DirectorySeparatorChar + Constants.LIB_FOLDER;

            // create installation location
            if (!Directory.Exists(installLocation))
                Directory.CreateDirectory(installLocation);

            if (!Directory.Exists(headerFolder))
                Directory.CreateDirectory(headerFolder);

            if (!Directory.Exists(libFolder))
                Directory.CreateDirectory(libFolder);

            File.Copy($".{Path.DirectorySeparatorChar}{Constants.BINARY_NAME_WIN}", installLocation + Path.DirectorySeparatorChar + Constants.BINARY_NAME_WIN, true);
            File.Copy($".{Path.DirectorySeparatorChar}lib{Path.DirectorySeparatorChar}{Constants.LIB_NAME}", libFolder + Path.DirectorySeparatorChar + Constants.LIB_NAME, true);

            string[] headers = Directory.GetFiles($".{Path.DirectorySeparatorChar}{Constants.HEADER_FOLDER}");
            foreach (string headerPath in headers)
            {
                string headerName = headerPath.Substring(headerPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                string destination = headerFolder + Path.DirectorySeparatorChar + headerName;
                Console.WriteLine($"Copying {headerName} to {destination}");
                File.Copy(headerPath, destination, true);
            }

            path += installLocation + Path.PathSeparator;
            Environment.SetEnvironmentVariable(Constants.PATH_VAR_NAME, path, EnvironmentVariableTarget.User);
        }

        public static bool CheckWslInstallation()
        {
            bool wslFound = true;
            Console.WriteLine("Checking if WSL is installed...");
            ProcessStartInfo procInfo = new ProcessStartInfo("wsl", "echo 0");
            procInfo.CreateNoWindow = true;

            using (Process proc = new Process())
            {
                proc.StartInfo = procInfo;
                try
                {
                    proc.Start();
                    Thread.Sleep(200);

                    if (!proc.HasExited)
                        proc.WaitForExit();
                }
                catch(Exception)
                {
                    wslFound = false;
                }

            }

            if (!wslFound)
                ConsoleHelper.WriteWarning($"WSL is not installed{Environment.NewLine}");
            else
                ConsoleHelper.WriteSuccess($"WSL is installed{Environment.NewLine}");

            return wslFound;
        }

        private static bool CheckGccInstallation()
        {
            bool gccFound = true;
            Console.WriteLine("Checking if gcc is installed...");
            ProcessStartInfo procInfo = new ProcessStartInfo("wsl", "gcc");
            procInfo.CreateNoWindow = true;

            using (Process proc = new Process())
            {
                proc.StartInfo = procInfo;
                try
                {
                    proc.Start();
                    Thread.Sleep(200);

                    if (!proc.HasExited)
                        proc.WaitForExit();
                }
                catch (Exception)
                {
                    gccFound = false;
                }

                if (gccFound)
                    gccFound = proc.ExitCode >= 0 && proc.ExitCode <= 1;
            }

            if (gccFound)
                ConsoleHelper.WriteSuccess($"GCC is installed{Environment.NewLine}");
            else
                ConsoleHelper.WriteWarning($"GCC is not installed{Environment.NewLine}");

            return gccFound;
        }
    }
}