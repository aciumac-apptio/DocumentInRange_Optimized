using System;
using System.Collections.Generic;
using System.Globalization;

namespace DocumentsInRange
{
    public class DocumentsInRangeProvider
    {
        private const string yyyyMMddHHmmss = "yyyyMMddHHmmss";

        public DocumentsInRangeProvider()
        {
        }

        /// <summary>
        /// Helper function to build lowest possible date string from given partial input
        /// </summary>
        /// <param name="prefix">partial date string</param>
        /// <returns>lowest possible date string</returns>
        private static string BuildSuffixL(string prefix)
        {
            Dictionary<int, string> lengthToSuffix = new Dictionary<int, string>()
            {
                { 13, "0"},
                { 12, "00"},
                { 11, "000"},
                { 10, "0000"},
                { 9,  "00000"},
                { 8,  "000000"},
                { 6,  "01000000"},
                { 4,  "0101000000"},
                { 3,  "00101000000"},
                { 2,  "000101000000"},
                { 1,  "0000101000000"}
            };

            string suffix = "";

            switch (prefix.Length)
            {
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                case 6:
                case 4:
                case 3:
                case 2:
                case 1:
                    suffix = lengthToSuffix[prefix.Length];
                    break;
                case 7:
                    // given tenths of days
                    suffix = prefix[prefix.Length - 1] != '0' ? "0000000" : "1000000";
                    break;
                case 5:
                    //given tenths of a month
                    suffix = prefix[prefix.Length - 1] != '0' ? "001000000" : "101000000";
                    break;
                default:
                    break;
            }

            return suffix;
        }

        /// <summary>
        /// Helper function to build highest possible date string from given partial input
        /// </summary>
        /// <param name="prefix">partial date string</param>
        /// <returns>highest possible date string</returns>
        private static string BuildSuffixR(string prefix)
        {
            Dictionary<int, string> lengthToSuffix = new Dictionary<int, string>()
            {
                { 13, "0"},
                { 12, "00"},
                { 11, "000"},
                { 10, "0000"},
                { 9,  "00000"},
                { 8,  "000000"},
                { 4,  "1231000000"},
                { 3,  "91231000000"},
                { 2,  "991231000000"},
                { 1,  "9991231000000"}
            };

            string suffix = "";

            switch (prefix.Length)
            {
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                case 4:
                case 3:
                case 2:
                case 1:
                    suffix = lengthToSuffix[prefix.Length];
                    break;
                case 7:
                    char c6 = prefix[prefix.Length - 1];
                    // Month can have maximum of 28, 29, 30, 31 days
                    switch (c6)
                    {
                        case '0':
                        case '1':
                            suffix = "9000000";
                            break;
                        case '2':
                            DateTime.TryParseExact(prefix.Substring(0, 6), "yyyyMM", null, DateTimeStyles.None, out DateTime date1);
                            if (date1.Month == 2)
                            {
                                suffix = DateTime.IsLeapYear(date1.Year) ? "9000000" : "8000000"; ;
                            }
                            else
                            {
                                suffix = "9000000";
                            }

                            break;
                        case '3':
                            DateTime.TryParseExact(prefix.Substring(0, 6), "yyyyMM", null, DateTimeStyles.None, out DateTime date3);

                            suffix = DateTime.DaysInMonth(date3.Year, date3.Month) == 30 ? "0000000" : "1000000";
                            break;
                        default:
                            break;
                    }
                    break;
                case 6:
                    DateTime.TryParseExact(prefix, "yyyyMM", null, DateTimeStyles.None, out DateTime date2);
                    suffix = $"{DateTime.DaysInMonth(date2.Year, date2.Month)}000000";
                    break;
                case 5:
                    suffix = prefix[prefix.Length - 1] == '1' ? "231000000" : "930000000";
                    break;
                default:
                    break;
            }

            return suffix;
        }

