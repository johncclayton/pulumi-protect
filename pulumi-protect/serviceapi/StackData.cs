namespace pulumi_protect.serviceapi;

public record StackData(
    string OrgName,
    string ProjectName,
    DateTime LastUpdateTime,
    int ResourceCount
);