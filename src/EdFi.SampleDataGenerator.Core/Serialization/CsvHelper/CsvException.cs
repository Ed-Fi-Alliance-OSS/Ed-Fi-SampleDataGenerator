using System;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class CsvException : Exception
    {
        public CsvException()
        {
        }

        public CsvException(string message) : base(message)
        {
        }

        public CsvException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
