using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class ProgramCsvClassMap : ComplexObjectTypeCsvClassMap<Program>
    {
        public ProgramCsvClassMap()
        {
            Map(x => x.ProgramId);
            Map(x => x.ProgramName);
            Map(x => x.ProgramType); 
            Map(x => x.ProgramSponsor); 
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);
            ExtensionMappings();
        }    
    }
}