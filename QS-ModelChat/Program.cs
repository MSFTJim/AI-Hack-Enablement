﻿using Microsoft.Extensions.Configuration;

Console.WriteLine("Start of Program");

// Load configuration and get values
var (projectEndpoint, modelDeploymentName, contentsafetyEndpoint) = LoadConfiguration();

Console.WriteLine($"Project Endpoint: {projectEndpoint}");
Console.WriteLine($"Model Deployment Name: {modelDeploymentName}");
Console.WriteLine($"Content Safety Endpoint: {contentsafetyEndpoint}");




//////// MAIN Program END ////////

// Load configuration from appsettings, user secrets, and environment variables
(string, string, string) LoadConfiguration()
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddUserSecrets(typeof(Program).Assembly, optional: true)
        .AddEnvironmentVariables()
        .Build();

    string projectEndpoint = config["PROJECT_ENDPOINT"]
        ?? throw new InvalidOperationException("Missing PROJECT_ENDPOINT");

    string modelDeploymentName = config["MODEL_DEPLOYMENT_NAME"]
        ?? throw new InvalidOperationException("Missing MODEL_DEPLOYMENT_NAME");

    string contentsafetyEndpoint = config["CSAFE_ENDPOINT"]
        ?? throw new InvalidOperationException("Missing CSAFE_ENDPOINT");

    return (projectEndpoint, modelDeploymentName, contentsafetyEndpoint);
}
