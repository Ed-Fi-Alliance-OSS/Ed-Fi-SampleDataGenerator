using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentCohort
{
    public class CohortEntityGenerator : StudentCohortEntityGlobalGenerator
    {
        public override IEntity GeneratesEntity => StudentCohortEntity.Cohort;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(EducationOrganizationEntity.LocalEducationAgency, EducationOrganizationEntity.Program);

        private int _interventionId = 1;

        public CohortEntityGenerator() : this(new RandomNumberGenerator())
        {
        }

        public CohortEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            foreach (var edOrgReference in context.GlobalData.EducationOrganizationData.Schools.Select(x => EdFiReferenceTypeHelpers.GetEducationOrganizationReference(x.SchoolId)))
            {
                context.GlobalData.CohortData.Cohorts.Add(
                    CreateCohort(edOrgReference, CohortTypeDescriptor.AcademicIntervention, CohortScopeDescriptor.School));

                context.GlobalData.CohortData.Cohorts.Add(
                    CreateCohort(edOrgReference, CohortTypeDescriptor.AttendanceIntervention, CohortScopeDescriptor.School));

                context.GlobalData.CohortData.Cohorts.Add(
                    CreateCohort(edOrgReference, CohortTypeDescriptor.DisciplineIntervention, CohortScopeDescriptor.School));
            }
        }

        private Cohort CreateCohort(EducationOrganizationReferenceType educationOrganizationReference, CohortTypeDescriptor cohortType, CohortScopeDescriptor cohortScopeType)
        {
            return new Cohort
            {
                CohortIdentifier = $"CHRT_{educationOrganizationReference.EducationOrganizationIdentity.EducationOrganizationId}_{_interventionId++}",
                EducationOrganizationReference = educationOrganizationReference,
                CohortType = cohortType.GetStructuredCodeValue(),
                CohortScope = cohortScopeType.GetStructuredCodeValue()
            };
        }
    }
}