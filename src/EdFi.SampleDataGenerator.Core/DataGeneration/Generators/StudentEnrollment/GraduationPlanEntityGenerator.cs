using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public class GraduationPlanEntityGenerator : TemplateDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        public override IEntity[] GeneratesEntities => new IEntity[] { StudentEnrollmentEntity.GraduationPlan };

        public GraduationPlanEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, EmptyGeneratorFactory)
        {
        }

        public override void Generate(GlobalDataGeneratorConfig config)
        {
            var graduationPlanTemplates = Configuration.GlobalConfig.GraduationPlanTemplates;

            foreach (var school in Configuration.SchoolProfilesById.Values)
            {
                foreach (var gradeProfile in school.GradeProfiles)
                {
                    var graduationYear = gradeProfile.GetGraduationYear(school, Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.SchoolYear());

                    var templatesForThisGrade = gradeProfile.GraduationPlanTemplateReferences.GetGraduationPlanTemplates(graduationPlanTemplates);
                    var plans = templatesForThisGrade.Select(t => t.GetGraduationPlan(school, graduationYear));

                    config.GraduationPlans.AddRange(plans);
                }
            }
        }
    }

    public class GraduationPlanGlobalDataEntityGenerator : GlobalDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        public override IEntity[] GeneratesEntities => new IEntity[] { StudentEnrollmentEntity.GraduationPlan };

        public GraduationPlanGlobalDataEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, EmptyGeneratorFactory)
        {
        }

        public override void Generate(GlobalDataGeneratorContext context)
        {
            context.GlobalData.GraduationPlans = Configuration.GraduationPlans;
        }
    }
}
