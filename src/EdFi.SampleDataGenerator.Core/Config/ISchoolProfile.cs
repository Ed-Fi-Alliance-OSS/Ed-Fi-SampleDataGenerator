using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface ISchoolProfile
    {
        int SchoolId { get; }
        string SchoolName { get; }
        IGradeProfile[] GradeProfiles { get; }
        IStaffProfile StaffProfile { get; }
        IDisciplineProfile DisciplineProfile { get; }
        ISchoolAttendanceProfile AttendanceProfile { get; }
        int CourseLoad { get; }

        int InitialStudentCount { get; }
    }

    public class SchoolProfileValidator : AbstractValidator<ISchoolProfile>
    {
        public SchoolProfileValidator(ISampleDataGeneratorConfig globalConfig)
        {
            RuleFor(x => x.SchoolName).NotEmpty().WithMessage("SchoolProfile SchoolName must be defined and non-empty");

            RuleFor(x => x.SchoolName).Must(HaveFileSystemSafeNames).WithMessage("SchoolProfile SchoolName '{SchoolName}' must be safe for use in filenames. Invalid characters: {InvalidCharacters}");

            RuleFor(x => x.GradeProfiles)
                .NotEmpty()
                .WithMessage("At least one GradeProfile must be defined for School '{0}'", x => x.SchoolName);

            RuleForEach(x => x.GradeProfiles).SetValidator(x => new GradeProfileValidator(x.SchoolName));

            RuleFor(x => x.DisciplineProfile)
                .NotEmpty()
                .WithMessage("DisciplineProfile must be defined for school '{0}'", x => x.SchoolName)
                .SetValidator(x => new DisciplineProfileValidator());

            RuleFor(x => x.AttendanceProfile).SetValidator(x => new SchoolAttendanceProfileValidator(x.SchoolName));

            RuleFor(x => x.InitialStudentCount)
                .GreaterThan(0)
                .WithMessage("SchoolProfile '{0}' must contain at least 1 student", x => x.SchoolName);

            RuleFor(x => x.StaffProfile)
                .NotEmpty()
                .WithMessage("Staff profile must be defined for school '{0}'", x => x.SchoolName)
                .SetValidator(x => new StaffProfileValidator(x.SchoolName, globalConfig));

            RuleFor(x => x.CourseLoad)
                .GreaterThan(0)
                .WithMessage("SchoolProfile '{0}' must define a course load", x => x.SchoolName);
        }

        private bool HaveFileSystemSafeNames(ISchoolProfile profile, string schoolName, PropertyValidatorContext context)
        {
            var invalidCharacters = schoolName.FileSystemUnsafeCharacters();

            if (invalidCharacters.Any())
            {
                context.MessageFormatter.AppendArgument("SchoolName", schoolName);
                context.MessageFormatter.AppendArgument("InvalidCharacters", new string(invalidCharacters));
                return false;
            }

            return true;
        }
    }
}