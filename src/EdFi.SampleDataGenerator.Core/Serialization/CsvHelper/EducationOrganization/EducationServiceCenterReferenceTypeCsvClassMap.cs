using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationServiceCenterReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<EducationServiceCenterReferenceType>
    {
        public EducationServiceCenterReferenceTypeCsvClassMap()
        {
            References<EducationServiceCenterIdentityTypeCsvClassMap>(x => x.EducationServiceCenterIdentity);
        }
    }
}