using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.MasterScheduleGenerator.Console.Configuration
{
    public class MasterScheduleGeneratorConfig
    {
        public SchoolYearType SchoolYear { get; set; }
        public TermDescriptor TermDescriptor { get; set; }
        public List<School> Schools { get; set; } 
        public List<Course> Courses { get; set; }
        public List<ClassPeriod> ClassPeriods { get; set; } 
        public List<Location> Locations { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
