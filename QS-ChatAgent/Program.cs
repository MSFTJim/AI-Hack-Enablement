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

// Create project client to call Foundry API
AIProjectClient projectClient = new(
    endpoint: new Uri(projectEndpoint),
    tokenProvider: new DefaultAzureCredential());

// Create a conversation for multi-turn chat
ProjectConversation conversation = projectClient.ProjectOpenAIClient.GetProjectConversationsClient().CreateProjectConversation();

// Chat with the agent to answer questions
ProjectResponsesClient responsesClient = projectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgent(
    defaultAgent: agentName,
    defaultConversationId: conversation.Id);
ResponseResult response = responsesClient.CreateResponse("What is the size of France in square miles?");
Console.WriteLine(response.GetOutputText());

// Ask a follow-up question in the same conversation
response = responsesClient.CreateResponse("And what is the capital city?");
Console.WriteLine(response.GetOutputText());


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

