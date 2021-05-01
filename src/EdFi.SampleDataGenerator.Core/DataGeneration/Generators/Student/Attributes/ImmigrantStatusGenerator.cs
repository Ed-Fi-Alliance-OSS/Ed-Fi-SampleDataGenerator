using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class ImmigrantStatusGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField[] DependsOnFields => new IEntityField[] { StudentField.Race, StudentField.BirthData };
        public override IEntityField GeneratesField => StudentField.ImmigrantStatus;

        public ImmigrantStatusGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var raceMapping = Configuration.GlobalConfig.EthnicityMappings.MappingFor(context);
            var option = Configuration
                .StudentProfile
                .ImmigrantPopulationProfile?
                .CountriesOfOrigin
                .Where(c => Configuration.GlobalConfig.EthnicityMappings.MappingFor(c.Race) == raceMapping)
                .GetRandomItemWithDistribution(x => x.Frequency, RandomNumberGenerator);

            if (option != null)
            {
                context.StudentCharacteristics.IsImmigrant = true;
                context.Student.BirthData.BirthCountry = option.Name.ToDescriptorFromCodeValue<CountryDescriptor>().GetStructuredCodeValue();
                LogStat(option.Name);
            }
        }
    }
}