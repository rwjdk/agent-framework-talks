using Microsoft.Extensions.VectorData;

namespace RAG.Models;

public class MyVectorEntry
{
    [VectorStoreKey]
    public required Guid Id { get; set; }

    [VectorStoreData]
    public required string Question { get; set; }

    [VectorStoreData]
    public required string Answer { get; set; }

    [VectorStoreVector(1536)]
    public string Vector => $"Q: {Question} - A: {Answer}";
}