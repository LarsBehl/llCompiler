using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using LL.Installer;

// TODO implement linux version
string systemDriveVarName = "SystemDrive";
string homeVarName = "HOMEPATH";
string pathVarName = "Path";
string wslVarName = "WSLENV";
#if RELEASE
string libName = "libLL.a";
string binaryName = "llCompiler.exe";
#endif
int EXIT_FAILURE = -1;
int EXIT_SUCCESS = 0;
ConsoleColor prev = Console.ForegroundColor;

string systemDrive = Environment.GetEnvironmentVariable(systemDriveVarName);
string home = Environment.GetEnvironmentVariable(homeVarName);

string path = Environment.GetEnvironmentVariable(pathVarName);
if (string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("Could not retrieve path variable");
    Environment.Exit(EXIT_FAILURE);
}

// compiler is already installed
if (path.Contains(Constants.INSTALL_FOLDER))
{
    Console.WriteLine("llCompiler already installed");
    Environment.Exit(EXIT_SUCCESS);
}

if (string.IsNullOrWhiteSpace(systemDrive))
{
    Console.WriteLine("Could not retrieve the system drive");
    Environment.Exit(EXIT_FAILURE);
}

if (string.IsNullOrWhiteSpace(home))
{
    Console.WriteLine("Could not retrieve the home path");
    Environment.Exit(EXIT_FAILURE);
}

// create default install location
string defaultInstallLocation = systemDrive + home + Path.DirectorySeparatorChar + Constants.INSTALL_FOLDER;

Console.WriteLine($"Please enter an installation location (default: {defaultInstallLocation})");
bool first = true;
string installLocation = string.Empty;
bool useDefault = false;

// request a custom installation location from the user
do
{
    if (first)
        first = false;
    else
        Console.WriteLine("Invalid location");

    Console.Write("$ ");
    installLocation = Console.ReadLine();
    useDefault = string.IsNullOrWhiteSpace(installLocation);
} while (!Directory.Exists(installLocation) && !useDefault);

// user did not enter custom location
if (useDefault)
    installLocation = defaultInstallLocation;
else
{
    // make sure that the folder the compiler will be installed in is called "llCompiler"
    if (installLocation.EndsWith(Constants.INSTALL_FOLDER))
        installLocation += Path.DirectorySeparatorChar + Constants.INSTALL_FOLDER;
}

Console.WriteLine($"{Environment.NewLine}Installing the compiler under:{Environment.NewLine}\t{installLocation}");

# if RELEASE
string headerFolder = installLocation + Path.DirectorySeparatorChar + Constants.HEADER_FOLDER;
string libFolder = installLocation + Path.DirectorySeparatorChar + Constants.LIB_FOLDER;

// create installation location
if(!Directory.Exists(installLocation))
    Directory.CreateDirectory(installLocation);

if(!Directory.Exists(headerFolder))
    Directory.CreateDirectory(headerFolder);

if(!Directory.Exists(libFolder))
    Directory.CreateDirectory(libFolder);

File.Copy($".{Path.DirectorySeparatorChar}{binaryName}", installLocation + Path.DirectorySeparatorChar + binaryName, true);
File.Copy($".{Path.DirectorySeparatorChar}lib{Path.DirectorySeparatorChar}{libName}", libFolder + Path.DirectorySeparatorChar + libName, true);

string[] headers = Directory.GetFiles($".{Path.DirectorySeparatorChar}{Constants.HEADER_FOLDER}");
foreach(string headerPath in headers)
{
    string headerName = headerPath.Substring(headerPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
    string destination = headerFolder + Path.DirectorySeparatorChar + headerName;
    Console.WriteLine($"Copying {headerName} to {destination}");
    File.Copy(headerPath, destination, true);
}

path += Path.PathSeparator + installLocation;
Environment.SetEnvironmentVariable(pathVarName, path, EnvironmentVariableTarget.User);
# endif

Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine($"llCompiler successfully installed!{Environment.NewLine}");
Console.ForegroundColor = prev;

bool wslFound = true;
bool gccFound = true;
Console.WriteLine("Checking if WSL is installed...");

string wslEnv = Environment.GetEnvironmentVariable(wslVarName);
wslFound = gccFound = !string.IsNullOrWhiteSpace(wslEnv);

if (!wslFound)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"WSL is not installed{Environment.NewLine}");
    Console.ForegroundColor = prev;
}
else
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"WSL is installed{Environment.NewLine}");
    Console.ForegroundColor = prev;
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
            
            if(!proc.HasExited)
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
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"GCC is installed{Environment.NewLine}");
        Console.ForegroundColor = prev;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"GCC is not installed{Environment.NewLine}");
        Console.ForegroundColor = prev;
    }
}

Console.ForegroundColor = ConsoleColor.Yellow;
if (!wslFound)
    Console.WriteLine("Warning: wsl is not installed; Please consider installing");
if (!gccFound)
    Console.WriteLine("Warning: gcc is not installed; Please consider installing");
Console.ForegroundColor = prev;
Console.WriteLine("Press any key to exit...");
Console.ReadKey();