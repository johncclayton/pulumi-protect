using System.Diagnostics;

using pprot;
using pprot.serviceapi;

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
    var stackReferences = allStacks.Stacks.Select(o => new Reference(o)).ToList();
    
    // primitive: find the -s argument
    string? argStackName = null;
    for (int index = 0; index < argsList.Count - 1; ++index)
    {
        if(args[index] == "-s" || args[index] == "--stack")
            argStackName = args[index + 1];
    }

    if (argStackName == null)
        throw new ArgumentException("Could not find the current stack, or --stack was not specified");
    
    Console.WriteLine($"Detected stack to destroy: {argStackName}");
    
    // write some reactive code to: 
    // - fetch all the stacks
    // - fetch the current resource state for each stack
    // - determine if any of the resources are stack references, and to what they point to
    // - if there are any stacks pointed to, then pull their stack resources and repeat
    foreach (var theRef in stackReferences)
    {
        if (theRef.Stack.ResourceCount == 0)
            continue;

        var myFullyQualifiedStackName = theRef.Stack.FullyQualifiedStackName;

        var stackState = await apiClient.GetStackStateAsync(myFullyQualifiedStackName);
        var refs = stackState.Deployment.Resources.Where(o => o.Urn.Contains("pulumi:pulumi:StackReference"));

        foreach (var stackRef in refs)
        {
            var parentStackName = stackRef.Inputs["name"].ToString();
            Console.WriteLine($"On stack: {myFullyQualifiedStackName}, looking up referenced stack: {parentStackName}");
            
            var theParent =
                stackReferences.FirstOrDefault(o => o.Stack.FullyQualifiedStackName == parentStackName);
            
            if (theParent == null)
                Console.WriteLine(
                    $"Failed to find the stack that was referenced, which was: {parentStackName}");
            else
                theParent.AddChildStackReference(theRef);
        }
    }

    // this will allow us to form a tree, or series of stack references
    var tracking = stackReferences.ToHashSet();
    while (tracking.Count > 0)
    {
        var firstItem = tracking.First();
        firstItem.ConsoleDump(tracking);
    }

    // if our stack is being targeted in the tree - then DO NOT allow the destroy command
    
    // protection : exit program regardless.
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

// run the process and wait for it to finish - redirection of keyboard allows users to 
// select stacks and answer prompts.
process.Start();

process.WaitForExit();


