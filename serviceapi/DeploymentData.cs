namespace pprot.serviceapi;

public record SecretsProvidersData(string Type, Dictionary<string, object> State)
{
    public bool IsAzureKeyVault()
    {
        return Type == "cloud";
    }
}
public record ResourceData(string Urn, bool Custom, string Type, Dictionary<string, object> Inputs,
    Dictionary<string, object> Outputs);

public record DeploymentData(SecretsProvidersData Secrets_Providers, ResourceData[] Resources);

    
