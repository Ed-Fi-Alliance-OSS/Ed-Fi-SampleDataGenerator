using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class RaceGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField GeneratesField => StudentField.Race;
        public override IEntityField[] DependsOnFields => NoDependencies;

        public RaceGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            context.StudentCharacteristics.Race = context.SeedRecord.Race;

            context.StudentCharacteristics.HispanicLatinoEthnicity = context.SeedRecord.HispanicLatinoEthnicity;
            MapOldEthnicity(context);
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var option = Configuration.StudentProfile.RaceConfiguration.GetRandomItem(RandomNumberGenerator);
            var mapping = Configuration.GlobalConfig.EthnicityMappings.MappingFor(option.Value);

            context.StudentCharacteristics.Race = mapping.GetRaceDescriptor();

            context.StudentCharacteristics.HispanicLatinoEthnicity = mapping.HispanicLatinoEthnicity;

            MapOldEthnicity(context);

            LogStat(option);
        }

        private static void MapOldEthnicity(StudentDataGeneratorContext context)
        {
            var raceType = context.StudentCharacteristics.Race;

            if (context.StudentCharacteristics.HispanicLatinoEthnicity)
                context.StudentCharacteristics.OldEthnicity = OldEthnicityDescriptor.Hispanic;
            else
            {
                if (raceType == RaceDescriptor.AmericanIndianAlaskaNative)
                    context.StudentCharacteristics.OldEthnicity = OldEthnicityDescriptor.AmericanIndianOrAlaskanNative;

                if (raceType == RaceDescriptor.Asian || raceType == RaceDescriptor.NativeHawaiianPacificIslander)
                    context.StudentCharacteristics.OldEthnicity = OldEthnicityDescriptor.AsianOrPacificIslander;

                if (raceType == RaceDescriptor.BlackAfricanAmerican)
                    context.StudentCharacteristics.OldEthnicity = OldEthnicityDescriptor.BlackNotOfHispanicOrigin;

                if (raceType == RaceDescriptor.White)
                    context.StudentCharacteristics.OldEthnicity = OldEthnicityDescriptor.WhiteNotOfHispanicOrigin;
            }
        }
    }
}