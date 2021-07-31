using System;
using LL.Installer;
using LL.Installer.Helper;

// TODO implement linux version
if (args.Length > 1)
    ConsoleHelper.WriteRTFM();
if (args.Length == 1)
{
    if (args[0] == "-d")
        InstallationHelper.UninstallWindows();
    else
        ConsoleHelper.WriteRTFM();
}
else
{
    InstallationHelper.InstallWindows();
}
Console.WriteLine(InstallationHelper.CheckWslInstallation());

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
Environment.Exit(Constants.EXIT_SUCCESS);