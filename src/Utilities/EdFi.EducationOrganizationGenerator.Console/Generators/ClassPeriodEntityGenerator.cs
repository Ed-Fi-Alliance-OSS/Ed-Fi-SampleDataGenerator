using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class ClassPeriodEntityGenerator : EducationOrganizationEntityGenerator
    {
        public ClassPeriodEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.ClassPeriod;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.School, };

        protected override void GenerateCore(EducationOrganizationData context)
        {
            foreach (var school in context.Schools)
            {
                for (var period = 1; period <= 8; ++period)
                {
                    var classPeriod = new ClassPeriod
                    {
                        id = $"CP_{school.SchoolId}_{period:D2}",
                        ClassPeriodName = $"{period:D2} - Traditional",
                        SchoolReference = school.GetSchoolReference()
                    };

                    context.ClassPeriods.Add(classPeriod);
                }
            }
        }
    }
}
