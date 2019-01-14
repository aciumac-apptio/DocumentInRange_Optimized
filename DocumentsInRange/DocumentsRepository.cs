using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsInRange
{
    // Using a repository in place of actual web api
    public class DocumentsRepository : IDocumentsRepository
    {
        private const string yyyyMMddHHmmss = "yyyyMMddHHmmss";
        private List<Document> documents;

        /// <summary>
        /// Builds repository for the task. Contains documents between start and end date for the task given
        /// </summary>
        public DocumentsRepository()
        {
            documents = new List<Document>();

            // Normal 
            DateTime startDate = new DateTime(1965, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime(2030, 4, 30, 0, 0, 0);

            while (startDate.CompareTo(endDate) <= 0)
            {
                documents.Add(new Document { documentDate = startDate.ToString(yyyyMMddHHmmss) });
                startDate = startDate.AddDays(1);                
            }
        }

        /// <summary>
        /// Gets documents from the starting date provided
        /// </summary>
        /// <param name="datePrefix">date prefix paramenter</param>
        /// <returns>IEnumerable<Document> documents </returns>
        public IEnumerable<Document> GetDocuments(string datePrefix)
        {
            return documents
                .Where(x => x.documentDate.StartsWith(datePrefix))
                .OrderBy(x => x.documentDate)
                .ToArray();
        }
    }
}
