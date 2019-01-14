using System.Collections.Generic;

namespace DocumentsInRange
{
    public interface IDocumentsRepository
    {
        IEnumerable<Document> GetDocuments(string datePrefix);

    }
}