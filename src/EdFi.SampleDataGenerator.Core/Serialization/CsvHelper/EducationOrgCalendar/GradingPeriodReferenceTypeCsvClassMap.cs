using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class GradingPeriodReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<GradingPeriodReferenceType>
    {
        public GradingPeriodReferenceTypeCsvClassMap()
        {
            References<GradingPeriodIdentityTypeCsvClassMap>(x => x.GradingPeriodIdentity);
        }
    }
}