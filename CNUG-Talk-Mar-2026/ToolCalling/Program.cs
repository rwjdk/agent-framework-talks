using Azure.AI.OpenAI;
using ConsoleUtilities;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI.Chat;
using Secrets;
using System.ClientModel;
using System.Text;
using AgentFrameworkToolkit.Tools;
using ToolCalling.Tools;

Console.Clear();
Utils.Gray("--- Tool Calling Demo ---");
Console.OutputEncoding = Encoding.UTF8;

Secrets.Secrets secrets = SecretsManager.GetSecrets();
AzureOpenAIClient client = new AzureOpenAIClient(new Uri(secrets.AzureOpenAiEndpoint), new ApiKeyCredential(secrets.AzureOpenAiKey));

PersonTools personInstance = new PersonTools();
AITool getPersonsTool = AIFunctionFactory.Create(personInstance.GetPersons, "get_persons", "Get all persons we know");
AITool getPersonTool = AIFunctionFactory.Create(personInstance.GetPerson, "get_person", "Get a specific person");


#region MCP and other tools (we will see that a bit later)

await using McpClient mcpClient = await McpClient.CreateAsync(new HttpClientTransport(new HttpClientTransportOptions
{
    Endpoint = new Uri("https://api.githubcopilot.com/mcp/"),
    TransportMode = HttpTransportMode.StreamableHttp,
    AdditionalHeaders = new Dictionary<string, string>
    {
        {"Authorization", $"Bearer {secrets.GitHubPatToken}"}
    }
}));

IList<McpClientTool> mcpTools = await mcpClient.ListToolsAsync();

AIToolsFactory toolsFactory = new AIToolsFactory();
#endregion

AITool changeColorTool = AIFunctionFactory.Create(ChangeConsoleColor, "change_color", "Change the color of the interface");


AIAgent agent = client
    .GetChatClient("gpt-4.1-mini")
    .AsAIAgent(
        tools: //List of Tools the agent should be available to use
        [
            getPersonsTool,
            getPersonTool,
            changeColorTool,
            ..mcpTools
        ]
    ).AsBuilder().Use(ToolCallingMiddleware).Build()
    
    
    
    
    
    //.AsBuilder().Use(ToolCallingMiddleware).Build()
    ;

AgentSession session = await agent.CreateSessionAsync();

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine() ?? "";
    if (input == "/new")
    {
        session = await agent.CreateSessionAsync();
        Console.Clear();
        Console.ResetColor();
        continue;
    }
    AgentResponse response = await agent.RunAsync(input, session);
    Console.WriteLine(response);

    Console.WriteLine();
    Utils.Gray($"Token Usage: In = {response.Usage!.InputTokenCount} | Out = {response.Usage.OutputTokenCount}");
    Utils.Separator();
}

static async ValueTask<object?> ToolCallingMiddleware(
    AIAgent agent, 
    FunctionInvocationContext context,
    Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next, 
    CancellationToken cancellationToken)
{
    StringBuilder toolDetails = new();
    toolDetails.Append($"- Tool Call: '{context.Function.Name}'");
    if (context.Arguments.Count > 0)
    {
        toolDetails.Append($" (Args: {string.Join(",", context.Arguments.Select(x => $"[{x.Key} = {x.Value}]"))}");
    }
    Utils.Yellow(toolDetails.ToString());
    
    //Tip: You can on the fly manipulate and cancel tool calls here

    return await next.Invoke(context, cancellationToken);
}

//Action Tools
static void ChangeConsoleColor(ConsoleColor color)
{
    Console.ForegroundColor = color;
}


