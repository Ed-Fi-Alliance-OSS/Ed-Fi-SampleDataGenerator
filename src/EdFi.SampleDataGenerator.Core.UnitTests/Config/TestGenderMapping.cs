using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestGenderMapping : IGenderMapping
    {
        public string Gender { get; set; }
        public string EdFiGender { get; set; }

        public SexDescriptor SexDescriptor { get; set; }

        public SexDescriptor GetSexDescriptor()
        {
            return SexDescriptor;
        }

        public static IGenderMapping[] Defaults = 
        {
            new TestGenderMapping { EdFiGender = SexDescriptor.Male.CodeValue, Gender = "Male", SexDescriptor = SexDescriptor.Male },
            new TestGenderMapping { EdFiGender = SexDescriptor.Female.CodeValue, Gender = "Female", SexDescriptor = SexDescriptor.Female }
        };
    }
}
