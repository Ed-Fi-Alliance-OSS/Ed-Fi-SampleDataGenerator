using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed partial class SessionCsvClassMap : ComplexObjectTypeCsvClassMap<Session>
    {
        public SessionCsvClassMap()
        {
            Map(x => x.SessionName);
            Map(x => x.SchoolYear).ConvertEnumerationType();
            Map(x => x.BeginDate);
            Map(x => x.EndDate);
            Map(x => x.TotalInstructionalDays);
            Map(x => x.Term);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
            References<GradingPeriodReferenceTypeCsvClassMap>(x => x.GradingPeriodReference);
            ExtensionMappings();
        }
    }
}