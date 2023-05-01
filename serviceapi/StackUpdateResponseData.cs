namespace pprot.serviceapi;

public record StackUpdateResponseData
(
    List<StackUpdateData> Updates,
    string ContinuationToken
);