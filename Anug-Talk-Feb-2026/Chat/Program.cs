using System.ClientModel;
using System.Text;
using Azure.AI.OpenAI;
using ConsoleUtilities;
using Microsoft.Agents.AI;
using OpenAI.Chat;
using Secrets;

Console.Clear();
Utils.Gray("--- Chat Demo ---");
Console.OutputEncoding = Encoding.UTF8;

(string endpoint, string apiKey) = SecretsManager.GetAzureOpenAIApiKeyBasedCredentials();
AzureOpenAIClient client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));

ChatClientAgent agent = client
    .GetChatClient("gpt-4.1")
    .AsAIAgent( //Settings for your agent goes here
        instructions: "" //Add Personality, Rules and format (covered after this demo)
    ); 

AgentSession session = await agent.CreateSessionAsync();

await NormalLoop();
//--- Or ---
//await StreamingLoop();

return;

async Task NormalLoop()
{
    while (true)
    {
        Console.Write("> ");
        string input = Console.ReadLine() ?? "";
        if (input == "/new")
        {
            session = await agent.CreateSessionAsync();
            Console.Clear();
            continue;
        }
        AgentResponse response = await agent.RunAsync(input, session);
        Console.WriteLine(response);

        if (response.Usage != null)
        {
            Console.WriteLine();
            Utils.Gray($"Token Usage: In = {response.Usage.InputTokenCount} | Out = {response.Usage.OutputTokenCount}");
        }

        Utils.Separator();
    }
}

async Task StreamingLoop()
{
    while (true)
    {
        Console.Write("> ");
        string input = Console.ReadLine() ?? "";
        List<AgentResponseUpdate> updates = [];
        await foreach (AgentResponseUpdate update in agent.RunStreamingAsync(input, session))
        {
            updates.Add(update);
            Console.Write(update);
        }

        AgentResponse response = updates.ToAgentResponse();
        if (response.Usage != null)
        {
            Console.WriteLine();
            Utils.Gray($"Token Usage: In = {response.Usage.InputTokenCount} | Out = {response.Usage.OutputTokenCount}");
        }

        Utils.Separator();
    }
}
