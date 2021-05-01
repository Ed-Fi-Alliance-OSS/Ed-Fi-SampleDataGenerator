using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public class ParentProfile
    {
        public FamilyStructureType FamilyStructure { get; set; }
        public Parent Parent1 { get; set; }
        public Parent Parent2 { get; set; }
    }

    public class Parent
    {
        public Entities.Parent Entity { get; set; }
        public RelationDescriptor RelationDescriptor { get; set; }
        public bool LivesWithStudent { get; set; }
        public bool Remarried { get; set; }
    }
}
