using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class StateEducationAgencyIdentityTypeCsvClassMap : CsvClassMap<StateEducationAgencyIdentityType>
    {
        public StateEducationAgencyIdentityTypeCsvClassMap()
        {
            Map(x => x.StateEducationAgencyId);
        }
    }
}