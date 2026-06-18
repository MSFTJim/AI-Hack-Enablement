using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.AI.Projects;
using Azure.AI.Extensions.OpenAI;
using OpenAI.Responses;
using Azure.AI.Projects.Agents;

#pragma warning disable OPENAI001

Console.WriteLine("Start of Program");

// Load configuration and get values
var (projectEndpoint, modelDeploymentName, contentsafetyEndpoint, agentName) = LoadConfiguration();

Console.WriteLine($"Project Endpoint: {projectEndpoint}");
Console.WriteLine($"Model Deployment Name: {modelDeploymentName}");
Console.WriteLine($"Content Safety Endpoint: {contentsafetyEndpoint}");
Console.WriteLine($"Agent Name: {agentName}");


// Create Foundry Agent

// Create project client to call Foundry API
AIProjectClient projectClient = new(
    endpoint: new Uri(projectEndpoint),
    tokenProvider: new DefaultAzureCredential());

// Create an agent with a model and instructions
ProjectsAgentDefinition agentDefinition = new DeclarativeAgentDefinition(modelDeploymentName) // supports all Foundry direct models
{
    Instructions = "You are a helpful assistant that answers general questions",
};

ProjectsAgentVersion agent = projectClient.AgentAdministrationClient.CreateAgentVersion(
    agentName,
    options: new(agentDefinition));
Console.WriteLine($"Agent created (id: {agent.Id}, name: {agent.Name}, version: {agent.Version})");





//////// MAIN Program END ////////

// Load configuration from appsettings, user secrets, and environment variables
(string, string, string, string) LoadConfiguration()
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

    string agentName = config["AGENT_NAME"]
        ?? throw new InvalidOperationException("Missing AGENT_NAME");

    return (projectEndpoint, modelDeploymentName, contentsafetyEndpoint, agentName);
}