        private static string BuildSuffixMax(string prefix)
        {
            Dictionary<int, string> lengthToSuffix = new Dictionary<int, string>()
            {
                { 13, "9"},
                { 12, "59"},
                { 11, "959"},
                { 10, "5959"},
                { 8,  "235959"},
                { 4,  "1231235959"},
                { 3,  "91231235959"},
                { 2,  "991231235959"},
                { 1,  "9991231235959"}
            };

            string suffix = "";

            switch (prefix.Length)
            {
                case 13:
                case 12:
                case 11:
                case 10:
                case 8:
                case 4:
                case 3:
                case 2:
                case 1:
                    suffix = lengthToSuffix[prefix.Length];
                    break;
                case 7:
                    char c6 = prefix[prefix.Length - 1];
                    switch (c6)
                    {
                        case '0':
                        case '1':
                            suffix = "9235959";
                            break;
                        case '2':
                            DateTime.TryParseExact(prefix.Substring(0, 6), "yyyyMM", null, DateTimeStyles.None, out DateTime date1);
                            if (date1.Month == 2)
                            {
                                suffix = DateTime.IsLeapYear(date1.Year) ? "9235959" : "8235959"; ;
                            }
                            else
                            {
                                suffix = "9235959";
                            }

                            break;
                        case '3':
                            DateTime.TryParseExact(prefix.Substring(0, 6), "yyyyMM", null, DateTimeStyles.None, out DateTime date3);

                            suffix = DateTime.DaysInMonth(date3.Year, date3.Month) == 30 ? "0235959" : "1235959";
                            break;
                        default:
                            break;
                    }
                    break;
                case 9:
                    suffix = prefix[prefix.Length - 1] == '2' ? "35959" : "95959";
                    break;
                case 6:
                    DateTime.TryParseExact(prefix, "yyyyMM", null, DateTimeStyles.None, out DateTime date2);
                    suffix = $"{DateTime.DaysInMonth(date2.Year, date2.Month)}235959";
                    break;
                case 5:
                    suffix = prefix[prefix.Length - 1] == '1' ? "231235959" : "930235959";
                    break;
                default:
                    break;
            }

            return suffix;
        }

        /// <summary>
        /// Gets all documents in specified date range (inclusive) and returns number or repository calls made
        /// </summary>
        /// <param name="repo">document repository</param>
        /// <param name="startDateTime">start date</param>
        /// <param name="endDateTime">end date</param>
        /// <param name="apiCalls">number of api calls executed</param>
        /// <returns>List of documents</returns>
        public List<Document> GetDocumentsInRange(IDocumentsRepository repo, string startDateTime, string endDateTime, out long apiCalls)
        {
            var documentList = new List<Document>();
            apiCalls = 0;

            NormalizeInputDates(startDateTime, endDateTime, out DateTime startDate, out DateTime endDate);
            string[] callList = PrepareApiCalls(startDate, endDate);

            foreach (var call in callList)
            {
                documentList.AddRange(repo.GetDocuments(call));
                apiCalls++;
            }

            documentList.Sort();

            return documentList;
        }

        /// <summary>
        /// Returns required calls to be made to GetDocuments API to get documents between startDateTime and endDateTime
        /// </summary>
        /// <param name="startDateTime">starting date</param>
        /// <param name="endDateTime">ending date</param>
        /// <returns>array of calls to be made to GetDocuments API</returns>
        public string[] PrepareApiCalls(DateTime startDateTime, DateTime endDateTime)
        {
            List<string> prefixesToCall = new List<string>();

            DateTime currentDt = new DateTime((startDateTime.Year / 1000 + 1) * 1000, 1, 1);
            if (currentDt < endDateTime)
            {
                // If startDateTime and endDateTime are in different milleniums
                prefixesToCall.AddRange(CollectPrefixesProto(startDateTime, currentDt.AddDays(-1)));
                while (currentDt.AddYears(1000).Year / 1000 <= endDateTime.Year / 1000)
                {
                    prefixesToCall.AddRange(CollectPrefixesProto(currentDt, currentDt.AddYears(1000).AddDays(-1)));
                    currentDt = currentDt.AddYears(1000);
                }

                prefixesToCall.AddRange(CollectPrefixesProto(currentDt, endDateTime));
            }
            else
            {
                prefixesToCall.AddRange(CollectPrefixesProto(startDateTime, endDateTime));
            }

            return prefixesToCall.ToArray();
        }

        /// <summary>
        /// Returns lowest dateTime from a given partial datetime string
        /// </summary>
        /// <param name="startDateTime">datetime string</param>
        /// <param name="marginLeft">lowest date</param>
        /// <returns>true if passed in string can be used to create a valid date</returns>
        public bool GetLeftMargin(string startDateTime, out DateTime marginLeft)
        {
            //Check that arguments are not null or empty and less than or equal to 14 characters
            if (string.IsNullOrEmpty(startDateTime) || startDateTime.Length > 14)
            {
                throw new ArgumentException("Invalid startDateTime or endDateTime");
            }

            // Normalize the input dates for partial inputs
            if (startDateTime.Length < 14)
            {
                string suffix = BuildSuffixL(startDateTime);
                startDateTime = $"{startDateTime}{suffix}";
            }

            bool result = DateTime.TryParseExact(startDateTime, yyyyMMddHHmmss, null, DateTimeStyles.None, out marginLeft);

            return result;
        }

