using Azure.AI.OpenAI;
using ConsoleUtilities;
using Microsoft.Agents.AI;
using OpenAI.Chat;
using Secrets;
using System.ClientModel;
using System.ComponentModel;
using System.Text;

Console.Clear();
Utils.Gray("--- Structured Output ---");
Console.OutputEncoding = Encoding.UTF8;

(string endpoint, string apiKey) = SecretsManager.GetAzureOpenAIApiKeyBasedCredentials();
AzureOpenAIClient client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));

ChatClientAgent agent = client.GetChatClient("gpt-4.1-mini").AsAIAgent(instructions: "You are a Movie Expert");

string question = "List the top 5 best movies according to IMDB";
AgentResponse unstructuredResponse = await agent.RunAsync(question); //Without structure
string unstructuredText = unstructuredResponse.Text;


AgentResponse<List<Movie>> structuredResponse = await agent.RunAsync<List<Movie>>(question);
string json = structuredResponse.Text;
List<Movie> movies = structuredResponse.Result;


int counter = 1;
foreach (Movie movie in movies)
{
    Console.WriteLine($"{counter}. {movie.Title} by {movie.Director} [Released {movie.YearOfRelease} with a score of {movie.ImdbScore}]");
    counter++;
}

Console.WriteLine();
Utils.Gray("response.Text = Raw JSON");
Console.WriteLine(structuredResponse.Text);
    
class Movie
{
    public required string Title { get; set; }
    public required string Director { get; set; }
    public required int YearOfRelease { get; set; }
    public required decimal ImdbScore { get; set; }
}
