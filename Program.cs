﻿using System.Diagnostics;
using pulumi_protect.serviceapi;

// remove first arg from command line args
var origArgs = Environment.GetCommandLineArgs();
var argsList = new List<string>(origArgs);
argsList.RemoveAt(0);

// is the "command" arg set and is it "destroy"?
var command = argsList.FirstOrDefault();
if (command == "destroy")
{
    var pulumiOrg = "soxes";
    
    // fetch all stacks from the back end service (pulumi service for now)
    var apiClient = new PulumiServiceApiClient(pulumiOrg);
    if (apiClient.ValidatePulumiCliAuthentication() == false)
    {
        // exit 1
        Environment.Exit(1);
    }
    
    var allStacks = await apiClient.GetAllStacksAsync();
    foreach (var theStack in allStacks.Stacks)
    {
        Console.WriteLine($"Destroying {theStack.OrgName}/{theStack.ProjectName}/{theStack.StackName}");
    }
    
    // exit program
    Environment.Exit(0);
}

var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "pulumi",
        Arguments = // join argsList together with spaces
            string.Join(" ", argsList),
        UseShellExecute = false,
        RedirectStandardOutput = false,
        RedirectStandardError = false,
        RedirectStandardInput = false,
        CreateNoWindow = true,
    },
};

// run the process and wait for it to finish
process.Start();
process.WaitForExit();

