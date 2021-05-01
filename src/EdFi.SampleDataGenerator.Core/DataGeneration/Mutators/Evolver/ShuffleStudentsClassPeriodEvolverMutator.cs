using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Evolver
{
    public class ShuffleStudentsClassPeriodEvolverMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        public override IEntity Entity => StudentEnrollmentEntity.StudentSectionAssociation;
        public override IEntityField EntityField => StudentEnrollmentField.StudentSectionAssociationList;
        public override string Name => "ShuffleStudentsClassPeriod";
        public override MutationType MutationType => MutationType.Evolution;

        public ShuffleStudentsClassPeriodEvolverMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            
            var sessions = Configuration.StudentConfig.GlobalData.EducationOrgCalendarData.Sessions
                .ForSchool(Configuration.StudentConfig.SchoolProfile)
                .Where(s => s.BeganInDateRange(DataPeriod.AsDateRange()))
                    .ToList();

            if (!sessions.Any())
                return MutationResult.NoMutation;

            var session = sessions.GetRandomItem(RandomNumberGenerator);

            var result = new List<StudentSectionAssociation>();
            var classPeriods = Configuration.StudentConfig.GlobalData.EducationOrganizationData.ClassPeriods
                .Where(cp => cp.SchoolReference.ReferencesSchool(Configuration.StudentConfig.SchoolProfile))
                .ToList();

            var oldSections = context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations.ToList();

            var sectionsToKeep = oldSections.Where(a => !a.SectionReference.SectionIdentity.CourseOfferingReference.ReferencesSession(session));
            result.AddRange(sectionsToKeep);

            var sectionsToChange = oldSections.Where(a => a.SectionReference.SectionIdentity.CourseOfferingReference.ReferencesSession(session))
                .ToList();

            var controlSectionsList = sectionsToChange.ToList();
            var numberOfSectionsToChange = sectionsToChange.Count;

            for (int i = 0; i < numberOfSectionsToChange; i++)
            {
                var currentSectionAssociation = sectionsToChange[i];
                var randomIndex = RandomNumberGenerator.Generate(0, controlSectionsList.Count);

                var classPeriod = classPeriods.First(cp => currentSectionAssociation.SectionReference
                    .GetSection(Configuration.StudentConfig).ClassPeriodReference.First().ReferencesClassPeriod(cp));

                var randomSectionSelected = controlSectionsList[randomIndex];

                var selectedCourseReference = randomSectionSelected.SectionReference.SectionIdentity
                    .CourseOfferingReference;

                var selectedCourseOffering = Configuration.StudentConfig.GlobalData.MasterScheduleData.CourseOfferings
                    .FirstOrDefault(x => selectedCourseReference.ReferencesCourseOffering(x) &&
                    x.SchoolReference.ReferencesSchool(Configuration.StudentConfig.SchoolProfile) && 
                    x.SessionReference.ReferencesSession(session));

                var selectedSection = Configuration.StudentConfig.GlobalData.MasterScheduleData.Sections
                    .First(a => a.CourseOfferingReference.ReferencesCourseOffering(selectedCourseOffering) && a.ClassPeriodReference.First().ReferencesClassPeriod(classPeriod));

                var sectionAssocation = new StudentSectionAssociation
                {
                    SectionReference = selectedSection.GetSectionReference(),
                    StudentReference = context.Student.GetStudentReference(),
                    HomeroomIndicator = classPeriod.IsHomeRoom(),
                    HomeroomIndicatorSpecified = true,
                    BeginDate = randomSectionSelected.BeginDate,
                    EndDate = randomSectionSelected.EndDate,
                    EndDateSpecified = true
                };

                result.Add(sectionAssocation);
                controlSectionsList.Remove(randomSectionSelected);
            }
            context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations = result;
            return MutationResult.NewMutation(oldSections, context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations);
        }
    }
}
