using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public abstract class ValidatorTestBase<TValidator, TClass> where TValidator : IValidator<TClass>
    {
        private readonly TValidator _validator;

        protected ValidatorTestBase(TValidator validator)
        {
            _validator = validator;
        } 

        protected void Validate(TClass itemToValidate, bool expectedResult)
        {
            _validator.AssertValidationResult(itemToValidate, expectedResult);
        }

        protected void ShouldFailValidation(TClass itemToValidate, params string[] expectedErrors)
        {
            _validator.AssertValidationFailure(itemToValidate, expectedErrors);
        }
    }
}