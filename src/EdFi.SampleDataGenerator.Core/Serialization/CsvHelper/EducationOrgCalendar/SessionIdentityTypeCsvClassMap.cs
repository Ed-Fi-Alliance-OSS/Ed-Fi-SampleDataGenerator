using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class SessionIdentityTypeCsvClassMap : CsvClassMap<SessionIdentityType>
    {
        public SessionIdentityTypeCsvClassMap()
        {
            Map(x => x.SchoolYear).ConvertEnumerationType();
            Map(x => x.SessionName);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}