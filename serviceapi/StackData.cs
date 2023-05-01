namespace pprot.serviceapi;

public record StackData(
    string OrgName,
    string ProjectName,
    string StackName,
    DateTime LastUpdateTime,
    int ResourceCount
);