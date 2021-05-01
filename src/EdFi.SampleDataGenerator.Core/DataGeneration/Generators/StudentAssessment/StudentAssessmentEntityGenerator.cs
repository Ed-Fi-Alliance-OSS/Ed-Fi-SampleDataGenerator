using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAssessment
{
    public class StudentAssessmentEntityGenerator : StudentAssessmentEntityGeneratorBase
    {
        //Assessment scores will have a small variance so the students' test
        //scores don't 1:1 map to their PerformanceIndex - this value is the
        //absolute value of the percentage variance (i.e. actual variance will be %age +/-)
        private const double StudentAssessmentScoreVariance = 0.05;
        private const double AssessmentFailureStudentPerformanceProfileCutoff = StudentPerformanceProfileDistribution.BottomQuartile;

        private List<AssessmentProbability> _assessmentProbabilities = new List<AssessmentProbability>();
        private List<Assessment> _assessmentsForGradeLevel;
        private List<CalendarDate> _schoolCalendarDates;

        private IDataPeriod _stateAssessmentsDataPeriod;
        private Dictionary<Assessment, DateTime> _stateAssessmentDates;
        private Dictionary<string, List<Assessment>> _studentAssessmentsByDataPeriodName;

        public StudentAssessmentEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentAssessmentEntity.StudentAssessment;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, AssessmentMetadataEntity.Assessment);

        private AdministrationEnvironmentDescriptor GetAssessmentAdministrationEnvironment(Assessment assessment)
        {
            if (AssessmentCategoryDescriptor.CollegeEntranceExam.CodeValue == assessment.AssessmentCategory)
                return AdministrationEnvironmentDescriptor.TestingCenter;

            return AdministrationEnvironmentDescriptor.Classroom;
        }

        private int GetStudentAssessmentScore(AssessmentPerformanceLevel performanceLevel, StudentPerformanceProfile studentPerformanceProfile)
        {
            var minScore = double.Parse(performanceLevel.MinimumScore);
            var maxScore = double.Parse(performanceLevel.MaximumScore);

            var variance = (maxScore - minScore) * StudentAssessmentScoreVariance;
            var scoreVariance = RandomNumberGenerator.GenerateDouble(-variance, variance);

            var studentScore = minScore + (studentPerformanceProfile.PerformanceIndex * (maxScore - minScore)) + scoreVariance;
            var finalStudentScore = (int)Math.Min(Math.Max(minScore, studentScore), maxScore);

            return finalStudentScore;
        }

        private bool PerformanceLevelMet(AssessmentPerformanceLevel assessmentPerformanceLevel)
        {
            return Constants.AssessmentPerformanceLevel.PerformanceLevelMetValues.Contains(assessmentPerformanceLevel.PerformanceLevel);
        }

        private int GetPerformanceLevelSortOrder(AssessmentPerformanceLevel performanceLevel)
        {
            if (performanceLevel.PerformanceLevel == null)
                throw new ArgumentNullException(nameof(performanceLevel), "AssessmentPerformanceLevel.PerformanceLevel is null'");

            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.Fail.CodeValue) return -3;
            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.WellBelowBasic.CodeValue) return -2;
            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.BelowBasic.CodeValue) return -1;
            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.Basic.CodeValue) return 0;
            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.Pass.CodeValue) return 0;
            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.Proficient.CodeValue) return 1;
            if (performanceLevel.PerformanceLevel == PerformanceBaseConversionDescriptor.Advanced.CodeValue) return 2;

            throw new ArgumentException($"AssessmentPerformanceLevel '{performanceLevel.PerformanceLevel}' not currently supported");
        }

        protected override void OnConfigure()
        {
            _assessmentsForGradeLevel =
                Configuration
                    .AssessmentMetadataData
                    .Assessments
                    .GetAssessmentsForGradeLevel(Configuration.GradeProfile)
                    .ToList();

            _assessmentProbabilities = GetAssessmentProbabilitiesFromConfiguration();
            _schoolCalendarDates = Configuration.GetSchoolInstructionalDays().ToList();

            var dataPeriods = Configuration.GlobalConfig.TimeConfig.DataClockConfig.DataPeriods.ToList();
            _stateAssessmentsDataPeriod = dataPeriods.OrderByDescending(dp => dp.StartDate).First();

            _stateAssessmentDates = _assessmentsForGradeLevel
                                        .Where(a => a.IsStateAssessment())
                                        .ToDictionary(k => k, v => GetRamdomSchoolDate(_stateAssessmentsDataPeriod.AsDateRange()));
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            _studentAssessmentsByDataPeriodName = Configuration.GlobalConfig.TimeConfig.DataClockConfig.DataPeriods
                    .ToDictionary(k => k.Name, v => new List<Assessment>());

            foreach (var assessment in _assessmentsForGradeLevel)
            {
                if (assessment.IsSATVariant() && _studentAssessmentsByDataPeriodName.Values.Any(a => a.StudentHasTakenSATVariant())) continue;
                if (!StudentShouldTakeAssessment(assessment, context.StudentPerformanceProfile)) continue;

                var dataPeriodForAssessment = GetDataPeriodForAssessment(assessment, context.EnrollmentDateRange);

                _studentAssessmentsByDataPeriodName[dataPeriodForAssessment.Name].Add(assessment);
            }
        }

        private IDataPeriod GetDataPeriodForAssessment(Assessment assessment, DateRange studentEnrollmentDateRange)
        {
            return assessment.IsStateAssessment() 
                ? _stateAssessmentsDataPeriod 
                : Configuration.GlobalConfig.TimeConfig.DataClockConfig.DataPeriods
                    .Where(dp => studentEnrollmentDateRange.Overlaps(dp.AsDateRange()))
                    .ToList()
                    .GetRandomItem(RandomNumberGenerator);
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var studentAssessments = new List<Entities.StudentAssessment>();
            var studentEnrollmentDateRange = dataPeriod.Intersect(context.EnrollmentDateRange);

            foreach (var assessment in _studentAssessmentsByDataPeriodName[dataPeriod.Name])
            {
                var chanceStudentPassed = context.StudentPerformanceProfile.PerformanceIndex <= AssessmentFailureStudentPerformanceProfileCutoff
                    ? Math.Max(0, 0.5 - (AssessmentFailureStudentPerformanceProfileCutoff - context.StudentPerformanceProfile.PerformanceIndex) / (2.0 * AssessmentFailureStudentPerformanceProfileCutoff))
                    : 1;
                var studentPassed = RandomNumberGenerator.GetRandomBool(chanceStudentPassed);

                var assessmentPerformanceLevels =
                    (
                        studentPassed
                            ? assessment.AssessmentPerformanceLevel.Where(apl => Constants.AssessmentPerformanceLevel.PerformanceLevelMetValues.Contains(apl.PerformanceLevel))
                            : assessment.AssessmentPerformanceLevel.Where(apl => Constants.AssessmentPerformanceLevel.PerformanceLevelNotMetValues.Contains(apl.PerformanceLevel))
                    )
                    .OrderBy(GetPerformanceLevelSortOrder)
                    .ToList();

                var assessmentPerformanceIndex = Math.Min((int)(context.StudentPerformanceProfile.PerformanceIndex * assessmentPerformanceLevels.Count), assessmentPerformanceLevels.Count - 1);
                var studentPerformanceLevel = assessmentPerformanceLevels[assessmentPerformanceIndex];

                var assessmentReportingMethod = studentPerformanceLevel?.AssessmentReportingMethod ?? AssessmentReportingMethodDescriptor.ScaleScore.GetStructuredCodeValue();
                var studentAssessmentScore = GetStudentAssessmentScore(studentPerformanceLevel, context.StudentPerformanceProfile);

                var administrationDate = GetAdministrationDate(studentEnrollmentDateRange, assessment);
                if (!administrationDate.HasValue)
                    continue;

                var studentAssessment = new Entities.StudentAssessment
                {
                    AdministrationDate = administrationDate.Value,
                    SerialNumber = "0",
                    AdministrationLanguage = GetAssessmentAdministrationLanguage(context, assessment),
                    AdministrationEnvironment = GetAssessmentAdministrationEnvironment(assessment).GetStructuredCodeValue(),
                    ScoreResult = new[]
                    {
                        new ScoreResult
                        {
                            Result = studentAssessmentScore.ToString("D"),
                            ResultDatatypeType = ResultDatatypeTypeDescriptor.Integer.GetStructuredCodeValue(),
                            AssessmentReportingMethod = assessmentReportingMethod
                        }
                    },
                    WhenAssessedGradeLevel = Configuration.GradeProfile.GetGradeLevel().GetStructuredCodeValue(),
                    StudentReference = context.Student.GetStudentReference(),
                    AssessmentReference = assessment.GetAssessmentReference(),
                    PerformanceLevel = new[]
                    {
                        new PerformanceLevel
                        {
                            PerformanceLevelMet = PerformanceLevelMet(studentPerformanceLevel),
                            PerformanceLevel1 = studentPerformanceLevel.PerformanceLevel
                        }
                    }
                };
                studentAssessments.Add(studentAssessment);
            }
            context.GeneratedStudentData.StudentAssessmentData.StudentAssessments.AddRange(studentAssessments);
        }

        private DateTime? GetAdministrationDate(DateRange studentEnrollmentDateRange, Assessment assessment)
        {
            if (assessment.IsStateAssessment())
            {
                var assessmentDate = _stateAssessmentDates[assessment];
                return studentEnrollmentDateRange.Contains(assessmentDate)
                    ? assessmentDate
                    : (DateTime?)null;
            }

            var administrationDate = GetRamdomSchoolDate(studentEnrollmentDateRange);
            if (assessment.IsCollegeEntranceExam())
            {
                var nextSaturday = administrationDate.FindClosestDayOfWeek(DayOfWeek.Saturday, DateTimeExtensions.SearchDirection.Forward);
                var previousSaturday = administrationDate.FindClosestDayOfWeek(DayOfWeek.Saturday, DateTimeExtensions.SearchDirection.Backward);

                return studentEnrollmentDateRange.Contains(nextSaturday)
                    ? nextSaturday
                    : studentEnrollmentDateRange.Contains(previousSaturday)
                        ? previousSaturday
                        : (DateTime?)null;
            }

            return administrationDate;
        }

        private DateTime GetRamdomSchoolDate(DateRange dateRange)
        {
            return _schoolCalendarDates.WithinDateRange(dateRange).ToList().GetRandomItem(RandomNumberGenerator).Date;
        }

        private static string GetAssessmentAdministrationLanguage(StudentDataGeneratorContext context, Assessment assessment)
        {
            var english = LanguageMapType.English.Value;

            var studentLanguage = context.GetStudentEducationOrganization().Language?.FirstOrDefault()?.Language1 ?? english;
            var assessmentLanguages = assessment.Language ?? new[] { english };

            return assessmentLanguages.FirstOrDefault(l => l == studentLanguage) ?? english;
        }

        private bool StudentShouldTakeAssessment(Assessment assessment, StudentPerformanceProfile studentPerformanceProfile)
        {
            var probability = (from x in _assessmentProbabilities
                where (x.RegexMatch && x.AssessmentTitleMatchExpression.IsMatch(assessment.AssessmentTitle) ||
                       !x.RegexMatch && x.AssessmentTitle.Equals(assessment.AssessmentTitle, StringComparison.CurrentCultureIgnoreCase))
                      && (studentPerformanceProfile.PerformanceIndex >= x.LowerPerformanceIndex
                          && studentPerformanceProfile.PerformanceIndex <= x.UpperPerformanceIndex)
                orderby x.UpperPerformanceIndex descending
                select x
            ).FirstOrDefault();

            var probabilityToTakeAssessment = probability?.ProbabilityValue ?? 0;

            return RandomNumberGenerator.GetRandomBool(probabilityToTakeAssessment);
        }

        private List<AssessmentProbability> GetAssessmentProbabilitiesFromConfiguration()
        {
            if(Configuration.GradeProfile.AssessmentParticipationConfigurations == null || Configuration.GradeProfile.AssessmentParticipationConfigurations.Length == 0)
                return new List<AssessmentProbability>();

           return  (from assessment in Configuration.GradeProfile.AssessmentParticipationConfigurations
                    from participation in assessment.ParticipationRates
                    select new AssessmentProbability
                    {
                        AssessmentTitle = assessment.AssessmentTitle,
                        LowerPerformanceIndex = participation.LowerPerformancePercentile > 0 && participation.LowerPerformancePercentile < 1 ? StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(participation.LowerPerformancePercentile) : participation.LowerPerformancePercentile,
                        UpperPerformanceIndex = participation.UpperPerformancePercentile > 0 && participation.UpperPerformancePercentile < 1 ? StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(participation.UpperPerformancePercentile) : participation.UpperPerformancePercentile,
                        ProbabilityValue = participation.Probability,
                        RegexMatch = assessment.RegexMatch,
                        AssessmentTitleMatchExpression = assessment.AssessmentTitleMatchExpression
                    }).ToList();
        }
    }

    public class AssessmentProbability
    {
        public string AssessmentTitle { get; set; }
        public double LowerPerformanceIndex { get; set; }
        public double UpperPerformanceIndex { get; set; }
        public double ProbabilityValue { get; set; }
        public bool RegexMatch { get; set; }
        public Regex AssessmentTitleMatchExpression { get; set; }
    }
}
