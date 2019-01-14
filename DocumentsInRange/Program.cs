using System;

namespace DocumentsInRange
{
    class Program
    {
        static void Main(string[] args)
        {
            IDocumentsRepository repo = new DocumentsRepository();
            DocumentsInRangeProvider documentsInRangeProvider = new DocumentsInRangeProvider();

            string startDate = "19700225000000";
            string endDate = "20190430000000";
            if (args.Length == 2)
            {
                startDate = args[0];
                endDate = args[1];
            }            

            var docsInRange = documentsInRangeProvider.GetDocumentsInRange(repo, startDate, endDate, out long apiCallsNumber);
            Console.WriteLine($"Date range: {startDate} - {endDate}");
            Console.WriteLine($"Total # of calls made: {apiCallsNumber}. Found {docsInRange.Count} documents.");

            Console.WriteLine("\nDocuments:");
            int i = 1;
            foreach (var doc in docsInRange)
            {
                if (i < 6 || i > docsInRange.Count - 5)
                {
                    Console.WriteLine($"\t{i}\t{doc}");
                }

                if (i == 5 && docsInRange.Count > 5)
                {
                    Console.WriteLine($"\t...\t...");
                }

                i++;
            }

            Console.WriteLine("\nDone. Press any key to exit ...");
            Console.ReadKey();
        }
    }
}
