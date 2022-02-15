// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

var packageCommand = new ProcessStartInfo
{
    FileName = "/usr/bin/zypper",
    Arguments = "pa --unneeded",
    RedirectStandardOutput = true,
    UseShellExecute = false
};

var packageCmd = Process.Start(packageCommand);

var output = packageCmd?.StandardOutput.ReadToEnd();
var unneededPackages = (from line in output?.Split("\n")!
    where line.StartsWith("i")
    select line.Split("|")
    into parts
    select parts[2].Trim()).ToList();

if (unneededPackages.Count == 0)
{
    Console.WriteLine(output); // Nothing to do.
    Environment.Exit(0);
}

var removeCommand = new ProcessStartInfo
{
    FileName = "/usr/bin/zypper",
    Arguments = $"rm {string.Join(' ', unneededPackages)}",
    RedirectStandardInput = true
    //RedirectStandardOutput = true,
    //UseShellExecute = false
};

var proc = Process.Start(removeCommand);
proc?.WaitForExit();
