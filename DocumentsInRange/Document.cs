using System;

namespace DocumentsInRange
{
    public class Document : IComparable<Document>
    {
        public string documentDate { get; set; }

        public int CompareTo(Document other)
        {
            return string.Compare(documentDate, other.documentDate);
        }

        public override string ToString()
        {
            return "DocumentDate: " + documentDate.ToString();
        }
    }
}
