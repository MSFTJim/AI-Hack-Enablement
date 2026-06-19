# AI-Hack-Enablement

Code and examples for the Enablement team AI Hack event, demonstrating various Azure AI Foundry agent patterns and capabilities.

## Solution Overview

This solution demonstrates the progression from basic Azure Foundry agent patterns to a real-world scenario. It starts with official Microsoft Learn quickstart examples and extends them with a custom REST API and agent that uses the API as a tool—creating a more realistic, practical agent implementation than simply chatting with an LLM.

The solution is based on the [Azure Foundry Quickstart](https://learn.microsoft.com/en-us/azure/foundry/tutorials/quickstart-create-foundry-resources) tutorial, with added real-world components.

## Projects

### Real-World Implementations

#### **EnablementAgent** 
A real-world-focused Azure AI Foundry Agent specialized for the Enablement team. This agent demonstrates how to build a realistic agent scenario by integrating with a custom REST API (`EnablementFocusApi`) as a function tool. The agent answers questions about team members' focus areas and expertise by calling the API to retrieve detailed information. This is the key learning point—agents become powerful when they can interact with real backend services.

- **Type**: Foundry Agent with Custom API Tool
- **Key Features**: Function tool integration with external API, context-aware responses
- **Use Case**: Real-world agent pattern showing API-backed agents for internal team knowledge management

#### **EnablementFocusApi**
A custom ASP.NET Core REST API that serves as the backend for `EnablementAgent`. This service provides team member expertise data through a structured interface. It includes built-in HTTP logging and OpenAPI documentation.

- **Type**: ASP.NET Core Web API
- **Key Features**: HTTP logging middleware, OpenAPI integration, team expertise endpoints
- **Use Case**: Backend service for agent tools; demonstrates the practical side of agent integration

### Quickstart Examples
These projects come from the official [Azure Foundry Quickstart](https://learn.microsoft.com/en-us/azure/foundry/tutorials/quickstart-create-foundry-resources) and illustrate foundational agent patterns:

#### **QS-Agent**
A foundational example demonstrating the basics of creating an Azure AI Foundry Agent. This simple agent uses general instructions and serves as the starting point for building custom agents.

- **Type**: Foundry Agent (Basic)
- **Key Features**: Simple agent creation, configuration loading
- **Use Case**: Learning reference for Foundry agent development

#### **QS-ChatAgent**
An interactive example showcasing multi-turn conversation capabilities. This agent maintains conversation context across multiple turns, allowing for follow-up questions and coherent dialogue.

- **Type**: Foundry Agent (Conversational)
- **Key Features**: Session management, multi-turn context preservation
- **Use Case**: Building chatbots and interactive AI experiences

#### **QS-ModelChat**
A direct model inference example that bypasses the agent layer and calls Azure AI Foundry models directly. This demonstrates a simpler approach for single-turn model interactions without agent overhead.

- **Type**: Direct Model Call (No Agent)
- **Key Features**: Simple async model invocation, direct responses
- **Use Case**: Lightweight AI inference for straightforward queries

## Getting Started

### Learning Path

1. **Start with Quickstarts**: Run `QS-ModelChat` to understand basic model inference, then `QS-Agent` for agent creation, and `QS-ChatAgent` for multi-turn conversations.
2. **Build Real-World Scenario**: Review `EnablementAgent` and `EnablementFocusApi` together to see how agents integrate with real APIs as function tools.
3. **Extend**: Use these patterns as templates for your own domain-specific agents and APIs.

### Configuration

Each project requires Azure AI Foundry configuration. Set the following environment variables or use user secrets:

- `AI_RESOURCE_ENDPOINT`: Your Azure AI Foundry project endpoint
- `MODEL_DEPLOYMENT_NAME`: The name of your deployed model
- `CSAFE_ENDPOINT`: Content Safety endpoint (where applicable)
- `AGENT_NAME`: Agent identifier (for agent-based projects)

## Project Structure

```
├── EnablementAgent/          # Real-World: Agent with API tool integration
├── EnablementFocusApi/       # Real-World: REST API backend for agent
├── QS-Agent/                 # Quickstart: Basic agent creation
├── QS-ChatAgent/             # Quickstart: Multi-turn conversation
├── QS-ModelChat/             # Quickstart: Direct model inference
└── scripts/                  # Azure CLI deployment scripts
```

**Organization**: Real-world projects are designed to work together (EnablementAgent + EnablementFocusApi). Quickstart projects (QS-*) are independent learning examples.

## Resources

- [Azure AI Foundry Documentation](https://learn.microsoft.com/en-us/azure/ai-services/agents/)
- [Azure AI Projects SDK](https://github.com/Azure/azure-sdk-for-net)
