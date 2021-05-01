using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public class StudentCharacteristics
    {
        public OldEthnicityDescriptor OldEthnicity { get; set; }
        public RaceDescriptor Race { get; set; }
        public SexDescriptor Sex { get; set; }
        public bool HispanicLatinoEthnicity { get; set; }
        public bool IsImmigrant { get; set; }
        public bool IsFoodServiceEligible => FoodServiceElected != null;
        public SchoolFoodServiceProgramServiceDescriptor FoodServiceElected { get; set; }
        public bool IsHomeless { get; set; }
        public bool IsEconomicDisadvantaged { get; set; }
    }
}
