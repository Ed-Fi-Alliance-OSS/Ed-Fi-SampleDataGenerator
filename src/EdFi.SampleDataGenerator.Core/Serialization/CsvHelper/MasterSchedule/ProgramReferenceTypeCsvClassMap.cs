using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.MasterSchedule
{
    public sealed class ProgramReferenceTypeCsvClassMap : CsvClassMap<ProgramReferenceType>
    {
        public ProgramReferenceTypeCsvClassMap()
        {
            References<ProgramIdentityTypeCsvClassMap>(x => x.ProgramIdentity);
        }
    }

    public sealed class ProgramIdentityTypeCsvClassMap : CsvClassMap<ProgramIdentityType>
    {
        public ProgramIdentityTypeCsvClassMap()
        {
            Map(x => x.ProgramName);
            Map(x => x.ProgramType);
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);
        }
    }
}
