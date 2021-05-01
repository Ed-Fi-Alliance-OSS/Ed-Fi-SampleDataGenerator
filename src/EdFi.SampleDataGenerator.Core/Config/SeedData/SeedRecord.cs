using System;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Config.SeedData
{
    public class SeedRecord
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public SexDescriptor Gender { get; set; }
        public RaceDescriptor Race { get; set; }
        public bool HispanicLatinoEthnicity { get; set; }
        public DateTime BirthDate { get; set; }
        public GradeLevelDescriptor GradeLevel { get; set; }
        public double PerformanceIndex { get; set; }
        public int SchoolId { get; set; }
    }

    public interface IHasSeedRecord
    {
        SeedRecord SeedRecord { get; set; }
        bool HasSeedRecord { get; }
    }
}
