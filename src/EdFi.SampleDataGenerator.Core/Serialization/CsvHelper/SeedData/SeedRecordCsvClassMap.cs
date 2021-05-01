using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Config.SeedData;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.SeedData
{
    public sealed class SeedRecordCsvClassMap : CsvClassMap<SeedRecord>
    {
        public SeedRecordCsvClassMap()
        {
            Map(x => x.FirstName);
            Map(x => x.MiddleName);
            Map(x => x.LastName);
            Map(x => x.Gender).Name("Gender").ConvertDescriptorType();
            Map(x => x.Race).Name("Race").ConvertDescriptorType();
            Map(x => x.HispanicLatinoEthnicity);
            Map(x => x.BirthDate);
            Map(x => x.GradeLevel).Name("GradeLevel").ConvertDescriptorType();
            Map(x => x.PerformanceIndex).TypeConverterOption.Format("0.00000"); //ensure performance index gets enough precision on output
            Map(x => x.SchoolId);
        }
    }
}
