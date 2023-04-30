namespace pulumi_protect.serviceapi;

public record StackUpdateResponseData
(
    List<StackUpdateData> Updates,
    string ContinuationToken
);