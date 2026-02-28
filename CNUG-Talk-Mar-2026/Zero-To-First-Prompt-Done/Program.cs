using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

Console.Clear();
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string endpoint = config["Endpoint"]!;
string apiKey = config["ApiKey"]!;
string model = "gpt-4.1-mini";

AzureOpenAIClient client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));

ChatClientAgent agent = client.GetChatClient(model).AsAIAgent();

//Normal Call
string question1 = "What is the capital of France?";
Console.WriteLine($"Q: {question1}");
AgentResponse response1 = await agent.RunAsync(question1);
Console.WriteLine($"A: {response1}");

Console.WriteLine();
Console.WriteLine("-----");
Console.WriteLine();

//Streaming Call
string question2 = "How to make soup?";
Console.WriteLine($"Q: {question2}");
Console.Write("A: ");
await foreach (AgentResponseUpdate update in agent.RunStreamingAsync(question2))
{
    Console.Write(update);
}

Console.WriteLine("--- Done ---");