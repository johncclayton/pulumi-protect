namespace pulumi_protect.serviceapi;

public record StackListResponseData
(
    List<StackData> Stacks,
    string ContinuationToken
);