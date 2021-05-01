using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class SessionReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<SessionReferenceType>
    {
        public SessionReferenceTypeCsvClassMap()
        {
            References<SessionIdentityTypeCsvClassMap>(x => x.SessionIdentity);
            References<SessionLookupTypeCsvClassMap>(x => x.SessionLookup);
        }
    }
}
