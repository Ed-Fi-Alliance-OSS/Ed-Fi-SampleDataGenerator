using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public sealed class StaffSchoolAssocationGenerator : StaffAssociationEntityGenerator
    {
        public StaffSchoolAssocationGenerator(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StaffAssociationEntity.StaffSchoolAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StaffAssociationEntity.StaffRequirement);

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            foreach (var staffRequirement in context.GlobalData.StaffAssociationData.StaffRequirements.Where(sr => !sr.IsLeaAdministrator))
            {
                context.GlobalData.StaffAssociationData.StaffSchoolAssociation.Add(new StaffSchoolAssociation
                {
                    StaffReference  = staffRequirement.StaffReference,
                    SchoolReference = GetSchoolReference(staffRequirement.EducationOrganizationId),
                    SchoolYearSpecified = true,
                    SchoolYear = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.SchoolYear(),
                    GradeLevel = staffRequirement.GradeLevel?.ToStructuredCodeValueFormatArray(),
                    ProgramAssignment = staffRequirement.ProgramAssignment.GetStructuredCodeValue(),
                    AcademicSubject = staffRequirement.Subjects?.ToStructuredCodeValueFormatArray()
                });
            }
        }

        private static SchoolReferenceType GetSchoolReference(int schoolId)
        {
            return new SchoolReferenceType
            {
                SchoolIdentity = new SchoolIdentityType
                {
                    SchoolId = schoolId,
                },
                SchoolLookup = new SchoolLookupType
                {
                    SchoolId = schoolId,
                    SchoolIdSpecified = true
                }
            };
        }
    }
}
