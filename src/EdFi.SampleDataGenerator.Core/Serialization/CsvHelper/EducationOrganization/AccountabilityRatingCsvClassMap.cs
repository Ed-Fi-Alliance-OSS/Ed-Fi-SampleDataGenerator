using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class AccountabilityRatingCsvClassMap : CsvClassMap<AccountabilityRating>
    {
        public AccountabilityRatingCsvClassMap()
        {
            Map(x => x.Rating);
            Map(x => x.RatingDate);
            Map(x => x.RatingOrganization);
            Map(x => x.RatingProgram);
            Map(x => x.RatingTitle);
            Map(x => x.SchoolYear).ConvertEnumerationType();
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);
            ExtensionMappings();
        }
    }
}