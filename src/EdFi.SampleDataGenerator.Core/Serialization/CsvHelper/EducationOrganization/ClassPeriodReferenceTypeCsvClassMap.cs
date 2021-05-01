using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class ClassPeriodReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<ClassPeriodReferenceType>
    {
        public ClassPeriodReferenceTypeCsvClassMap()
        {
            References<ClassPeriodIdentityTypeCsvClassMap>(x => x.ClassPeriodIdentity);
        }
    }
}
