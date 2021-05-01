using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class DisciplineHelpers
    {
        public static readonly BehaviorDescriptor[] SeriousBehaviors = { BehaviorDescriptor.StateOffense };

        public static readonly BehaviorDescriptor[] NonSeriousBehaviors =
        {
            BehaviorDescriptor.SchoolCodeOfConduct,
            BehaviorDescriptor.SchoolViolation,
            BehaviorDescriptor.Other,
        };
        public static readonly StudentParticipationCodeDescriptor[] PerpetratorCodeDescriptors = { StudentParticipationCodeDescriptor.Perpetrator };

        public static readonly StudentParticipationCodeDescriptor[] NonPerpetratorCodeDescriptors =
        {
            StudentParticipationCodeDescriptor.Reporter,
            StudentParticipationCodeDescriptor.Victim,
            StudentParticipationCodeDescriptor.Witness
        };
    }
}
