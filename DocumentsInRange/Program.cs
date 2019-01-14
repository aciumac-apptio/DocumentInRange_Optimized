using System;
using System.Globalization;

namespace DocumentsInRange
{
    class Program
    {
        private const string yyyyMMddHHmmss = "yyyyMMddHHmmss";

        static void Main(string[] args)
        {
            IDocumentsRepository repo = new DocumentsRepository();
            DocumentsInRangeProvider documentsInRangeProvider = new DocumentsInRangeProvider();

            string startDate = "19700225000000";
            string endDate = "20190430000000";

            DateTime.TryParseExact(startDate, yyyyMMddHHmmss, null, DateTimeStyles.None, out DateTime startDateTime);
            DateTime.TryParseExact(endDate, yyyyMMddHHmmss, null, DateTimeStyles.None, out DateTime endDateTime);

            if (args.Length == 2)
            {
                startDate = args[0];
                endDate = args[1];
            }            

            var docsInRange = documentsInRangeProvider.GetDocumentsInRange(repo, startDate, endDate, out long apiCallsNumber);
            var apiCalls = documentsInRangeProvider.PrepareApiCalls(startDateTime, endDateTime);
            Array.Sort(apiCalls);

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

            int j = 1;
            Console.WriteLine("\nAPI calls made:");
            foreach (var call in apiCalls)
            {
                if (j < 6 || j > apiCalls.Length - 5)
                {
                    Console.WriteLine($"\t{j}\t{call}");
                }

                if (j == 5 && apiCalls.Length > 5)
                {
                    Console.WriteLine($"\t...\t...");
                }

                j++;
            }

            Console.WriteLine("\nDone. Press any key to exit ...");
            Console.ReadKey();
        }
    }
}
