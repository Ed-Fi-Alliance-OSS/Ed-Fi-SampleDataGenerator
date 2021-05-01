using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class BiographicalGeneratorHelpers
    {
        public static Telephone GenerateTelephoneNumber(this ILocationInfo locationInfo, IRandomNumberGenerator randomNumberGenerator, string city)
        {
            var telephoneNumberType = TelephoneNumberTypeDescriptor.Mobile;
            return locationInfo.GenerateTelephoneNumber(randomNumberGenerator, city, telephoneNumberType);

        }

        public static Telephone GenerateTelephoneNumber(this ILocationInfo locationInfo, IRandomNumberGenerator randomNumberGenerator, string city, TelephoneNumberTypeDescriptor telephoneNumberType)
        {
            var phoneNumber = locationInfo.GetPhoneNumberForCity(randomNumberGenerator, city);

            return new Telephone
            {
                OrderOfPriority = 1,
                TelephoneNumber = phoneNumber,
                TelephoneNumberType = telephoneNumberType.CodeValue
            };
        }

        public static string GetPhoneNumberForCity(this ILocationInfo locationInfo, IRandomNumberGenerator randomNumberGenerator, string city)
        {
            var cityInfo = locationInfo.Cities.First(x => x.Name.Equals(city));

            var areaCode = cityInfo.AreaCodes.GetRandomItem(randomNumberGenerator).Value;
            var numberPrefix = randomNumberGenerator.Generate(100, 999);
            var number = randomNumberGenerator.Generate(0, 9999);

            return TelephoneHelpers.BuildNumber(areaCode, numberPrefix, number);
        }

        public static ElectronicMail GeneratePersonalEmailAddress(Name name, int? uniqueId, string domainName = "example.com")
        {
            return new ElectronicMail
            {
                ElectronicMailAddress = $"{name.FirstName}.{name.LastSurname}{uniqueId}@{domainName}",
                ElectronicMailType = ElectronicMailTypeDescriptor.HomePersonal.CodeValue
            };
        }

        public static ElectronicMail GeneratePersonalEmailAddress(string loginId, string domainName = "example.com")
        {
            return new ElectronicMail
            {
                ElectronicMailAddress = $"{loginId}@{domainName}",
                ElectronicMailType = ElectronicMailTypeDescriptor.HomePersonal.CodeValue
            };
        }

        public static ElectronicMail GenerateOrganizationalEmailAddress(string loginId, string educationOrganizationName)
        {
            return new ElectronicMail
            {
                ElectronicMailAddress = $"{loginId}@{educationOrganizationName.LettersOnly()}.edu",
                ElectronicMailType = ElectronicMailTypeDescriptor.Organization.CodeValue
            };
        }

        private static readonly HashSet<string> ExistingLoginIds = new HashSet<string>(); 
        public static string GenerateLoginId(this Name name)
        {
            var loginName = $"{name.FirstName.LettersOnly()}.{name.LastSurname.LettersOnly()}".ToLower();
            var loginId = loginName;
            var id = 0;
            while (ExistingLoginIds.Contains(loginId))
            {
                id++;
                loginId = $"{loginName}{id}";
            }

            ExistingLoginIds.Add(loginId);

            return loginId;
        }

        private static readonly string[] StreetTypes = { "Street", "Avenue", "Boulevard", "Way", "Lane", "Road", "Drive" };
        public static Address GenerateAddress(this ILocationInfo locationInfo, IRandomNumberGenerator randomNumberGenerator, StreetNameFile streetNameFile, AddressTypeDescriptor addressType)
        {
            var state = locationInfo.GetStateAbbreviation();
            var city = locationInfo.Cities.GetRandomItem(randomNumberGenerator);

            var streetNumber = randomNumberGenerator.Generate(10, 19999);
            var streetName = streetNameFile.GetRandomRecord(randomNumberGenerator).Name;
            var streetType = StreetTypes[streetNumber % StreetTypes.Length];

            var postalCode = city.PostalCodes.GetRandomItem(randomNumberGenerator).Value;

            return new Address
            {
                AddressType = addressType.CodeValue,
                StreetNumberName = $"{streetNumber} {streetName} {streetType}",
                City = city.Name,
                StateAbbreviation = state.CodeValue,
                NameOfCounty = city.County,
                PostalCode = postalCode
            };
        }

        public static Address GenerateHomeAddress(this ILocationInfo locationInfo, IRandomNumberGenerator randomNumberGenerator, StreetNameFile streetNameFile)
        {
            return GenerateAddress(locationInfo, randomNumberGenerator, streetNameFile, AddressTypeDescriptor.Home);
        }

        public static Address GeneratePhysicalAddress(this ILocationInfo locationInfo, IRandomNumberGenerator randomNumberGenerator, StreetNameFile streetNameFile)
        {
            return GenerateAddress(locationInfo, randomNumberGenerator, streetNameFile, AddressTypeDescriptor.Physical);
        }
    }
}
