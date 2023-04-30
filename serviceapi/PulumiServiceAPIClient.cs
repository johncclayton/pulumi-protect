using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Flurl.Http;

namespace pulumi_protect.serviceapi;

public class PulumiServiceApiClient
{
    private string PulumiOrganization { get; set; }
    private string RootPulumiApi { get; set; }
    private string? PulumiAccessToken { get; set; }
    
    public PulumiServiceApiClient(string org = "soxes", string? accessToken = null)
    {
        RootPulumiApi = "https://api.pulumi.com";
        PulumiOrganization = org;
        
        if(PulumiAccessToken == null || accessToken != null)
            PulumiAccessToken = accessToken ?? Environment.GetEnvironmentVariable("PULUMI_ACCESS_TOKEN");
    }

    public async Task<StackListResponseData> GetAllStacksAsync()
    {
        var response = await CreateRequest()
            .AppendPathSegment("api/user/stacks")
            .SetQueryParam("organization", PulumiOrganization)
            .GetJsonAsync<StackListResponseData>();
        return response;
    }

    public async Task<StackResponseData> GetStackAsync(string fullQualifiedStackName)
    {
        return await CreateRequest()
            .AppendPathSegment($"api/stacks/{fullQualifiedStackName}")
            .GetJsonAsync<StackResponseData>();
    }

    public async Task<StackStateResponseData> GetStackStateAsync(string fullQualifiedStackName)
    {
        return await CreateRequest()
            .AppendPathSegment($"api/stacks/{fullQualifiedStackName}/export")
            .GetJsonAsync<StackStateResponseData>();
    }
    
    public async Task<StackUpdateResponseData> GetStackLastUpdateAsync(string fullQualifiedStackName)
    {
        return await CreateRequest()
            .AppendPathSegment($"api/stacks/{fullQualifiedStackName}/updates")
            .SetQueryParam("pageSize", 1)
            .SetQueryParam("page", 1)
            .SetQueryParam("output-type", "service")
            .GetJsonAsync<StackUpdateResponseData>();
    }

    // public async Task<string> DecryptCipherTextForStackAsync(string encryptedBase64)
    // {
    //     var response = await CreateRequest()
    //         .AppendPathSegment($"api/stacks/{FullyQualifiedStackName}/decrypt")
    //         .PostJsonAsync(new { ciphertext = encryptedBase64 });
    //     dynamic value = await response.GetJsonAsync<dynamic>();
    //     var valueBytes = Convert.FromBase64String(value["plaintext"].ToString());
    //     return System.Text.Encoding.UTF8.GetString(valueBytes);
    // }

    private IFlurlRequest CreateRequest()
    {
        return RootPulumiApi
            .WithHeader("Authorization", $"token {PulumiAccessToken}")
            .WithHeader("Accept", "application/vnd.pulumi+8")
            .WithHeader("Content-Type", "application/json");
    }

    public bool ValidatePulumiCliAuthentication()
    {
        // is the pulumi access token set?
        var pulumiAccessToken = Environment.GetEnvironmentVariable("PULUMI_ACCESS_TOKEN");
        if (pulumiAccessToken != null)
        {
            PulumiAccessToken = pulumiAccessToken;
            return PulumiAccessToken != null;
        }

        // credentials.json is in the users home directory
        var credentialsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".pulumi",
            "credentials.json");
        
        if (!File.Exists(credentialsFile))
        {
            throw new Exception("Pulumi CLI is not authenticated. Please run `pulumi login`.");
        }
        
        // read the file contents, it's JSON
        var credentialsJson = File.ReadAllText(credentialsFile);
        
        // parse the JSON
        var credentials = PulumiCredentials.FromJson(credentialsJson);
        
        // well, it didn't explode - that's generally a good sign.
        PulumiAccessToken = credentials.AccessTokens?.HttpsApiPulumiCom;

        return PulumiAccessToken != null;
    }
}