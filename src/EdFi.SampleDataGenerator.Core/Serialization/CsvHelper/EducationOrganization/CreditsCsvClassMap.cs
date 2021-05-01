using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class CreditsCsvClassMap : CsvClassMap<Credits>
    {
        public CreditsCsvClassMap()
        {
            Map(x => x.CreditConversion);
            Map(x => x.CreditType);
            Map(x => x.Credits1);
        }
    }
}