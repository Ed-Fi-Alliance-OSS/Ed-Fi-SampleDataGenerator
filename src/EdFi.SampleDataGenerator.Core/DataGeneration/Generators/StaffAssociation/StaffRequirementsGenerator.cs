using System;
using System.Collections.Generic;
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
    public sealed class StaffRequirementsGenerator : StaffAssociationEntityGenerator
    {
        public const int MinCoursesToAssignPerTeacherPerTerm = 2;
        public const int MaxCoursesToAssignPerTeacherPerTerm = 3;
        public const double PercentageOfHighlyQualifiedTeachers = .1;

        public StaffRequirementsGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StaffAssociationEntity.StaffRequirement;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(MasterScheduleEntity.Section);

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            GenerateLocalEducationAgencyAdministrativeStaff(context);
            GenerateSchoolAdministrativeStaff(context);
            GenerateTeachingStaff(context);
        }

        private void GenerateTeachingStaff(GlobalDataGeneratorContext context)
        {
            var unassignedSections = context
                .GlobalData
                .MasterScheduleData
                .Sections
                .Where(x => Configuration.SchoolProfilesById.ContainsKey(x.LocationSchoolReference.SchoolIdentity.SchoolId))
                .OrderBy(x => x.LocationSchoolReference.SchoolIdentity.SchoolId)
                .ThenBy(x => x.CourseOfferingReference.CourseOfferingIdentity.LocalCourseCode)
                .ToList();

            var terms = unassignedSections.Select(GetSessionNameFromSection).Distinct().ToList();

            while (unassignedSections.Any())
            {
                var schoolId = unassignedSections.First().LocationSchoolReference.SchoolIdentity.SchoolId;
                var subject = unassignedSections.First().GetCourse(Configuration).AcademicSubject;
                var sectionsToAssignToNewTeacher = new List<Section>();

                foreach (var term in terms)
                {
                    var sectionsToAssignForTerm = unassignedSections
                        .Where(s => GetSessionNameFromSection(s) == term)
                        .Where(s => s.LocationSchoolReference.SchoolIdentity.SchoolId == schoolId)
                        .Where(s => s.GetCourse(Configuration).AcademicSubject == subject)
                        .Take(RandomNumberGenerator.Generate(MinCoursesToAssignPerTeacherPerTerm,
                            MaxCoursesToAssignPerTeacherPerTerm))
                        .ToList();

                    sectionsToAssignToNewTeacher.AddRange(sectionsToAssignForTerm);
                    unassignedSections = unassignedSections.Except(sectionsToAssignToNewTeacher).ToList();
                }

                var assignedCourses =
                    sectionsToAssignToNewTeacher.Select(x => x.GetCourse(Configuration)).Where(x => x != null).ToList();
                context.GlobalData.StaffAssociationData.StaffRequirements.Add(new StaffRequirement
                {
                    HighlyQualified = RandomNumberGenerator.GetRandomBool(PercentageOfHighlyQualifiedTeachers),
                    SectionReference = sectionsToAssignToNewTeacher.Select(s => s.GetSectionReference()).ToArray(),
                    StaffReference = GenerateNewStaffReference(),
                    EducationOrganizationId = schoolId,
                    EducationOrganizationName = Configuration.SchoolProfilesById[schoolId].SchoolName,
                    SpeaksSpanish = sectionsToAssignToNewTeacher.Any(SectionServesSpanishSpeakingStudents),
                    GradeLevel = GetGradeLevelReferences(assignedCourses, schoolId),
                    ProgramAssignment = GetProgramReference(sectionsToAssignToNewTeacher),
                    Subjects = GetSubjectReferences(assignedCourses),
                    StaffClassification =
                        StaffClassificationDescriptor.Teacher
                });
            }
        }

        private void GenerateSchoolAdministrativeStaff(GlobalDataGeneratorContext context)
        {
            foreach (var school in Configuration.SchoolProfilesById.Values)
            {
                GenerateSchoolAdministrativeStaff(context, school);
            }
        }

        private string GetSessionNameFromSection(Section section)
        {
            return section.CourseOfferingReference.CourseOfferingIdentity.SessionReference.SessionIdentity.SessionName;
        }

        private void GenerateLocalEducationAgencyAdministrativeStaff(GlobalDataGeneratorContext context)
        {
            foreach (var localEducationAgency in context.GlobalData.EducationOrganizationData.LocalEducationAgencies)
            {
                foreach (var administrativePosition in StaffRequirement.LeaAdministrativePositions)
                {
                    context.GlobalData.StaffAssociationData.StaffRequirements.Add(new StaffRequirement
                    {
                        HighlyQualified = true,
                        SectionReference = new SectionReferenceType[] { },
                        EducationOrganizationId = localEducationAgency.LocalEducationAgencyId,
                        EducationOrganizationName = localEducationAgency.NameOfInstitution,
                        StaffReference = GenerateNewStaffReference(),
                        StaffClassification = administrativePosition,
                        ProgramAssignment = ProgramAssignmentDescriptor.RegularEducation,
                    });
                }
            }
        }

        private void GenerateSchoolAdministrativeStaff(GlobalDataGeneratorContext context, ISchoolProfile profile)
        {
            foreach (var administrativePosition in StaffRequirement.SchoolAdministrativePositions)
            {
                context.GlobalData.StaffAssociationData.StaffRequirements.Add(new StaffRequirement
                {
                    HighlyQualified = true,
                    SectionReference = new SectionReferenceType[] { },
                    StaffReference = GenerateNewStaffReference(),
                    EducationOrganizationId = profile.SchoolId,
                    EducationOrganizationName = profile.SchoolName,
                    StaffClassification = administrativePosition,
                    ProgramAssignment = ProgramAssignmentDescriptor.RegularEducation
                });
            }
        }

        private static ProgramAssignmentDescriptor GetProgramReference(IEnumerable<Section> sections)
        {
            var programs = sections
                .Where(x => x.ProgramReference != null && x.ProgramReference.Any())
                .SelectMany(x => x.ProgramReference)
                .Select(x => x.ProgramIdentity?.ProgramType ?? ProgramTypeDescriptor.RegularEducation.GetStructuredCodeValue())
                .ToList();

            if (!programs.Any() || programs.TrueForAll(p => p == ProgramTypeDescriptor.RegularEducation.GetStructuredCodeValue()))
            {
                return ProgramAssignmentDescriptor.RegularEducation;
            }

            if (programs.Contains(ProgramTypeDescriptor.Bilingual.GetStructuredCodeValue()) ||
                programs.Contains(ProgramTypeDescriptor.EnglishAsASecondLanguageESL.GetStructuredCodeValue())||
                programs.Contains(ProgramTypeDescriptor.BilingualSummer.GetStructuredCodeValue())) {
                return ProgramAssignmentDescriptor.BilingualEnglishAsASecondLanguage;
            }

            return ProgramAssignmentDescriptor.Other;
        }

        private static StaffReferenceType GenerateNewStaffReference()
        {
            var id = Guid.NewGuid().ToString("N");
            return new StaffReferenceType
            {
                StaffIdentity = new StaffIdentityType
                {
                    StaffUniqueId = id
                },

                StaffLookup = new StaffLookupType
                {
                    StaffUniqueId = id
                }
            };
        }

        private static bool SectionServesSpanishSpeakingStudents(Section section)
        {
            var populationServedLikelyRequiresSpanish =
                 section.PopulationServed == PopulationServedDescriptor.ESLStudents.GetStructuredCodeValue() ||
                 section.PopulationServed == PopulationServedDescriptor.MigrantStudents.GetStructuredCodeValue();

            var sectionTaughtInSpanish = section.InstructionLanguage == LanguageDescriptor.Spanish_spa.GetStructuredCodeValue();

            var isBillengualEslProgram = 
                GetProgramReference(new[] {section}) == ProgramAssignmentDescriptor.BilingualEnglishAsASecondLanguage;

            return populationServedLikelyRequiresSpanish || sectionTaughtInSpanish || isBillengualEslProgram;
        }

        private GradeLevelDescriptor[] GetGradeLevelReferences(IEnumerable<Course> courses, int schoolId)
        {
            var gradeLevels = courses
                .Where(x => x.OfferedGradeLevel != null)
                .SelectMany(x => x.OfferedGradeLevel)
                .Select(x => x.ParseFromStructuredCodeValue<GradeLevelDescriptor>())
                .GroupBy(x => x.CodeValue)
                .Select(x => x.First())
                .ToArray();

            return gradeLevels.Any() ? gradeLevels : new[] {GetRandomGradeLevelReference(schoolId)};
        }

        private GradeLevelDescriptor GetRandomGradeLevelReference(int schoolId)
        {
            return 
                Configuration.SchoolProfilesById[schoolId]
                    .GradeProfiles
                    .GetRandomItem(RandomNumberGenerator)
                    .GetGradeLevel();
        }

        private static AcademicSubjectDescriptor[] GetSubjectReferences(IEnumerable<Course> courses)
        {
            return courses
                .Select(x => x.AcademicSubject).Distinct()
                .Select(a => a.ParseFromStructuredCodeValue<AcademicSubjectDescriptor>()).ToArray();
        }
    }
}

