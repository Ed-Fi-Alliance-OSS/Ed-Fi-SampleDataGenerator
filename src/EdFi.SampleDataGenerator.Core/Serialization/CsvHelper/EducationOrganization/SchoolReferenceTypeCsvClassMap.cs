using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class SchoolReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<SchoolReferenceType>
    {
        public SchoolReferenceTypeCsvClassMap()
        {
            References<SchoolIdentityTypeCsvClassMap>(x => x.SchoolIdentity);
        }
    }
}