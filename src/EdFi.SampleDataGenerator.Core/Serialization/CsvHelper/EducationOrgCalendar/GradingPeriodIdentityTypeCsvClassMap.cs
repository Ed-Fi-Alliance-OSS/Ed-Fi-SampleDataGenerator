using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public class GradingPeriodIdentityTypeCsvClassMap : CsvClassMap<GradingPeriodIdentityType>
    {
        public GradingPeriodIdentityTypeCsvClassMap()
        {
            Map(x => x.GradingPeriod);
            Map(x => x.PeriodSequence);
            Map(x => x.SchoolYear).ConvertEnumerationType();
        }
    }
}
