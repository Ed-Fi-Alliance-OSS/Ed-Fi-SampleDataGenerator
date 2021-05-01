using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.EducationOrgCalendar
{
    public class EducationOrgCalendarGenerator : GlobalDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.EducationOrgCalendar;

        public EducationOrgCalendarGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, EmptyGeneratorFactory)
        {
        }

        public override void Generate(GlobalDataGeneratorContext context)
        {
            context.GlobalData.EducationOrgCalendarData = Configuration.EducationOrgCalendarData;
        }
    }
}
