using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class ClassPeriodIdentityTypeCsvClassMap : CsvClassMap<ClassPeriodIdentityType>
    {
        public ClassPeriodIdentityTypeCsvClassMap()
        {
            Map(x => x.ClassPeriodName);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}