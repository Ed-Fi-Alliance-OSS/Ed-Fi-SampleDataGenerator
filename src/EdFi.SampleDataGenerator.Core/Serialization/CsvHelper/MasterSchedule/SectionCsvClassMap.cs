using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.MasterSchedule
{
    public sealed partial class SectionCsvClassMap : CsvClassMap<Section>
    {
        public SectionCsvClassMap()
        {
            Map(x => x.SectionIdentifier);
            Map(x => x.SequenceOfCourse);
            Map(x => x.EducationalEnvironment);
            Map(x => x.InstructionLanguage);
            References<CreditsCsvClassMap>(x => x.AvailableCredits);
            References<CourseOfferingReferenceTypeCsvClassMap>(x => x.CourseOfferingReference);
            References<SchoolReferenceTypeCsvClassMap>(x => x.LocationSchoolReference);
            References<LocationReferenceTypeCsvClassMap>(x => x.LocationReference);
            References<ClassPeriodReferenceTypeCsvClassMap>(x => x.ClassPeriodReference);
            References<ProgramReferenceTypeCsvClassMap>(x => x.ProgramReference);
            ExtensionMappings();
        }
    }
}