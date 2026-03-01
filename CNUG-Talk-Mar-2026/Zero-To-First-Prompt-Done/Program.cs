using System.ClientModel;
using System.Text;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
Console.Clear();
Console.OutputEncoding = Encoding.UTF8;

//Step 1: Make an ai.azure.com resource and Get the Endpoint and API Key (Remove the '/api/projects/<project_name>' of the endpoint)
//Step 2: Deploy a model (gpt-4.1-mini)
//Step 3: Setup Secret and Add two NuGet packages (Azure.AI.OpenAI + Microsoft.Agents.AI.OpenAI)
//[Note that all AI Packages are pre-release]
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string endpoint = config["Endpoint"]!;
string apiKey = config["ApiKey"]!;
string model = "gpt-4.1-mini";

//Step 4: Create an AzureOpenAI Client
//Step 5: On the client create a ChatClient (defining the model to use) and convert it to a Microsoft Agent Framework 'ChatClientAgent' (or the more generic AIAgent)
//Step 6: Ask the Agent a question
//Step 7: Try a streaming answer

#region Finished code
{
    return;
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
}
#endregion