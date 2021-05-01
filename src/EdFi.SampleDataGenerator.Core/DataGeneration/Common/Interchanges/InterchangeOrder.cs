using System;
using System.Linq;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges
{
    public static partial class InterchangeOrder
    {
        private static readonly Lazy<InterchangeOrdering[]> DefaultOrdering = new Lazy<InterchangeOrdering[]>(Initialize);

        public static InterchangeOrdering[] GetDefaultOrdering()
        {
            return DefaultOrdering.Value;
        }

        private static InterchangeOrdering[] Initialize()
        {
            var coreInterchanges = new[]
            {
                Interchange.Descriptors,
                Interchange.Standards,
                Interchange.EducationOrganization,
                Interchange.EducationOrgCalendar,
                Interchange.MasterSchedule,
                Interchange.StaffAssociation,
                Interchange.Student,
                Interchange.StudentTranscript,
                Interchange.StudentEnrollment,
                Interchange.Parent,
                Interchange.AssessmentMetadata,
                Interchange.StudentAssessment,
                Interchange.StudentIntervention,
                Interchange.StudentProgram,
                Interchange.StudentDiscipline,
                Interchange.PostSecondaryEvent,
                Interchange.StudentAttendance,
                Interchange.StudentGrade,
                Interchange.StudentGradebook,
                Interchange.StudentCohort,
                Interchange.Finance
            };

            var interchangeOrderings =
                coreInterchanges.Concat(ExtensionInterchanges())
                    .Select((interchange, index) => new InterchangeOrdering { Interchange = interchange, Order = index })
                    .ToArray();

            Validate(interchangeOrderings);

            return interchangeOrderings;
        }

        private static void Validate(InterchangeOrdering[] interchangeOrderings)
        {
            var duplicates = interchangeOrderings.Select(x => x.Interchange.Name).Distinct().ToDictionary(
                name => name,
                name => interchangeOrderings.Count(ordering => ordering.Interchange.Name == name)
            ).Where(x => x.Value > 1).ToArray();

            if (duplicates.Any())
            {
                throw new Exception(
                    "InterchangeOrder is supposed to provide a precise order of distinct interchanges, but it is " +
                    "now producing duplicates, resulting in an ambiguous order. Review the definition of InterchangeOrder. " +
                    "Be sure that the ExtensionInterchanges() method returns interchanges with distinct names, and only " +
                    "returns interchanges that are custom extensions." +
                    Environment.NewLine + Environment.NewLine +
                    "The following interchanges appear more than once: " +
                    string.Join(", ", duplicates.Select(x => x.Key)));
            }
        }
    }

    public class InterchangeOrdering
    {
        public Interchange Interchange { get; set; }
        public int Order { get; set; }
    }
}
