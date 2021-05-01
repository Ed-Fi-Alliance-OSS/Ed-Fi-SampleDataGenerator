using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IGenderMapping
    {
        string Gender { get; }
        string EdFiGender { get; }

        SexDescriptor GetSexDescriptor();
    }

    public static class GenderMappingExtensions
    {
        public static IGenderMapping MappingFor(this IGenderMapping[] mappings, string gender)
        {
            return mappings.FirstOrDefault(m => m.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
        }

        public static SexDescriptor SexDescriptorFor(this IGenderMapping[] mappings, string gender)
        {
            return mappings.MappingFor(gender).GetSexDescriptor();
        }
    }
}
