using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestEthnicityMapping : IEthnicityMapping
    {
        public string Ethnicity { get; set; }
        public string EdFiRaceType { get; set; }
        public bool HispanicLatinoEthnicity { get; set; }

        public RaceDescriptor RaceDescriptor { get; set; }

        public RaceDescriptor GetRaceDescriptor()
        {
            return RaceDescriptor;
        }

        public bool Equals(IEthnicityMapping lhs, IEthnicityMapping rhs)
        {
            return lhs != null
                   && rhs != null
                   && lhs.Equals(rhs);
        }

        public bool Equals(IEthnicityMapping other)
        {
            return other != null
                   && Ethnicity.Equals(other.Ethnicity)
                   && EdFiRaceType.Equals(other.EdFiRaceType)
                   && HispanicLatinoEthnicity == other.HispanicLatinoEthnicity;
        }

        public int GetHashCode(IEthnicityMapping obj)
        {
            return $"{obj.Ethnicity}{obj.EdFiRaceType}{obj.HispanicLatinoEthnicity}".GetHashCode();
        }

        public static IEthnicityMapping[] Defaults = 
        {
            new TestEthnicityMapping
            {
                Ethnicity = "AmericanIndianAlaskanNative",
                EdFiRaceType = RaceDescriptor.AmericanIndianAlaskaNative.CodeValue,
                RaceDescriptor = RaceDescriptor.AmericanIndianAlaskaNative
            },
            new TestEthnicityMapping
            {
                Ethnicity = "Asian",
                EdFiRaceType = RaceDescriptor.Asian.CodeValue,
                RaceDescriptor = RaceDescriptor.Asian
            },
            new TestEthnicityMapping
            {
                Ethnicity = "Black",
                EdFiRaceType = RaceDescriptor.BlackAfricanAmerican.CodeValue,
                RaceDescriptor = RaceDescriptor.BlackAfricanAmerican
            },
            new TestEthnicityMapping
            {
                Ethnicity = "Hispanic",
                EdFiRaceType = RaceDescriptor.White.CodeValue,
                HispanicLatinoEthnicity = true,
                RaceDescriptor = RaceDescriptor.White
            },
            new TestEthnicityMapping
            {
                Ethnicity = "NativeHawaiinPacificIslander",
                EdFiRaceType = RaceDescriptor.NativeHawaiianPacificIslander.CodeValue,
                RaceDescriptor = RaceDescriptor.NativeHawaiianPacificIslander
            },
            new TestEthnicityMapping
            {
                Ethnicity = "White",
                EdFiRaceType = RaceDescriptor.White.CodeValue,
                RaceDescriptor = RaceDescriptor.White
            }
        };
    }
}