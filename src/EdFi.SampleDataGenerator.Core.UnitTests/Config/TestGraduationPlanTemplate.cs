using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestGraduationPlanTemplate : IGraduationPlanTemplate
    {
        public string Name { get; set; }
        public string GraduationPlanType { get; set; }
        public int TotalCreditsRequired { get; set; }

        public GraduationPlanTypeDescriptor GetGraduationPlanTypeDescriptor()
        {
            return GraduationPlanType.ToDescriptorFromCodeValue<GraduationPlanTypeDescriptor>();
        }

        public static TestGraduationPlanTemplate Default = new TestGraduationPlanTemplate
        {
            Name = "Standard",
            GraduationPlanType = GraduationPlanTypeDescriptor.Standard.CodeValue,
            TotalCreditsRequired = 32
        };
    }
}
