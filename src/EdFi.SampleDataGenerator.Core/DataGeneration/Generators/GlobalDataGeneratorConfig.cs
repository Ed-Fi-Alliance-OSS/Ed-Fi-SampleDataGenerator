using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using FluentValidation;
using FluentValidation.Validators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public class GlobalDataGeneratorConfig
    {
        public ISampleDataGeneratorConfig GlobalConfig { get; set; }
        public NameFileData NameFileData { get; set; }
        public DescriptorData[] DescriptorFiles { get; set; }
        public StandardsData StandardsFileData { get; set; }
        public EducationOrganizationData EducationOrganizationData { get; set; }
        public EducationOrgCalendarData EducationOrgCalendarData { get; set; }
        public MasterScheduleData MasterScheduleData { get; set; }
        public AssessmentMetadataData AssessmentMetadataData { get; set; }
        public Dictionary<int, ISchoolProfile> SchoolProfilesById { get; set; }
        public CourseLookupCache CourseLookupCache { get; set; }
        public List<GraduationPlan> GraduationPlans { get; set; } = new List<GraduationPlan>();
        public List<SeedRecord> SeedRecords { get; set; } = new List<SeedRecord>(); 
    }

    public class GlobalDataGeneratorConfigValidator : AbstractValidator<GlobalDataGeneratorConfig>
    {
        public GlobalDataGeneratorConfigValidator()
        {
            RuleFor(x => x.GlobalConfig)
                .Must(SchoolStartDateBeforeCalendarDataStart)
                .WithMessage("Calendar data within the EducationOrgCalendar contains dates prior to the SchoolCalendar.StartDate defined in the XML config file");

            RuleFor(x => x.GlobalConfig)
                .Must(SchoolYearEndsAfterCalendarDataEnd)
                .WithMessage("Calendar data within the EducationOrgCalendar contains dates after the end of SchoolCalendar.Year defined in the XML config file");

            RuleFor(x => x.GlobalConfig)
                .Must(GraduationPlanReferencesMustBeValid)
                .WithMessage("Invalid GraduationPlan Names referenced in one or more GradeProfile configuration elements");

            RuleFor(x => x.AssessmentMetadataData)
                .SetValidator(new AssessmentMetadataDataValidator());

            RuleFor(x => x.EducationOrganizationData)
                .SetValidator(new EducationOrganizationDataValidator());

            RuleFor(x => x.MasterScheduleData)
                .Must(SectionsMustHaveOnlyOneClassPeriod)
                .WithMessage("Each Section must reference exactly one class period (multiple class periods per section is not currently supported).  Section {SectionIdentifier} does not have a ClassPeriod or has more than one.");

            RuleFor(x => x.MasterScheduleData)
                .Must(SectionMustReferenceValidClassPeriods)
                .WithMessage("Each Section must reference a valid ClassPeriod.  Section {SectionIdentifier} references at least one invalid ClassPeriod");

            RuleFor(x => x.MasterScheduleData)
                .Must(SectionMustReferenceValidCourseOffering)
                .WithMessage("Each Section must reference a valid CourseOffering.  Section {SectionIdentifier} has data in CourseOfferingReference column(s) that is not valid.");

            RuleFor(x => x.MasterScheduleData)
                .Must(CourseMustHaveSectionForEveryClassPeriod)
                .WithMessage("Every Course must have at least one Section for each class period in the day. Course {CourseCode} at School {SchoolId} does not have a corresponding Section for at least one class period.");

            RuleFor(x => x.MasterScheduleData)
                .Must(CourseOfferingMustHaveSectionForEveryTerm)
                .WithMessage("Every CourseOffering must have at a Section in each term of the School Year. CourseOffering {CourseCode} at School {SchoolId} does not have a corresponding Section for {Session}");

            RuleFor(x => x.MasterScheduleData)
                .Must(CourseAcademicSubjectCountMustBeGreaterThanOrEqualToSchoolCourseLoad)
                .WithMessage("Every School must have at least as many Courses/AcademicSubjects as the SchoolProfile.CourseLoad configuration. School {SchoolId} only has Course Sections with {AcademicSubjectCount} unique subjects for {GradeLevel} but is configured with a CourseLoad of {CourseLoad}");

            RuleFor(x => x.MasterScheduleData)
                .Must(SectionMustHaveAValidCourseOffering)
                .WithMessage("Every Section must reference a valid Course / CourseOffering.  Section {SectionIdentifier} has an invalid CourseCode {CourseCode}");

            RuleFor(x => x.MasterScheduleData)
                .Must(CourseOfferingMustHaveValidCourseCode)
                .WithMessage("CourseCode '{CourseCode}' is not valid in the CourseOfferings.csv file");
        }

        private static bool SchoolStartDateBeforeCalendarDataStart(GlobalDataGeneratorConfig config, ISampleDataGeneratorConfig globalConfig)
        {
            var schoolStartDate = globalConfig.TimeConfig.SchoolCalendarConfig.StartDate;

            return
                config.EducationOrgCalendarData.CalendarDates.All(
                    d => d.Date >= schoolStartDate);
        }

        private static bool SchoolYearEndsAfterCalendarDataEnd(GlobalDataGeneratorConfig config, ISampleDataGeneratorConfig globalConfig)
        {
            var endSchoolYear = globalConfig.TimeConfig.SchoolCalendarConfig.EndDate.Year;

            return
                config.EducationOrgCalendarData.CalendarDates.All(
                    d => d.Date.Year <= endSchoolYear);
        }

        private static bool GraduationPlanReferencesMustBeValid(GlobalDataGeneratorConfig config, ISampleDataGeneratorConfig globalConfig)
        {
            var graduationPlanTemplateNames = new HashSet<string>(config.GlobalConfig.GraduationPlanTemplates.Select(gpt => gpt.Name));

            return globalConfig.DistrictProfiles.All(districtProfile => 
                !districtProfile.SchoolProfiles.Any(schoolProfile => 
                    schoolProfile.GradeProfiles.Any(gradeProfile => 
                        gradeProfile.GraduationPlanTemplateReferences.Any(graduationPlanTemplateReference => 
                            !graduationPlanTemplateNames.Contains(graduationPlanTemplateReference.Name)))));
        }

        private static bool SectionsMustHaveOnlyOneClassPeriod(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            return masterScheduleData.Sections.All(s =>
            {
                propertyValidatorContext.MessageFormatter
                    .AppendArgument("SectionIdentifier", s.SectionIdentifier);
                return s.ClassPeriodReference.Count() == 1;
            });
        }

        private static bool SectionMustReferenceValidClassPeriods(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            return masterScheduleData.Sections.All(s =>
            {
                propertyValidatorContext.MessageFormatter
                    .AppendArgument("SectionIdentifier", s.SectionIdentifier);
                return s.ClassPeriodReference.All(cpr => config.EducationOrganizationData.ClassPeriods.Any(cpr.ReferencesClassPeriod));
            });
        }

        private static bool SectionMustReferenceValidCourseOffering(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            return masterScheduleData.Sections.All(s =>
            {
                propertyValidatorContext.MessageFormatter
                    .AppendArgument("SectionIdentifier", s.SectionIdentifier);
                return masterScheduleData.CourseOfferings.Any(co => s.CourseOfferingReference.ReferencesCourseOffering(co));
            });
        }

        private static bool CourseMustHaveSectionForEveryClassPeriod(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            return config.EducationOrganizationData.Schools.All(school =>
            {
                var classPeriodsForSchool = config.EducationOrganizationData.ClassPeriods
                    .Where(classPeriod => classPeriod.SchoolReference.ReferencesSchool(school.SchoolId))
                    .ToList();

                var coursesForSchool = config.EducationOrganizationData.Courses
                    .Where(c => c.EducationOrganizationReference.ReferencesSchool(school.SchoolId));

                return coursesForSchool.All(course =>
                {
                    var sectionsForCourse = masterScheduleData.Sections.Where(s => s.LocationSchoolReference.ReferencesSchool(school.SchoolId) && s.GetCourse(config).CourseCode == course.CourseCode);
                    propertyValidatorContext.MessageFormatter
                        .AppendArgument("SchoolId", school.SchoolId)
                        .AppendArgument("CourseCode", course.CourseCode);

                    return classPeriodsForSchool.All(classPeriod => sectionsForCourse.Any(s => s.ClassPeriodReference.All(x => x.ReferencesClassPeriod(classPeriod))));
                });
            });
        }

        private static bool CourseOfferingMustHaveSectionForEveryTerm(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            return config.EducationOrganizationData.Schools.All(school =>
            {
                var sessionsForSchool = config.EducationOrgCalendarData.Sessions
                    .Where(s => s.SchoolReference.ReferencesSchool(school.SchoolId));

                var courseOfferingsForSchool = config.MasterScheduleData.CourseOfferings
                    .Where(c => c.SchoolReference.ReferencesSchool(school.SchoolId));

                var courseOfferingsByCourseCode = courseOfferingsForSchool
                    .GroupBy(co => co.LocalCourseCode);

                return courseOfferingsByCourseCode.All(courseOfferingGroup =>
                {
                    var courseCode = courseOfferingGroup.Key;
                    var courseOfferings = courseOfferingGroup.ToList();
                    return sessionsForSchool.All(session =>
                    {
                        propertyValidatorContext.MessageFormatter
                            .AppendArgument("SchoolId", school.SchoolId)
                            .AppendArgument("CourseCode", courseCode)
                            .AppendArgument("Session", session.SessionName);

                        return courseOfferings.Any(co => co.SessionReference.ReferencesSession(session));
                    });
                });
            });
        }

        private static bool CourseAcademicSubjectCountMustBeGreaterThanOrEqualToSchoolCourseLoad(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            var schoolProfiles = config.GlobalConfig.DistrictProfiles.SelectMany(dp => dp.SchoolProfiles).ToList();

            return schoolProfiles.All(schoolProfile =>
            {
                var school = config.EducationOrganizationData.Schools.First(s => s.SchoolId == schoolProfile.SchoolId);

                return schoolProfile.GradeProfiles.All(grade =>
                {
                    var gradeLevel = grade.GetGradeLevel();

                    var coursesForSchool = config.EducationOrganizationData.Courses
                        .Where(c => c.EducationOrganizationReference.ReferencesSchool(school.SchoolId) && c.OfferedGradeLevel.Contains(gradeLevel.GetStructuredCodeValue()));

                    var academicSubjectCount = coursesForSchool.Select(c => c.AcademicSubject).Distinct().Count();

                    propertyValidatorContext.MessageFormatter
                        .AppendArgument("SchoolId", school.SchoolId)
                        .AppendArgument("AcademicSubjectCount", academicSubjectCount)
                        .AppendArgument("CourseLoad", schoolProfile.CourseLoad)
                        .AppendArgument("GradeLevel", gradeLevel.CodeValue);

                    return academicSubjectCount >= schoolProfile.CourseLoad;
                });
            });
        }

        private static bool SectionMustHaveAValidCourseOffering(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            var courseOfferings = masterScheduleData.CourseOfferings;
            return masterScheduleData.Sections.All(section =>
            {
                return courseOfferings.Any(co =>
                {
                    propertyValidatorContext.MessageFormatter
                        .AppendArgument("SectionIdentifier", section.SectionIdentifier)
                        .AppendArgument("CourseCode", co.LocalCourseCode);

                    return section.CourseOfferingReference.ReferencesCourseOffering(co);
                });
            });
        }

        private bool CourseOfferingMustHaveValidCourseCode(GlobalDataGeneratorConfig config, MasterScheduleData masterScheduleData, PropertyValidatorContext propertyValidatorContext)
        {
            return masterScheduleData.CourseOfferings.All(offering =>
            {
                propertyValidatorContext.MessageFormatter
                    .AppendArgument("CourseCode", offering.LocalCourseCode);

                return config.EducationOrganizationData.Courses.Any(course => offering.CourseReference.ReferencesCourse(course));
            });
        }
    }
    
    public class AssessmentMetadataDataValidator : AbstractValidator<AssessmentMetadataData>
    {
        public AssessmentMetadataDataValidator()
        {
            RuleFor(x => x.Assessments).NotEmpty();
            RuleForEach(x => x.Assessments).SetValidator(new AssessmentValidator());
        }
    }

    public class AssessmentValidator : AbstractValidator<Assessment>
    {
        public AssessmentValidator()
        {
            RuleFor(x => x.AssessmentPerformanceLevel)
                .NotNull()
                .WithMessage("At least one AssessmentPerformanceLevel in Assessment {AssessmentTitle} must be defined");

            var passingCodeValues = string.Join(",", Constants.AssessmentPerformanceLevel.PerformanceLevelMetValues);
            var failingCodeValues = string.Join(",", Constants.AssessmentPerformanceLevel.PerformanceLevelNotMetValues);

            RuleFor(x => x.AssessmentPerformanceLevel)
                .Must(DefineAtLeastOnePassingAndOneFailingScore)
                .WithMessage($"Assessment {{AssessmentTitle}} must define a set of PerformanceLevels that include one of each of the following sets: '{passingCodeValues}' and '{failingCodeValues}'");

            RuleFor(x => x.AssessmentPerformanceLevel)
                .Must(DefineScores)
                .WithMessage("AssessmentPerformanceLevel {PerformanceLevelName} in Assessment {AssessmentTitle} must define a MinimumScore and MaximumScore");
        }

        private static bool DefineAtLeastOnePassingAndOneFailingScore(Assessment assessment, AssessmentPerformanceLevel[] assessmentPerformanceLevels, PropertyValidatorContext propertyValidatorContext)
        {
            return assessmentPerformanceLevels.Any(pl =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("AssessmentTitle", assessment.AssessmentTitle);
                return Constants.AssessmentPerformanceLevel.PerformanceLevelMetValues.Contains(pl.PerformanceLevel?.ParseToCodeValue());
            }) &&
            assessmentPerformanceLevels.Any(pl =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("AssessmentTitle", assessment.AssessmentTitle);
                return Constants.AssessmentPerformanceLevel.PerformanceLevelNotMetValues.Contains(pl.PerformanceLevel?.ParseToCodeValue());
            });
        }

        private static bool DefineScores(Assessment assessment, AssessmentPerformanceLevel[] assessmentPerformanceLevels, PropertyValidatorContext propertyValidatorContext)
        {
            return assessmentPerformanceLevels.All(pl =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("AssessmentTitle", assessment.AssessmentTitle);
                propertyValidatorContext.MessageFormatter.AppendArgument("PerformanceLevelName", pl.PerformanceLevel.ParseToCodeValue());
                return !string.IsNullOrEmpty(pl.MinimumScore) && !string.IsNullOrEmpty(pl.MaximumScore);
            });
        }
    }

    public class EducationOrganizationDataValidator : AbstractValidator<EducationOrganizationData>
    {
        public EducationOrganizationDataValidator()
        {
            RuleFor(x => x.LocalEducationAgencies)
                .NotEmpty()
                .WithMessage("At least one LocalEducationAgency must be defined.");

            RuleFor(x => x.Schools)
                .NotEmpty()
                .WithMessage("At least one school must be defined.");

            RuleFor(x => x.Locations)
                .Must(HaveAtLeastOneLocationPerSchool)
                .WithMessage("{SchoolName} must have at least one Location defined.");

            RuleFor(x => x.ClassPeriods)
                .Must(HaveAtLeastOneClassPeriodPerSchool)
                .WithMessage("{SchoolName} must have at least one Class Period defined.");

            RuleFor(x => x.ClassPeriods)
                .Must(HaveValidClassPeriodName)
                .WithMessage("Class period {ClassPeriod} has an invalid name. Class period names must begin with the class period number (e.g. 01, 02, 03...).");

            RuleFor(x => x.Courses)
                .Must(HaveValidAtLeastOneOfferedGradeLevel)
                .WithMessage("Course {CourseTitle} must define at least one OfferedGradeLevel.");

            RuleFor(x => x.Programs)
                .Must(HaveProgramNameValue)
                .WithMessage("Program {ProgramId} must have name.");

            RuleFor(x => x.Programs)
                .Must(HaveEducationOrganizationReferenceValue)
                .WithMessage("{ProgramName} must have an Education Organization Reference value.");

            RuleFor(x => x.Programs)
                .Must(HaveStandardPrograms)
                .WithMessage("The LocalEducationAgency {NameOfInstitution} must have a RegularEducation program.");
        }

        private static bool HaveValidAtLeastOneOfferedGradeLevel(EducationOrganizationData educationOrganizationData, List<Course> courses, PropertyValidatorContext propertyValidatorContext)
        {
            return courses.All(c =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("CourseTitle", c.CourseTitle);
                return c.OfferedGradeLevel != null && c.OfferedGradeLevel.Length > 0 &&
                       c.OfferedGradeLevel.All(x => !string.IsNullOrEmpty(x));
            });
        }

        private static bool HaveValidClassPeriodName(EducationOrganizationData educationOrganizationData, List<ClassPeriod> classPeriods, PropertyValidatorContext propertyValidatorContext)
        {
            return classPeriods.All(cp =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("ClassPeriod", cp.ClassPeriodName);
                return cp.HasValidClassPeriodName();
            });
        }

        private static bool HaveAtLeastOneLocationPerSchool(EducationOrganizationData educationOrganizationData, List<Location> locations, PropertyValidatorContext propertyValidatorContext)
        {
            return educationOrganizationData.Schools.All(s =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("SchoolName", s.NameOfInstitution);
                return locations.Any(l => l.SchoolReference.ReferencesSchool(s.SchoolId));
            });
        }

        private static bool HaveAtLeastOneClassPeriodPerSchool(EducationOrganizationData educationOrganizationData, List<ClassPeriod> classPeriods, PropertyValidatorContext propertyValidatorContext)
        {
            return educationOrganizationData.Schools.All(school =>
            {
                return classPeriods.Any(cp =>
                {
                    propertyValidatorContext.MessageFormatter.AppendArgument("SchoolName", school.NameOfInstitution);
                    return cp.SchoolReference.ReferencesSchool(school.SchoolId);
                });
            });
        }

        private static bool HaveProgramNameValue(EducationOrganizationData educationOrganizationData, List<Program> programs, PropertyValidatorContext propertyValidatorContext)
        {
            return !programs.Any(p =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("ProgramId", p.ProgramId);
                return string.IsNullOrEmpty(p.ProgramName);
            });
        }

        private static bool HaveEducationOrganizationReferenceValue(EducationOrganizationData educationOrganizationData, List<Program> programs, PropertyValidatorContext propertyValidatorContext)
        {
            return programs.All(p =>
            {
                propertyValidatorContext.MessageFormatter.AppendArgument("ProgramName", p.ProgramName);
                return p.EducationOrganizationReference?.EducationOrganizationIdentity?.EducationOrganizationId != null;
            });
        }

        private static bool HaveStandardPrograms(EducationOrganizationData educationOrganizationData, List<Program> programs, PropertyValidatorContext propertyValidatorContext)
        {
            var localEducationAgenciesWithRegularEducationPrograms =
                programs.Where(p => p.ProgramType == ProgramTypeDescriptor.RegularEducation.GetStructuredCodeValue())
                    .Select(p => p.EducationOrganizationReference?.EducationOrganizationIdentity?.EducationOrganizationId)
                    .Distinct()
                    .ToArray();

            return educationOrganizationData.LocalEducationAgencies.TrueForAll(
                x =>
                {
                    propertyValidatorContext.MessageFormatter.AppendArgument("NameOfInstitution", x.NameOfInstitution);
                    return localEducationAgenciesWithRegularEducationPrograms.Contains(x.LocalEducationAgencyId);
                });
        }
    }
}
