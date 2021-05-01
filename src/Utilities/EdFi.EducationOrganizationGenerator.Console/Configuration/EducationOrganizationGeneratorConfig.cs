using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.EducationOrganizationGenerator.Console.Configuration
{
    public class EducationOrganizationGeneratorConfig
    {
        public List<Course> CourseTemplates { get; set; }
        public List<string> SchoolBaseNames { get; set; }
        public DistrictProfile DistrictProfile { get; set; }
        public StreetNameFile StreetNameFile { get; set; }
    }
}
