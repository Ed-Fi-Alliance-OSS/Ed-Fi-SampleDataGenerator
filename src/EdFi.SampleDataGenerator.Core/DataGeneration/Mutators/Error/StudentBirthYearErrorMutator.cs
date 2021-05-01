using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Helpers;
using System.Globalization;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Error
{
    public class StudentBirthYearErrorMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Student;
        public override IEntity Entity => StudentEntity.Student;
        public override IEntityField EntityField => StudentField.BirthData;
        public override string Name => "TransposeBirthDateDigits";
        public override MutationType MutationType => MutationType.Error;

        public StudentBirthYearErrorMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            if (context.Student.BirthData == null) return MutationResult.NoMutation;
            var oldBirthDate = context.Student.BirthData.BirthDate;
            var birthYear = oldBirthDate.Year;
            if (RandomNumberGenerator.GetRandomBool())
            {
                birthYear = birthYear - 100;
            }
            else
            {
                birthYear = Convert.ToInt16(birthYear.ToString().SwapCharacters(2, 3));
            }

            DateTime updatedBirthDate;
            if (!DateTime.TryParseExact(
                ($"{birthYear:D4}/{context.Student.BirthData.BirthDate.Month:D2}/{context.Student.BirthData.BirthDate.Day:D2}"
                ), "yyyy/MM/dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out updatedBirthDate)) return MutationResult.NoMutation;
            context.Student.BirthData.BirthDate = updatedBirthDate;
            return MutationResult.NewMutation(oldBirthDate, updatedBirthDate);
        }

    }
}
