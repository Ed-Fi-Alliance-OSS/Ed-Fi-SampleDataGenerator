using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationServiceCenterIdentityTypeCsvClassMap : CsvClassMap<EducationServiceCenterIdentityType>
    {
        public EducationServiceCenterIdentityTypeCsvClassMap()
        {
            Map(x => x.EducationServiceCenterId);
        }
    }
}