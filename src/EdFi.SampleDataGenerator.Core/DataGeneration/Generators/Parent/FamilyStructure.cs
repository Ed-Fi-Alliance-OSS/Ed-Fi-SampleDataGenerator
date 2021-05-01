using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public static class FamilyStructure
    {
        // Parent/Family structure:
        // https://www.census.gov/newsroom/press-releases/2016/cb16-192.html
        // 69% - Two married parents
        // 23% - Single mother
        //  4% - Single father
        //  4% - No parent at home
        public static readonly Dictionary<FamilyStructureType, double> FamilyStructureOdds = new Dictionary<FamilyStructureType, double>
        {
            {FamilyStructureType.MarriedParents, 0.69},
            {FamilyStructureType.SingleMother, 0.23},
            {FamilyStructureType.SingleFather, 0.04},
            {FamilyStructureType.NoParentAtHome, 0.04}
        };

        //one of {guardian, foster, grandparents or aunt/uncle}
        public static Dictionary<RelationDescriptor, double> NonParentRelationshipTypes = new Dictionary<RelationDescriptor, double>
        {
            {RelationDescriptor.Aunt, 0.25},
            {RelationDescriptor.CourtAppointedGuardian, 0.1},
            {RelationDescriptor.FosterParent, 0.10},
            {RelationDescriptor.Grandfather, 0.15},
            {RelationDescriptor.Grandmother, 0.25},
            {RelationDescriptor.Uncle, 0.15},
        };
    }

    public enum FamilyStructureType
    {
        MarriedParents,
        SingleMother,
        SingleFather,
        NoParentAtHome
    }
}
