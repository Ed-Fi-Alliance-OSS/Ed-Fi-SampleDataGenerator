using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IEthnicityMapping : IEqualityComparer<IEthnicityMapping>, IEquatable<IEthnicityMapping>
    {
        string Ethnicity { get; }
        string EdFiRaceType { get; }
        bool HispanicLatinoEthnicity { get; }

        RaceDescriptor GetRaceDescriptor();
    }

    public static class EthnicityMappingExtensions
    {
        public static IEthnicityMapping MappingFor(this IEthnicityMapping[] mappings, string ethnicity)
        {
            return mappings.FirstOrDefault(m => m.Ethnicity.Equals(ethnicity, StringComparison.OrdinalIgnoreCase));
        }

        public static IEthnicityMapping MappingFor(this IEthnicityMapping[] mappings, RaceDescriptor raceDescriptor, bool hispanicLatinoEthnicity)
        {
            return mappings.FirstOrDefault(m => m.GetRaceDescriptor() == raceDescriptor && m.HispanicLatinoEthnicity == hispanicLatinoEthnicity);
        }

        public static IEthnicityMapping MappingFor(this IEthnicityMapping[] mappings, string race, bool hispanicLatinoEthnicity)
        {
            return mappings.FirstOrDefault(m => m.GetRaceDescriptor().CodeValue == race && m.HispanicLatinoEthnicity == hispanicLatinoEthnicity);
        }

        public static IEthnicityMapping MappingFor(this IEthnicityMapping[] mappings, StudentDataGeneratorContext context)
        {
            return mappings.FirstOrDefault(m => m.GetRaceDescriptor() == context.StudentCharacteristics.Race && m.HispanicLatinoEthnicity == context.StudentCharacteristics.HispanicLatinoEthnicity);
        }

        public static RaceDescriptor RaceDescriptorFor(this IEthnicityMapping[] mappings, string ethnicity)
        {
            return mappings.MappingFor(ethnicity).GetRaceDescriptor();
        }

        //not all race/ethnicity types have a corresponding OldEthnicityType
        //as a workaround, an Enum type is returned here in order to permit null values
        public static OldEthnicityDescriptor OldEthnicity(this IEthnicityMapping[] mappings, bool hispanicLatinoEthnicity)
        {
            if (hispanicLatinoEthnicity)
                return OldEthnicityDescriptor.Hispanic;

            foreach (var race in mappings.Select(m => m.GetRaceDescriptor()))
            {
                if (race == RaceDescriptor.AmericanIndianAlaskaNative)
                    return OldEthnicityDescriptor.AmericanIndianOrAlaskanNative;

                if (race == RaceDescriptor.Asian || race == RaceDescriptor.NativeHawaiianPacificIslander)
                    return OldEthnicityDescriptor.AsianOrPacificIslander;

                if (race == RaceDescriptor.BlackAfricanAmerican)
                    return OldEthnicityDescriptor.BlackNotOfHispanicOrigin;

                if (race == RaceDescriptor.White)
                    return OldEthnicityDescriptor.WhiteNotOfHispanicOrigin;
            }
       
            return null;
        }
    }
}