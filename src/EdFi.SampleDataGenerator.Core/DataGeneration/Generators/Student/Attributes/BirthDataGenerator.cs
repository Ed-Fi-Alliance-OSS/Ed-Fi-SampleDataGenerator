using System;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class BirthDataGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField GeneratesField => StudentField.BirthData;
        public override IEntityField[] DependsOnFields => NoDependencies;

        public const double HeldBackChance = 0.02; //% chance the student has been held back.  sourced from google - recent numbers say ~1.5 - 2%
        public const int MinDayOffset = 30;
        public const int MaxDayOffset = -365;

        public BirthDataGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            context.Student.BirthData = new BirthData
            {
                BirthDate = context.SeedRecord.BirthDate
            };
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var schoolStartDate = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate;
            var birthDate = schoolStartDate.Subtract(GetBirthdayOffset());

            context.Student.BirthData = new BirthData
            {
                BirthDate = birthDate
            };
        }

        private TimeSpan GetBirthdayOffset()
        {
            var grade = Configuration.GradeProfile.GetGradeLevel();
            var baseYearOffset = grade.GetStudentAgeAtStartOfSchoolYear(); //given the student's grade level, how old are they under normal circumstances?
            var baseOffsetInDays = 365 * baseYearOffset;
            var approximateLeapYearAdjustment = baseYearOffset/4; //add back approximately 1 day every 4 years
            var randomDayOffset = RandomNumberGenerator.Generate(MaxDayOffset, MinDayOffset);
            var heldBackOffset = RandomNumberGenerator.GetValueWithProbability(HeldBackChance, 365, 0);
            
            var totalDayOffset = baseOffsetInDays + randomDayOffset + approximateLeapYearAdjustment + heldBackOffset;

            var result = new TimeSpan(totalDayOffset, 0, 0, 0);
            return result;
        }
    }
}
