using System.Linq;
using FluentValidation;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public static class ValidatorTestExtensions
    {
        public static void AssertValidationResult<TValidator, TClass>(this TValidator validator, TClass itemToValidate, bool expectedResult) where TValidator: IValidator<TClass>
        {
            var result = validator.Validate(itemToValidate);
            result.IsValid.ShouldBe(expectedResult, string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }

        public static void AssertValidationFailure<TValidator, TClass>(this TValidator validator, TClass itemToValidate, params string[] expectedErrors) where TValidator : IValidator<TClass>
        {
            var result = validator.Validate(itemToValidate);
            result.Errors.Select(e => e.ErrorMessage).ShouldBe(expectedErrors);
            result.IsValid.ShouldBe(false);
        }
    }
}
