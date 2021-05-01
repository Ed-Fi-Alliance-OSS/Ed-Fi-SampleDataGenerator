using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class InterchangeTypeHelpers
    {
        //as of the time this code was created, entities in most interchanges don't have a common base type
        //unique to the interchange, so we've manually created that mapping here
        public static readonly Type[] StandardsEntityTypes = {typeof(LearningStandard), typeof(LearningObjective)};
        public static readonly Type[] EducationOrganizationTypes = {typeof (StateEducationAgency), typeof(EducationServiceCenter), typeof(FeederSchoolAssociation), typeof(LocalEducationAgency), typeof(School), typeof(Location), typeof(ClassPeriod), typeof(Course), typeof(Program), typeof(AccountabilityRating), typeof(EducationOrganizationPeerAssociation), typeof(EducationOrganizationNetwork), typeof(EducationOrganizationNetworkAssociation)};
        public static readonly Type[] EducationOrgCalendarTypes = {typeof (CalendarDate), typeof (GradingPeriod), typeof (Session), typeof(Calendar)};
        public static readonly Type[] MasterScheduleTypes = {typeof (CourseOffering), typeof (Section)};
        public static readonly Type[] AssessmentMetadataTypes = {typeof (Assessment)};
        
        public static Type[] GetEntityTypesForInterchange(Type interchangeType)
        {
            if (interchangeType == typeof(InterchangeStandards))
                return StandardsEntityTypes;

            if (interchangeType == typeof (InterchangeEducationOrganization))
                return EducationOrganizationTypes;

            if (interchangeType == typeof(InterchangeEducationOrgCalendar))
                return EducationOrgCalendarTypes;

            if (interchangeType == typeof(InterchangeMasterSchedule))
                return MasterScheduleTypes;

            if (interchangeType == typeof (InterchangeDescriptors))
                return DescriptorHelpers.ScanDescriptorTypes().ToArray();

            if (interchangeType == typeof(InterchangeAssessmentMetadata))
                return AssessmentMetadataTypes;
            
            throw new ArgumentException($"Entities for {interchangeType.Name} not currently mapped");
        }
        
    }
}
