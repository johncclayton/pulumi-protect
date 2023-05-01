namespace pprot.serviceapi;

public class StackUpdateInfo
{
    public string? Kind { get; set; }
    public string? StartTime { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string>? Environment { get; set; }
    public Dictionary<string, ConfigurationData>? Config { get; set; }
    
    public string? Result { get; set; }
    public string? EndTime { get; set; }
    public int? Version { get; set; }
    
    public StackUpdateResourceChanges? ResourceChanges { get; set; }

    public string GetConfigString(string key) => Config![key].String;
    public string GetConfigSecret(string key) => Config![key].Secret;
    public object GetConfigObject(string key) => Config![key].Object;
}