using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public class CourseLookupCache
    {
        private Dictionary<int, Course> _courseCache; 

        public CourseLookupCache(GlobalDataGeneratorContext context)
        {
            BuildCache(context.GlobalData.MasterScheduleData, context.GlobalData.EducationOrganizationData);
        }

        public CourseLookupCache(GlobalDataGeneratorConfig config)
        {
            BuildCache(config.MasterScheduleData, config.EducationOrganizationData);
        }

        public CourseLookupCache(MasterScheduleData masterScheduleData, EducationOrganizationData educationOrganizationData)
        {
            BuildCache(masterScheduleData, educationOrganizationData);
        }

        public Course GetCourseFromSection(Section section)
        {
            return _courseCache[section.GetUniqueSectionIdentifier()];
        }

        private void BuildCache(MasterScheduleData masterScheduleData, EducationOrganizationData educationOrganizationData)
        {
            _courseCache = masterScheduleData.Sections.ToDictionary(x => x.GetUniqueSectionIdentifier(), educationOrganizationData.LookupCourse);
        }
    }
}