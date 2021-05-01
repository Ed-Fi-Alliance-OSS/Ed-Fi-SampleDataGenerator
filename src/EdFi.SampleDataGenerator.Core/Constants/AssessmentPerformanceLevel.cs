using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Constants
{
    public static class AssessmentPerformanceLevel
    {
        public static readonly string[] PerformanceLevelMetValues =
        {
            PerformanceBaseConversionDescriptor.Pass.CodeValue,
            PerformanceBaseConversionDescriptor.Basic.CodeValue,
            PerformanceBaseConversionDescriptor.Proficient.CodeValue,
            PerformanceBaseConversionDescriptor.Advanced.CodeValue,
        };

        public static readonly string[] PerformanceLevelNotMetValues =
        {
            PerformanceBaseConversionDescriptor.Fail.CodeValue,
            PerformanceBaseConversionDescriptor.BelowBasic.CodeValue,
            PerformanceBaseConversionDescriptor.WellBelowBasic.CodeValue,
        };
    }
}
