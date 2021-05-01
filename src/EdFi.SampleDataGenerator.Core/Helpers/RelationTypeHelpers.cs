using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class RelationTypeHelpers
    {
        public static SexDescriptor GetSexDescriptor(this RelationDescriptor relationType)
        {
            var femaleList = new[]
            {
                RelationDescriptor.Aunt,
                RelationDescriptor.CourtAppointedGuardian,
                RelationDescriptor.FosterParent,
                RelationDescriptor.Grandmother,
                RelationDescriptor.Mother,
                RelationDescriptor.MotherStep,
            };

            var maleList = new[]
            {
                RelationDescriptor.Father,
                RelationDescriptor.FatherStep,
                RelationDescriptor.Grandfather,
                RelationDescriptor.Uncle
            };

            if (femaleList.Contains(relationType))
                return SexDescriptor.Female;

            if (maleList.Contains(relationType))
                return SexDescriptor.Male;

            throw new ArgumentException($"No gender mapping available for RelationDescriptor {relationType.CodeValue}", nameof(relationType));
        }

        public static RelationDescriptor GetCounterpartRelationType(this RelationDescriptor relationType, bool includeStepParent = false)
        {
            if (relationType.CodeValue == RelationDescriptor.Aunt.CodeValue) return RelationDescriptor.Uncle;
            if (relationType.CodeValue == RelationDescriptor.CourtAppointedGuardian.CodeValue) return RelationDescriptor.CourtAppointedGuardian;
            if (relationType.CodeValue == RelationDescriptor.Father.CodeValue) return includeStepParent ? RelationDescriptor.MotherStep : RelationDescriptor.Mother;
            if (relationType.CodeValue == RelationDescriptor.FatherStep.CodeValue) return RelationDescriptor.Mother;
            if (relationType.CodeValue == RelationDescriptor.FosterParent.CodeValue) return RelationDescriptor.FosterParent;
            if (relationType.CodeValue == RelationDescriptor.Grandfather.CodeValue) return RelationDescriptor.Grandmother;
            if (relationType.CodeValue == RelationDescriptor.Grandmother.CodeValue) return RelationDescriptor.Grandfather;
            if (relationType.CodeValue == RelationDescriptor.Mother.CodeValue) return includeStepParent ? RelationDescriptor.FatherStep : RelationDescriptor.Father;
            if (relationType.CodeValue == RelationDescriptor.MotherStep.CodeValue) return RelationDescriptor.Father;
            if (relationType.CodeValue == RelationDescriptor.Uncle.CodeValue) return RelationDescriptor.Aunt;

            throw new ArgumentException($"No Counterpart RelationDescriptor defined for {relationType.CodeValue}", nameof(relationType));
        }
    }
}
