using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class ClassPeriodCsvClassMap : CsvClassMap<ClassPeriod>
    {
        public ClassPeriodCsvClassMap()
        {
            Map(x => x.ClassPeriodName);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
            ExtensionMappings();
        }
    }
}