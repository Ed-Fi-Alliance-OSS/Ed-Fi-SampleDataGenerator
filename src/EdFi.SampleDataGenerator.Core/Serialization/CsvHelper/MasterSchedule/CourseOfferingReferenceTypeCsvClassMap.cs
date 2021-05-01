using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.MasterSchedule
{
    public sealed class CourseOfferingReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<CourseOfferingReferenceType>
    {
        public CourseOfferingReferenceTypeCsvClassMap()
        {
            References<CourseOfferingIdentityTypeCsvClassMap>(x => x.CourseOfferingIdentity);
        }
    }
}