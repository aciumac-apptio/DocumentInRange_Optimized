using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentsInRange.Test
{
    [TestClass]
    public class DocumentsInRangeProviderTests
    {
        private readonly IDocumentsRepository repo;
        private readonly DocumentsInRangeProvider documentInRangeProvider;

        public DocumentsInRangeProviderTests()
        {
            // arrange
            repo = new DocumentsRepository();
            documentInRangeProvider = new DocumentsInRangeProvider();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid input. StartDateTime is not allowed to be longer then 14 characters.")]
        public void DocumentsInRangeProvider_InvalidStartDate_ExceptionThrown()
        {
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "19700225000000111", "20190430000000", out long calls);

            // assert
            Assert.Fail("This shouldn't be shown, because the code above should have thrown an exception.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid input. EndDateTime is not allowed to be longer then 14 characters.")]
        public void DocumentsInRangeProvider_InvalidEndDate_ExceptionThrown()
        {
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "19700225000000", "20190430000000111", out long calls);

            // assert
            Assert.Fail("This shouldn't be shown, because the code above should have thrown an exception.");
        }

        [TestMethod]
        public void DocumentsInRangeProvider_PartialDatesAsInput_Success()
        {
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "197002", "20190430", out long calls);

            // assert
            Assert.AreEqual(34, calls);
            Assert.AreEqual(17986, list.Count);
            Assert.AreEqual("19700201000000", list[0].documentDate);
            Assert.AreEqual("20190430000000", list[list.Count - 1].documentDate);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_PartialDatesAsInput_SameYearDifferentMonths()
        {
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "197002", "197012", out long calls);

            // assert
            Assert.AreEqual(11, calls);
            Assert.AreEqual(304, list.Count);
            Assert.AreEqual("19700201000000", list[0].documentDate);
            Assert.AreEqual("19701201000000", list[list.Count - 1].documentDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Start date must be less than or equal to the end date.")]
        public void DocumentsInRangeProvider_StartDateGreaterThanEndDate_ExceptionThrown()
        {
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "20200430000000", "20190430000000", out long calls);

            // assert
            Assert.Fail("This shouldn't be shown, because the code above should have thrown an exception.");
        }

        [TestMethod]
        public void DocumentsInRangeProvider_GetDocumentsInRange_Gets5DocumentsInclusive()
        {
            /* Expected list:
            * 1969-02-25 00:00:00
            * 1969-02-26 00:00:00
            * 1969-02-27 00:00:00
            * 1969-02-28 00:00:00
            * 1969-03-01 00:00:00
            */
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "19690225000000", "19690301030405", out long calls);

            // assert
            Assert.AreEqual(5, calls);
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("19690225000000", list[0].documentDate);
            Assert.AreEqual("19690301000000", list[list.Count - 1].documentDate);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_GetDocumentsInRange_Gets366Documents()
        {
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "19690225000000", "19700225000000", out long calls);

            // assert
            Assert.AreEqual(21, calls);
            Assert.AreEqual(366, list.Count);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_GetDocumentsInRange_ValidNumberOfApiCalls()
        {
            // act
            documentInRangeProvider.GetDocumentsInRange(repo, "19690225000000", "19700225000000", out long calls1);
            documentInRangeProvider.GetDocumentsInRange(repo, "19690225000000", "20000225000000", out long calls2);
            documentInRangeProvider.GetDocumentsInRange(repo, "19690225000000", "30000225000000", out long calls3);
            documentInRangeProvider.GetDocumentsInRange(repo, "19690225000000", "40000225000000", out long calls4);

            // assert
            Assert.AreEqual(21, calls1);
            Assert.AreEqual(24, calls2);
            Assert.AreEqual(25, calls3);
            Assert.AreEqual(26, calls4);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_2019_01_12__2019_01_29()
        {
            /* 
             * Dates from 2019-01-12 to 2019-01-29.
             * Expected result is list having 18 documents and 9 api calls.
            */
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "20190112000000", "20190129000000", out long calls);

            // assert
            // Todo fix the code to achieve 9 api calls
            Assert.AreEqual(9, calls);
            Assert.AreEqual(18, list.Count);
            Assert.AreEqual("20190112000000", list[0].documentDate);
            Assert.AreEqual("20190129000000", list[list.Count - 1].documentDate);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_2019_01_12__2019_01_30()
        {
            /* 
             * Dates from 2019-01-12 to 2019-01-30
             * Expected result is list having 19 documents and 10 api calls.
            */
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "20190112000000", "20190130000000", out long calls);

            // assert
            // Todo fix the code to achieve 10 api calls
            Assert.AreEqual(10, calls);
            Assert.AreEqual(19, list.Count);
            Assert.AreEqual("20190112000000", list[0].documentDate);
            Assert.AreEqual("20190130000000", list[list.Count - 1].documentDate);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_2019_01_01__2019_01_31()
        {
            /* 
             * Dates from 2019-01-01 to 2019-01-31.
             * Expected result is list having 31 documents in a single api call.
            */
            // act
            var list = documentInRangeProvider.GetDocumentsInRange(repo, "20190101000000", "20190131000000", out long calls);

            // assert
            // Todo fix the code to achieve 1 api calls
            Assert.AreEqual(1, calls);
            Assert.AreEqual(31, list.Count);
            Assert.AreEqual("20190101000000", list[0].documentDate);
            Assert.AreEqual("20190131000000", list[list.Count - 1].documentDate);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_1101__1107()
        {
            // act
            DateTime begDate = new DateTime(2019, 11, 1);
            DateTime endDate = new DateTime(2019, 11, 7);

            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;

            // assert
            Assert.AreEqual(7, calls);
            Assert.AreEqual("20191101", prefixes[0]);
            Assert.AreEqual("20191107", prefixes[6]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_GetLeftMarginTest()
        {
            documentInRangeProvider.GetLeftMargin("2", out DateTime marginLeft);
            Assert.AreEqual(new DateTime(2000, 01, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("20", out marginLeft);
            Assert.AreEqual(new DateTime(2000, 01, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("201", out marginLeft);
            Assert.AreEqual(new DateTime(2010, 01, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("2019", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 01, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("20191", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 10, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("20190", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 01, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("201910", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 10, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("201911", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 11, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("2019110", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 11, 01), marginLeft);

            documentInRangeProvider.GetLeftMargin("2019111", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 11, 10), marginLeft);

            documentInRangeProvider.GetLeftMargin("2019112", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 11, 20), marginLeft);

            documentInRangeProvider.GetLeftMargin("2019113", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 11, 30), marginLeft);

            documentInRangeProvider.GetLeftMargin("20191130", out marginLeft);
            Assert.AreEqual(new DateTime(2019, 11, 30), marginLeft);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_GetRightMarginTest()
        {
            documentInRangeProvider.GetRightMargin("2", out DateTime marginRight);
            Assert.AreEqual(new DateTime(2999, 12, 31, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("20", out marginRight);
            Assert.AreEqual(new DateTime(2099, 12, 31, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("201", out marginRight);
            Assert.AreEqual(new DateTime(2019, 12, 31, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("2019", out marginRight);
            Assert.AreEqual(new DateTime(2019, 12, 31, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("20191", out marginRight);
            Assert.AreEqual(new DateTime(2019, 12, 31, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("20190", out marginRight);
            Assert.AreEqual(new DateTime(2019, 9, 30, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("201910", out marginRight);
            Assert.AreEqual(new DateTime(2019, 10, 31, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("201911", out marginRight);
            Assert.AreEqual(new DateTime(2019, 11, 30, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("2019110", out marginRight);
            Assert.AreEqual(new DateTime(2019, 11, 9, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("2019111", out marginRight);
            Assert.AreEqual(new DateTime(2019, 11, 19, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("2019112", out marginRight);
            Assert.AreEqual(new DateTime(2019, 11, 29, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("2019113", out marginRight);
            Assert.AreEqual(new DateTime(2019, 11, 30, 0, 0, 0), marginRight);

            documentInRangeProvider.GetRightMargin("20191130", out marginRight);
            Assert.AreEqual(new DateTime(2019, 11, 30, 0, 0, 0), marginRight);

        }

        //[TestMethod]
        //public void DocumentsInRangeProvider_MinAndMaxDate()
        //{
        //    documentInRangeProvider.MinAndMaxDate("20191", out DateTime marginLeft, out DateTime marginRight);

        //    Assert.AreEqual(new DateTime(2019, 10, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 12, 31), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("201910", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2019, 10, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 10, 31), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("201911", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2019, 11, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 11, 30), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("2019111", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2019, 11, 10), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 11, 19), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("2019112", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2019, 11, 20), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 11, 29), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("2019", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2019, 01, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 12, 31), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("201", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2010, 01, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2019, 12, 31), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("20", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2000, 01, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2099, 12, 31), marginRight);

        //    documentInRangeProvider.MinAndMaxDate("2", out marginLeft, out marginRight);

        //    Assert.AreEqual(new DateTime(2000, 01, 01), marginLeft);
        //    Assert.AreEqual(new DateTime(2999, 12, 31), marginRight);

        //}


        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_1101__1130()
        {
            /* 
             * Dates from 2019-01-01 to 2019-01-30.
             * Expected result is list having 30 documents in 1 api call.
            */
            // act
            DateTime begDate = new DateTime(2019, 11, 1);
            DateTime endDate = new DateTime(2019, 11, 30);


            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;

            // assert
            Assert.AreEqual(1, calls);
            Assert.AreEqual("201911", prefixes[0]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_1101__1129()
        {
            // act
            DateTime begDate = new DateTime(2019, 11, 1);
            DateTime endDate = new DateTime(2019, 11, 29);

            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;

            // assert
            Assert.AreEqual(3, calls);
            Assert.AreEqual("2019110", prefixes[0]);
            Assert.AreEqual("2019111", prefixes[1]);
            Assert.AreEqual("2019112", prefixes[2]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_1101__1128()
        {
            // act
            DateTime begDate = new DateTime(2019, 11, 1);
            DateTime endDate = new DateTime(2019, 11, 28);

            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;

            // assert
            Assert.AreEqual(11, calls);
            Assert.AreEqual("2019110", prefixes[0]);
            Assert.AreEqual("2019111", prefixes[1]);
            Assert.AreEqual("20191120", prefixes[2]);
            Assert.AreEqual("20191121", prefixes[3]);
            Assert.AreEqual("20191122", prefixes[4]);
            Assert.AreEqual("20191123", prefixes[5]);
            Assert.AreEqual("20191124", prefixes[6]);
            Assert.AreEqual("20191125", prefixes[7]);
            Assert.AreEqual("20191126", prefixes[8]);
            Assert.AreEqual("20191127", prefixes[9]);
            Assert.AreEqual("20191128", prefixes[10]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_1112__1130()
        {
            /* 
             * Dates from 2019-01-12 to 2019-01-30.
             * Expected result is list having 19 documents in 10 api call.
            */
            // act
            DateTime begDate = new DateTime(2019, 11, 12);
            DateTime endDate = new DateTime(2019, 11, 30);


            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(10, calls);
            Assert.AreEqual("20191112", prefixes[0]);
            Assert.AreEqual("20191113", prefixes[1]);
            Assert.AreEqual("20191114", prefixes[2]);
            Assert.AreEqual("20191115", prefixes[3]);
            Assert.AreEqual("20191116", prefixes[4]);
            Assert.AreEqual("20191117", prefixes[5]);
            Assert.AreEqual("20191118", prefixes[6]);
            Assert.AreEqual("20191119", prefixes[7]);

            Assert.AreEqual("2019112", prefixes[8]);
            Assert.AreEqual("2019113", prefixes[9]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_0202__0430()
        {
            /* 
             * Dates from 2019-02-02 to 2019-04-30.
             * Expected result is list having 88 documents in 12 api call.
            */
            // act
            DateTime begDate = new DateTime(2019, 2, 2);
            DateTime endDate = new DateTime(2019, 4, 30);


            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(12, calls);
            Assert.AreEqual("20190202", prefixes[0]);
            Assert.AreEqual("20190203", prefixes[1]);
            Assert.AreEqual("20190204", prefixes[2]);
            Assert.AreEqual("20190205", prefixes[3]);
            Assert.AreEqual("20190206", prefixes[4]);
            Assert.AreEqual("20190207", prefixes[5]);
            Assert.AreEqual("20190208", prefixes[6]);
            Assert.AreEqual("20190209", prefixes[7]);
            Assert.AreEqual("2019021", prefixes[8]);
            Assert.AreEqual("2019022", prefixes[9]);
            Assert.AreEqual("201903", prefixes[10]);
            Assert.AreEqual("201904", prefixes[11]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_0202__0202_FiveYears()
        {
            /* 
             * Dates from 2019-02-02 to 2019-04-30.
             * Expected result is list having 88 documents in 12 api call.
            */
            // act
            DateTime begDate = new DateTime(2014, 1, 1);
            DateTime endDate = new DateTime(2019, 12, 31);


            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(6, calls);
            Assert.AreEqual("2014", prefixes[0]);
            Assert.AreEqual("2015", prefixes[1]);
            Assert.AreEqual("2016", prefixes[2]);
            Assert.AreEqual("2017", prefixes[3]);
            Assert.AreEqual("2018", prefixes[4]);
            Assert.AreEqual("2019", prefixes[5]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_PrepareApiCalls_1970_01_05__2005_12_25()
        {
            // act
            DateTime begDate = new DateTime(1970, 1, 1);
            DateTime endDate = new DateTime(2005, 12, 25);


            string[] prefixes = documentInRangeProvider.PrepareApiCalls(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(19, calls);
            Assert.AreEqual("197", prefixes[0]);
            Assert.AreEqual("198", prefixes[1]);
            Assert.AreEqual("199", prefixes[2]);
            Assert.AreEqual("2000", prefixes[3]);
            Assert.AreEqual("2001", prefixes[4]);
            Assert.AreEqual("2002", prefixes[5]);
            Assert.AreEqual("2003", prefixes[6]);
            Assert.AreEqual("2004", prefixes[7]);
            Assert.AreEqual("20050", prefixes[8]);
            Assert.AreEqual("200510", prefixes[9]);
            Assert.AreEqual("200511", prefixes[10]);
            Assert.AreEqual("2005120", prefixes[11]);
            Assert.AreEqual("2005121", prefixes[12]);
            Assert.AreEqual("20051220", prefixes[13]);
            Assert.AreEqual("20051221", prefixes[14]);
            Assert.AreEqual("20051222", prefixes[15]);
            Assert.AreEqual("20051223", prefixes[16]);
            Assert.AreEqual("20051224", prefixes[17]);
            Assert.AreEqual("20051225", prefixes[18]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_2000_01_01__2019_12_31_FiveYears()
        {
            /* 
             * Dates from 2019-02-02 to 2019-04-30.
             * Expected result is list having 88 documents in 12 api call.
            */
            // act
            DateTime begDate = new DateTime(2000, 1, 1);
            DateTime endDate = new DateTime(2019, 12, 31);


            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(2, calls);
            //Assert.AreEqual("2014", prefixes[0]);
            //Assert.AreEqual("2015", prefixes[1]);
            //Assert.AreEqual("2016", prefixes[2]);
            //Assert.AreEqual("2017", prefixes[3]);
            //Assert.AreEqual("2018", prefixes[4]);
            //Assert.AreEqual("2019", prefixes[5]);
        }


        [TestMethod]
        public void DocumentsInRangeProvider_Prefixes_1970_01_01__1995_12_31_FiveYears()
        {
            /* 
             * Dates from 2019-02-02 to 2019-04-30.
             * Expected result is list having 88 documents in 12 api call.
            */
            // act
            DateTime begDate = new DateTime(1970, 1, 1);
            DateTime endDate = new DateTime(1995, 12, 25);


            string[] prefixes = documentInRangeProvider.CollectPrefixesProto(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(18, calls);
            //Assert.AreEqual("2014", prefixes[0]);
            //Assert.AreEqual("2015", prefixes[1]);
            //Assert.AreEqual("2016", prefixes[2]);
            //Assert.AreEqual("2017", prefixes[3]);
            //Assert.AreEqual("2018", prefixes[4]);
            //Assert.AreEqual("2019", prefixes[5]);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_PrepareApiCalls_SameMillenium()
        {
            DateTime begDate = new DateTime(1970, 1, 1);
            DateTime endDate = new DateTime(1995, 12, 25);


            string[] prefixes = documentInRangeProvider.PrepareApiCalls(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(18, calls);
        }

        [TestMethod]
        public void DocumentsInRangeProvider_PrepareApiCalls_TwoMillenium()
        {
            DateTime begDate = new DateTime(1970, 1, 1);
            DateTime endDate = new DateTime(2005, 12, 25);


            string[] prefixes = documentInRangeProvider.PrepareApiCalls(begDate, endDate);
            int calls = prefixes.Length;
            Array.Sort(prefixes);

            // assert
            Assert.AreEqual(19, calls);
        }

    }
}
