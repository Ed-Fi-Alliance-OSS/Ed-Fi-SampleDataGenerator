using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentCohort
{
    public class StaffCohortAssociationEntityGenerator : StudentCohortEntityGlobalGenerator
    {
        public override IEntity GeneratesEntity => StudentCohortEntity.StaffCohortAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentCohortEntity.Cohort, StaffAssociationEntity.StaffEducationOrganizationAssignmentAssociation);

        public StaffCohortAssociationEntityGenerator() : this(new RandomNumberGenerator())
        {
        }

        public StaffCohortAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            var staffCohortAssociations = GenerateStaffCohortAssociations(context);
            context.GlobalData.CohortData.StaffCohortAssociations = staffCohortAssociations.ToList();
        }

        private IEnumerable<StaffCohortAssociation> GenerateStaffCohortAssociations(GlobalDataGeneratorContext context)
        {
            var administrativeStaffClassifications = new[]
            {
                StaffClassificationDescriptor.Principal,
                StaffClassificationDescriptor.AssistantPrincipal
            };

            var administrativeStaff = context.GlobalData.StaffAssociationData.StaffEducationOrganizationAssignmentAssociation
                                    .Where(staff => administrativeStaffClassifications.Any(sc => sc.CodeValue == staff.StaffClassification)).ToList();

            return context.GlobalData.CohortData.Cohorts.SelectMany(
                    c => CreateStaffCohortAssociationByStaffEdOrgAssignments(c,
                            administrativeStaff.Where(s => s.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(c.EducationOrganizationReference))));
        }

        private IEnumerable<StaffCohortAssociation> CreateStaffCohortAssociationByStaffEdOrgAssignments(Cohort cohort, IEnumerable<StaffEducationOrganizationAssignmentAssociation> staffEdOrgAssignments)
        {
            return staffEdOrgAssignments.Select(staffAssignment => 
                    new StaffCohortAssociation
                    {
                        CohortReference = cohort.GetCohortReference(),
                        StaffReference = staffAssignment.StaffReference,
                        BeginDate = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate
                    });
        }
    }
}
