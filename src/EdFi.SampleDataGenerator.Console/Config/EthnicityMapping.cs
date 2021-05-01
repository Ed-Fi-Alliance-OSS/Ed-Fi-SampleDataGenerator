using System;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class EthnicityMapping : IEthnicityMapping
    {
        [XmlAttribute]
        public string Ethnicity { get; set; }

        [XmlAttribute]
        public string EdFiRaceType { get; set; }

        [XmlAttribute]
        public bool HispanicLatinoEthnicity { get; set; }

        public RaceDescriptor GetRaceDescriptor()
        {
            RaceDescriptor result;
            if (DescriptorHelpers.TryParseFromCodeValue(EdFiRaceType, true, out result))
            {
                return result;
            }

            throw new Exception($"Can't map '{Ethnicity}' to an Ed-Fi RaceDescriptor");
        }

        public bool Equals(IEthnicityMapping lhs, IEthnicityMapping rhs)
        {
            return lhs != null
                   && rhs != null
                   && lhs.Equals(rhs);
        }

        public int GetHashCode(IEthnicityMapping obj)
        {
            return $"{obj.Ethnicity}{obj.EdFiRaceType}{obj.HispanicLatinoEthnicity}".GetHashCode();
        }

        public bool Equals(IEthnicityMapping other)
        {
            return other != null
                   && Ethnicity.Equals(other.Ethnicity)
                   && EdFiRaceType.Equals(other.EdFiRaceType)
                   && HispanicLatinoEthnicity == other.HispanicLatinoEthnicity;
        }
    }
}
