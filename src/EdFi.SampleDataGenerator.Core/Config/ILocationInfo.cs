using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface ILocationInfo
    {
        string State { get; }
        ICity[] Cities { get; }
    }

    public static class LocationInfoExtensions
    {
        public static StateAbbreviationDescriptor GetStateAbbreviation(this ILocationInfo locationInfo)
        {
            return DescriptorHelpers.ParseFromCodeValue<StateAbbreviationDescriptor, ILocationInfo>(locationInfo, l => l.State, l => $"'{l.State}' is not a valid state abbreviation");
        }
    }

    public interface ICity
    {
        string Name { get; }
        string County { get; }
        IAreaCode[] AreaCodes { get; }
        IPostalCode[] PostalCodes { get; }
    }

    public interface IAreaCode
    {
        int Value { get; }
    }

    public interface IPostalCode
    {
        string Value { get; }
    }

    public class LocationInfoValidator : AbstractValidator<ILocationInfo>
    {
        public LocationInfoValidator(string districtName)
        {
            RuleFor(x => x.State).NotEmpty().WithMessage("State must be defined and non-empty in {0}", districtName);
            RuleFor(x => x.State).Must(BeConvertibleToStateAbbreviationDescriptor).WithMessage($"'{0}' is not a valid state abbreviation in {1}", x => x.State, x => districtName);
            RuleFor(x => x.Cities).NotEmpty().WithMessage("At least one City must be defined for State {0} in {1}", x => x.State, x => districtName);
            RuleForEach(x => x.Cities).SetValidator(new CityValidator(districtName));
        }

        private bool BeConvertibleToStateAbbreviationDescriptor(string state)
        {
            StateAbbreviationDescriptor stateAbbreviation;
            return DescriptorHelpers.TryParseFromCodeValue(state, true, out stateAbbreviation);
        }
    }

    public class CityValidator : AbstractValidator<ICity>
    {
        public CityValidator(string districtName)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("A city for {0} has an empty name; City Name must not be defined and non-empty.", districtName);
            RuleFor(x => x.County).NotEmpty().WithMessage("County is empty for city {0} in {1}; County must be defined and non-empty.", x => x.Name, x => districtName);
            RuleFor(x => x.AreaCodes).NotEmpty().WithMessage("You must define at least one AreaCode for city {0} in {1}.", x => x.Name, x => districtName);
            RuleFor(x => x.PostalCodes).NotEmpty().WithMessage("You must define at least one PostalCode for city {0} in {1}.", x => x.Name, x => districtName);
            RuleForEach(x => x.AreaCodes).SetValidator(x => new AreaCodeValidator(x.Name, districtName));
            RuleForEach(x => x.PostalCodes).SetValidator(x => new PostalCodeValidator(x.Name, districtName));
        }
    }

    public class AreaCodeValidator : AbstractValidator<IAreaCode>
    {
        public AreaCodeValidator(string cityName, string districtName)
        {
            RuleFor(x => x.Value).InclusiveBetween(1, 999).WithMessage("Area code for city {0} in {1} must be between 001 and 999.", x => cityName, x => districtName);
        }
    }

    public class PostalCodeValidator : AbstractValidator<IPostalCode>
    {
        public PostalCodeValidator(string cityName, string districtName)
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("Postal code for city {0} in {1} may not be empty.", x => cityName, x => districtName);
        }
    }
}
