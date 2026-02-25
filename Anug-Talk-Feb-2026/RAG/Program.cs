using Azure.AI.OpenAI;
using ConsoleUtilities;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.SqliteVec;
using OpenAI.Chat;
using RAG;
using RAG.Models;
using Secrets;
using System.ClientModel;
using System.Text;
using RAG.Services;
using RAG.Tools;

Console.Clear();
Utils.Gray("--- RAG ---");
Console.OutputEncoding = Encoding.UTF8;

(string endpoint, string apiKey) = SecretsManager.GetAzureOpenAIApiKeyBasedCredentials();
AzureOpenAIClient client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));

string connectionString = $"Data Source={Path.GetTempPath()}\\data.db";
VectorStore vectorStore = new SqliteVectorStore(connectionString, new SqliteVectorStoreOptions
{
    EmbeddingGenerator = client.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator()
});

//1. Clean
List<MyDataEntry> data = MyDataService.GetData();

//2. Ingest
VectorStoreCollection<Guid, MyVectorEntry> collection = await MyVectorStoreService.PrepareVectorStoreCollection(vectorStore);
await MyVectorStoreService.IngestData(collection, data);

//3. Create and that we can use to Search (Tool) and augment LLM Input
ChatClientAgent agent = client.GetChatClient("gpt-4.1-mini").AsAIAgent(
    instructions: "You are a Internal Knowledge-base Agent. Always use tool 'search_internal_kb' to get your data",
    tools: [AIFunctionFactory.Create(new SearchTool(collection).Search, "search_internal_kb")]
);

AgentSession session = await agent.CreateSessionAsync();

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

    Console.WriteLine();
    Utils.Gray($"Token Usage: In = {response.Usage!.InputTokenCount} | Out = {response.Usage.OutputTokenCount}");
    Utils.Separator();
}