        /// <summary>
        /// Returns highest dateTime from a given partial datetime string
        /// </summary>
        /// <param name="startDateTime">datetime string</param>
        /// <param name="marginRight">highest date</param>
        /// <returns>true if passed in string can be used to create a valid date</returns>
        public bool GetRightMargin(string startDateTime, out DateTime marginRight)
        {
            //Check that arguments are not null or empty and less than or equal to 14 characters
            if (string.IsNullOrEmpty(startDateTime) || startDateTime.Length > 14)
            {
                throw new ArgumentException("Invalid startDateTime or endDateTime");
            }

            // Normalize the input dates for partial inputs
            if (startDateTime.Length < 14)
            {
                string suffix = BuildSuffixR(startDateTime);
                startDateTime = $"{startDateTime}{suffix}";
            }

            bool result = DateTime.TryParseExact(startDateTime, yyyyMMddHHmmss, null, DateTimeStyles.None, out marginRight);

            return result;
        }

        /// <summary>
        /// Builds api calls
        /// </summary>
        /// <param name="beginDate">starting date</param>
        /// <param name="endDate">ending date</param>
        /// <returns>array of api calls to be made</returns>
        public string[] CollectPrefixesProto(DateTime beginDate, DateTime endDate)
        {
            List<string> prefixes = new List<string>();
            string begDateStr = beginDate.ToString(yyyyMMddHHmmss);
            string endDateStr = endDate.ToString(yyyyMMddHHmmss);
                        
            for (int i = 1; i < begDateStr.Length; i++)
            {
                char a = begDateStr[i - 1];
                while (a <= '9')
                {
                    string prefix = begDateStr.Substring(0, i - 1) + a;

                    if (GetLeftMargin(prefix, out DateTime marginLeft) && GetRightMargin(prefix, out DateTime marginRight))
                    {
                        // Check if calling GetDocuments api with prefix will cover dates in [marginLeft, marginRight] interval
                        if (beginDate <= marginLeft && marginRight <= endDate)
                        {
                            // Internal range is found
                            prefixes.Add(prefix);

                            //Recurse
                            if (marginLeft > beginDate)
                            {
                                prefixes.AddRange(CollectPrefixesProto(beginDate, marginLeft.AddDays(-1)));
                            }

                            if (marginRight < endDate)
                            {
                                prefixes.AddRange(CollectPrefixesProto(marginRight.AddDays(1), endDate));
                            }

                            return prefixes.ToArray();
                        }
                    }

                    a++;
                }
            }

            // If no internal range is found, iterate from beginning through each date exactly
            while (beginDate <= endDate)
            {
                prefixes.Add(beginDate.ToString("yyyyMMdd"));
                beginDate = beginDate.AddDays(1);
            }

            return prefixes.ToArray();
        }

        /// <summary>
        /// Validates that dates have proper length and start date is less than or equal to end date
        /// </summary>
        /// <param name="startDateTime">Input starting date</param>
        /// <param name="endDateTime">Input ending date</param>
        private void NormalizeInputDates(string startDateTime, string endDateTime, out DateTime startDate, out DateTime endDate)
        {
            //Check that arguments are not null or empty and less than or equal to 14 characters
            if (string.IsNullOrEmpty(startDateTime) || string.IsNullOrEmpty(endDateTime)
                || startDateTime.Length > 14 || endDateTime.Length > 14)
            {
                throw new ArgumentException("Invalid startDateTime or endDateTime");
            }

            // Normalize the input dates for partial inputs
            if (startDateTime.Length < 14)
            {
                string suffix = BuildSuffixL(startDateTime);
                startDateTime = $"{startDateTime}{suffix}";
            }

            if (endDateTime.Length < 14)
            {
                string suffix = BuildSuffixL(endDateTime);
                endDateTime = $"{endDateTime}{suffix}";
            }

            // Check that provided dates are valid dates (ex. February 29th is invalid for non-leap years)
            string format = yyyyMMddHHmmss.Substring(0, startDateTime.Length);
            if (!DateTime.TryParseExact($"{startDateTime}", format, null, DateTimeStyles.None, out startDate))
            {
                throw new ArgumentException("Invalid start date. Please specify a valid start date.");
            }

            if (!DateTime.TryParseExact(endDateTime, format, null, DateTimeStyles.None, out endDate))
            {
                throw new ArgumentException("Invalid end date. Please specify a valid end date.");
            }

            if (startDate.CompareTo(endDate) > 0)
            {
                throw new ArgumentException("Start date must be less than or equal to the end date.");
            }
        }
    }
}
