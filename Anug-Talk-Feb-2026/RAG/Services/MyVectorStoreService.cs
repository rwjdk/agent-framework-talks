using Microsoft.Extensions.VectorData;
using RAG.Models;

namespace RAG.Services;

public static class MyVectorStoreService
{
    public static async Task<VectorStoreCollection<Guid, MyVectorEntry>> PrepareVectorStoreCollection(VectorStore vectorStore)
    {
        VectorStoreCollection<Guid, MyVectorEntry> vectorStoreCollection = vectorStore.
            GetCollection<Guid, MyVectorEntry>("knowledge_base");

        await vectorStoreCollection.EnsureCollectionExistsAsync();
        return vectorStoreCollection;
    }

    public static async Task IngestData(VectorStoreCollection<Guid, MyVectorEntry> collection, List<MyDataEntry> data)
    {
        //Delete old table
        await collection.EnsureCollectionDeletedAsync();

        //Create anew
        await collection.EnsureCollectionExistsAsync();
        Console.Clear();
        int counter = 0;
        foreach (MyDataEntry entry in data)
        {
            counter++;
            Console.Write($"\rEmbedding Data: {counter}/{data.Count}");
            await collection.UpsertAsync(new MyVectorEntry
            {
                Id = Guid.NewGuid(),
                Question = entry.Question,
                Answer = entry.Answer,
            });
        }

        Console.WriteLine();
        Console.WriteLine("\rEmbedding complete...");
    }

}