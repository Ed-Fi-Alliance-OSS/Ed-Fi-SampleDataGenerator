using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Constants
{
    public static class AssessmentPerformanceLevel
    {
        public static readonly string[] PerformanceLevelMetValues =
        {
            PerformanceLevelDescriptor.Pass.GetStructuredCodeValue(),
            PerformanceLevelDescriptor.Basic.GetStructuredCodeValue(),
            PerformanceLevelDescriptor.Proficient.GetStructuredCodeValue(),
            PerformanceLevelDescriptor.Advanced.GetStructuredCodeValue(),
        };

        public static readonly string[] PerformanceLevelNotMetValues =
        {
            PerformanceLevelDescriptor.Fail.GetStructuredCodeValue(),
            PerformanceLevelDescriptor.BelowBasic.GetStructuredCodeValue(),
            PerformanceLevelDescriptor.WellBelowBasic.GetStructuredCodeValue(),
        };
    }
}
