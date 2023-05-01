namespace pprot.serviceapi;

public record StackListResponseData
(
    List<StackData> Stacks,
    string ContinuationToken
);