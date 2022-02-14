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
var unneededPackages = new List<string>();

string? line;
while ((line = packageCmd?.StandardOutput.ReadLine()) != null)
{
    if (!line.StartsWith("i"))
        continue;

    var parts = line.Split("|");
    unneededPackages.Add(parts[2].Trim());
}

if (unneededPackages.Count == 0)
{
    Console.WriteLine("何もすることがありません。"); // Nothing to do.
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
