namespace pulumi_protect.serviceapi;

public record StackOperation
(
    string Kind, 
    string Author, 
    DateTime Started 
);