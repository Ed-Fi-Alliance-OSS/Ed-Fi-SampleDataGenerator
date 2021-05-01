using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    sealed class StaffEducationOrganizationEmploymentAssociationGenerator : StaffAssociationEntityGenerator
    {
        public StaffEducationOrganizationEmploymentAssociationGenerator(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StaffAssociationEntity.StaffEducationOrganizationEmploymentAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StaffAssociationEntity.StaffRequirement, StaffAssociationEntity.Staff);

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            foreach (var staffRequirement in context.GlobalData.StaffAssociationData.StaffRequirements)
            {
                context.GlobalData.StaffAssociationData.StaffEducationOrganizationEmploymentAssociation.Add(new StaffEducationOrganizationEmploymentAssociation
                {
                    StaffReference = staffRequirement.StaffReference,
                    EducationOrganizationReference = EdFiReferenceTypeHelpers.GetEducationOrganizationReference(staffRequirement.EducationOrganizationId),
                    FullTimeEquivalency = 1,
                    FullTimeEquivalencySpecified = true,
                    EmploymentStatus = EmploymentStatusDescriptor.TenuredOrPermanent.GetStructuredCodeValue(),
                    Department = GetDepartmentId(staffRequirement),
                    EmploymentPeriod = GetEmploymentPeriod(context, staffRequirement.StaffReference.StaffIdentity.StaffUniqueId)
                });
            }
        }

        public static string GetDepartmentId(StaffRequirement staffRequirement)
        {
            if (staffRequirement.Subjects != null && staffRequirement.Subjects.Any())
            {
                var subjectReferenceType = staffRequirement.Subjects.First();

                AcademicSubjectDescriptor academicSubject;
                if (DescriptorHelpers.TryParseFromCodeValue(subjectReferenceType.CodeValue, out academicSubject))
                {
                    if(academicSubject == AcademicSubjectDescriptor.Composite) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.CriticalReading) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.English) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.EnglishLanguageArts) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.Reading) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.Mathematics) return "200";
                    if(academicSubject == AcademicSubjectDescriptor.LifeAndPhysicalSciences) return "300";
                    if(academicSubject == AcademicSubjectDescriptor.SocialSciencesAndHistory) return "400";
                    if(academicSubject == AcademicSubjectDescriptor.SocialStudies) return "400";
                    if(academicSubject == AcademicSubjectDescriptor.Science) return "300";
                    if(academicSubject == AcademicSubjectDescriptor.FineAndPerformingArts) return "500";
                    if(academicSubject == AcademicSubjectDescriptor.ForeignLanguageAndLiterature) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.Writing) return "100";
                    if(academicSubject == AcademicSubjectDescriptor.PhysicalHealthAndSafetyEducation) return "600";
                    if(academicSubject == AcademicSubjectDescriptor.CareerAndTechnicalEducation) return "700";
                    if(academicSubject == AcademicSubjectDescriptor.ReligiousEducationAndTheology) return "800";
                    if(academicSubject == AcademicSubjectDescriptor.MilitaryScience) return "900";
                    if(academicSubject == AcademicSubjectDescriptor.Other) return "999";
                }
            }

            return "001"; //administration or other
        }

        

        private EmploymentPeriod GetEmploymentPeriod(GlobalDataGeneratorContext context, string staffId)
        {
            var staffMember =
                context.GlobalData.StaffAssociationData.Staff.First(
                    x => x.StaffUniqueId == staffId);

            var schoolYearStart = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate;

            var maxHireDate = schoolYearStart < SystemTime.Now ? schoolYearStart : SystemTime.Now;
            var minHireDate = maxHireDate.AddYears(-1 * (int)staffMember.YearsOfPriorTeachingExperience);

            return new EmploymentPeriod
            {
                HireDate = RandomNumberGenerator.GetRandomDay(minHireDate, maxHireDate)
            };
        }
    }
}
