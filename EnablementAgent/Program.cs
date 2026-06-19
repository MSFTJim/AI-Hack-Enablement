using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.AI.Projects;
using Azure.AI.Extensions.OpenAI;
using OpenAI.Responses;
using Azure.AI.Projects.Agents;
using OpenAI.Assistants;

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
ProjectsAgentDefinition agentDefinition = new DeclarativeAgentDefinition(modelDeploymentName)
{
    Instructions = "You are a helpful assistant that answers questions about the Enablement team's focus areas and expertise. When asked about someone's expertise, call the get_focus_areas function tool with their name.",
};

ProjectsAgentVersion agent = projectClient.AgentAdministrationClient.CreateAgentVersion(
    agentName,
    options: new(agentDefinition));
Console.WriteLine($"Agent created (id: {agent.Id}, name: {agent.Name}, version: {agent.Version})");

Console.WriteLine("\nAgent successfully created!");
Console.WriteLine("To add the 'get_focus_areas' tool, use Azure Foundry portal:");
Console.WriteLine("  1. Go to your Foundry project → Tools");
Console.WriteLine("  2. Create new tool with:");
Console.WriteLine("     Name: get_focus_areas");
Console.WriteLine("     Type: Function");
Console.WriteLine("     Description: Get the focus areas and expertise for an Enablement team member");
Console.WriteLine("     Input (JSON Schema):");
Console.WriteLine("     {");
Console.WriteLine("       \"type\": \"object\",");
Console.WriteLine("       \"properties\": {");
Console.WriteLine("         \"person\": {");
Console.WriteLine("           \"type\": \"string\",");
Console.WriteLine("           \"description\": \"The name of the person (jim, reni, chris, or jc)\"");
Console.WriteLine("         }");
Console.WriteLine("       },");
Console.WriteLine("       \"required\": [\"person\"]");
Console.WriteLine("     }");
Console.WriteLine("  3. Add tool to this agent version");
Console.WriteLine($"  Agent ID: {agent.Id}");
Console.WriteLine($"  Agent Version: {agent.Version}");




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

