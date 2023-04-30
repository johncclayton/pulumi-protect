namespace pulumi_protect.serviceapi;

public record StackStateResponseData
(
    int Version,
    DeploymentData Deployment
    
    // ability to fetch the encryption key
);
