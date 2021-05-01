using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class SchoolCsvClassMap : EducationOrganizationCsvClassMap<School>
    {
        public SchoolCsvClassMap()
        {
            Map(x => x.CharterStatus);
            Map(x => x.SchoolCategory);
            Map(x => x.SchoolType);
            Map(x => x.SchoolId);
            Map(x => x.TitleIPartASchoolDesignation);
            Map(x => x.AdministrativeFundingControl);
            Map(x => x.GradeLevel);
            References<LocalEducationAgencyReferenceTypeCsvClassMap>(x => x.LocalEducationAgencyReference);
            ExtensionMappings();
        }
    }
}