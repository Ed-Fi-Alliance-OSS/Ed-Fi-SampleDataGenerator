using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed partial class GradingPeriodCsvClassMap : ComplexObjectTypeCsvClassMap<GradingPeriod>
    {
        public GradingPeriodCsvClassMap()
        {
            Map(x => x.BeginDate);
            Map(x => x.EndDate);
            Map(x => x.TotalInstructionalDays);
            Map(x => x.PeriodSequence);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
            Map(x => x.GradingPeriod1);
            Map(x => x.SchoolYear).ConvertEnumerationType();
            ExtensionMappings();
        }
    }
}