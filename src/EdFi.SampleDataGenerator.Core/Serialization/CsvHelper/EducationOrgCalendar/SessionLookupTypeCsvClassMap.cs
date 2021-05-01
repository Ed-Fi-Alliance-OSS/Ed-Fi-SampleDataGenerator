using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class SessionLookupTypeCsvClassMap : CsvClassMap<SessionLookupType>
    {
        public SessionLookupTypeCsvClassMap()
        {
            Map(x => x.SchoolYear).ConvertEnumerationType();
            Map(x => x.SessionName);
            Map(x => x.Term);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}