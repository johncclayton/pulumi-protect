using pprot.serviceapi;
using Spectre.Console;

namespace pprot;

public class Reference
{
    public StackData Stack { get; set; }
    public IDictionary<string, Reference> ChildStacks { get; set; }

    public Reference(StackData data)
    {
        Stack = data;
        ChildStacks = new Dictionary<string, Reference>();
    }

    public void AddChildStackReference(Reference theRef)
    {
        var fqdn = theRef.Stack.FullyQualifiedStackName;
        if (!ChildStacks.ContainsKey(fqdn))
            ChildStacks.Add(fqdn, theRef);
    }

    public void ConsoleDump(HashSet<Reference> tracking, int indent = 0)
    {
        var Padding = " ".PadRight(indent);
        Console.WriteLine($"{Padding}Stack: {Stack.FullyQualifiedStackName}");
        tracking.RemoveWhere(o => o.Stack.FullyQualifiedStackName == Stack.FullyQualifiedStackName);
        foreach (var child in ChildStacks)
        {
            child.Value.ConsoleDump(tracking, indent + 2);
        }
    }
}