using System;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class GenderMapping : IGenderMapping
    {
        public string Gender { get; set; }
        public string EdFiGender { get; set; }

        public SexDescriptor GetSexDescriptor()
        {
            SexDescriptor result;
            if (DescriptorHelpers.TryParseFromCodeValue(EdFiGender, true, out result))
            {
                return result;
            }

            throw new Exception($"Can't map '{Gender}' to an Ed-Fi SexType");
        }
    }
}
