using System;
using System.IO;
using LL.Installer;

// TODO implement linux version
string systemDriveVarName = "SystemDrive";
string homeVarName = "HOMEPATH";
string pathVarName = "Path";
string libName = "libLL.a";
string binaryName = "llCompiler.exe";
int EXIT_FAILURE = -1;
int EXIT_SUCCESS = 0;

string systemDrive = Environment.GetEnvironmentVariable(systemDriveVarName);
string home = Environment.GetEnvironmentVariable(homeVarName);

string path = Environment.GetEnvironmentVariable(pathVarName);
if(string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("Could not retrieve path variable");
    Environment.Exit(EXIT_FAILURE);
}

// compiler is already installed
if(path.Contains(Constants.INSTALL_FOLDER))
{
    Console.WriteLine("llCompiler already installed");
    Environment.Exit(EXIT_SUCCESS);
}

if(string.IsNullOrWhiteSpace(systemDrive))
{
    Console.WriteLine("Could not retrieve the system drive");
    Environment.Exit(EXIT_FAILURE);
}

if(string.IsNullOrWhiteSpace(home))
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
    if(first)
        first = false;
    else
        Console.WriteLine("Invalid location");
    
    Console.Write("$ ");
    installLocation = Console.ReadLine();
    useDefault = string.IsNullOrWhiteSpace(installLocation);
} while(!Directory.Exists(installLocation) && !useDefault);

// user did not enter custom location
if(useDefault)
    installLocation = defaultInstallLocation;
else
{
    // make sure that the folder the compiler will be installed in is called "llCompiler"
    if(installLocation.EndsWith(Constants.INSTALL_FOLDER))
        installLocation += Path.DirectorySeparatorChar + Constants.INSTALL_FOLDER;
}

Console.WriteLine($"Installing the compiler under:{Environment.NewLine}\t{installLocation}");

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

ConsoleColor prev = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}llCompiler successfully installed!{Environment.NewLine}{Environment.NewLine}");
Console.ForegroundColor = prev;
Console.WriteLine("Press any key to exit...");
Console.ReadKey();