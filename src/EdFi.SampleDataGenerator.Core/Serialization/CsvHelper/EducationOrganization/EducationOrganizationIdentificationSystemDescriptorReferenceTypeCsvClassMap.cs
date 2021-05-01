using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationIdentificationSystemDescriptorReferenceTypeCsvClassMap : CsvClassMap<EducationOrganizationIdentificationSystemDescriptorReferenceType>
    {
        public EducationOrganizationIdentificationSystemDescriptorReferenceTypeCsvClassMap()
        {
            Map(x => x.CodeValue);
            Map(x => x.Namespace);
        }
    }
}