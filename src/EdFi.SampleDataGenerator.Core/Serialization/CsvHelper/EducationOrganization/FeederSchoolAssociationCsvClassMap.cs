using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class FeederSchoolAssociationCsvClassMap : CsvClassMap<FeederSchoolAssociation>
    {
        public FeederSchoolAssociationCsvClassMap()
        {
            Map(x => x.FeederRelationshipDescription);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
            References<SchoolReferenceTypeCsvClassMap>(x => x.FeederSchoolReference);
            ExtensionMappings();
        }
    }
}