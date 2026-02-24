using System.Text;
using ConsoleUtilities;
using Microsoft.Extensions.VectorData;
using RAG.Models;

namespace RAG.Tools;

public class SearchTool(VectorStoreCollection<Guid, MyVectorEntry> vectorStoreCollection)
{
    public async Task<string> Search(string input)
    {
        StringBuilder mostSimilarKnowledge = new StringBuilder();
        await foreach (VectorSearchResult<MyVectorEntry> searchResult in vectorStoreCollection.SearchAsync(input, 3))
        {
            string searchResultAsQAndA = $"Q: {searchResult.Record.Question} - A: {searchResult.Record.Answer}";
            Utils.Gray($"- Search result [Score: {searchResult.Score}] {searchResultAsQAndA}");
            mostSimilarKnowledge.AppendLine(searchResultAsQAndA);
        }

        Console.WriteLine();

        return mostSimilarKnowledge.ToString();
    }
}