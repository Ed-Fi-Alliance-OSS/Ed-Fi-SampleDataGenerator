using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class AddressCsvClassMap : CsvClassMap<Address>
    {
        public AddressCsvClassMap()
        {
            Map(x => x.AddressType);
            Map(x => x.StreetNumberName);
            Map(x => x.BuildingSiteNumber);
            Map(x => x.ApartmentRoomSuiteNumber);
            Map(x => x.City);
            Map(x => x.StateAbbreviation);
            Map(x => x.NameOfCounty);
            Map(x => x.PostalCode);
            Map(x => x.CountyFIPSCode);
        }
    }
}