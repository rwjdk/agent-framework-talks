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

AgentResponse<MovieResult> response = await agent.RunAsync<MovieResult>(question);

MovieResult movieResult = response.Result;
int counter = 1;
Utils.Gray($"Date of knowledge: {movieResult.DateOfKnowledge.ToLongDateString()}");
foreach (Movie movie in movieResult.Movies)
{
    Console.WriteLine($"{counter}. {movie.Title} by {movie.Director} [Released {movie.YearOfRelease} with a score of {movie.ImdbScore}]");
    counter++;
}

Console.WriteLine();
Utils.Gray("response.Text = Raw JSON");
Console.WriteLine(response.Text);
    
class MovieResult
{
    [Description("Report back in format yyyy-MM-dd")]
    public required DateTime DateOfKnowledge { get; set; }
    public required List<Movie> Movies { get; set; }
}

class Movie
{
    public required string Title { get; set; }
    public required string Director { get; set; }
    public required int YearOfRelease { get; set; }
    public required decimal ImdbScore { get; set; }
}
