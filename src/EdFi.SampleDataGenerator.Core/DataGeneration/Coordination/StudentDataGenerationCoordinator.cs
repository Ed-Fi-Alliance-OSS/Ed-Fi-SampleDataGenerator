using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EdFi.SampleDataGenerator.Core.AutoMapper;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using log4net;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Coordination
{
    public class StudentDataGenerationCoordinator : InterchangeGroupDataGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(StudentDataGenerationCoordinator));
        private readonly IStudentDataOutputCoordinator _studentDataOutputCoordinator;
        private readonly IBufferedEntityOutputService<SeedRecord, ISampleDataGeneratorConfig> _seedDataOutputService;
        private readonly IBufferedEntityOutputService<MutationLogEntry, MutationLogOutputConfiguration> _mutatorLogOutputService;
        private readonly IMapper _mapper;

        private readonly List<IStudentMutator> _mutators;

        private int _totalStudentsGenerated;

        public static GeneratorFactoryDelegate DefaultGeneratorFactory => InterchangeDataGeneratorFactory.GetAllStudentDataGenerators;

        public delegate IEnumerable<IStudentMutator> MutatorFactoryDelegate(IRandomNumberGenerator randomNumberGenerator);

        public StudentDataGenerationCoordinator() : this(new StudentDataOutputCoordinator(), new SeedDataOutputService(), new MutationLogOutputService("StudentMutatorLog.txt"), new RandomNumberGenerator(),
            new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperCoreProfile>())), DefaultGeneratorFactory,
            MutatorFactory.StudentMutatorFactory)
        {
        }

        public StudentDataGenerationCoordinator(IStudentDataOutputCoordinator studentDataOutputCoordinator,
            IBufferedEntityOutputService<SeedRecord, ISampleDataGeneratorConfig> seedDataOutputService,
            IBufferedEntityOutputService<MutationLogEntry, MutationLogOutputConfiguration> mutatorLogOutputService, 
            IRandomNumberGenerator randomNumberGenerator,
            IMapper mapper,
            GeneratorFactoryDelegate generatorFactory,
            MutatorFactoryDelegate mutatorFactory)
            : base(randomNumberGenerator, generatorFactory)
        {
            _studentDataOutputCoordinator = studentDataOutputCoordinator;
            _seedDataOutputService = seedDataOutputService;
            _mutatorLogOutputService = mutatorLogOutputService;
            _mapper = mapper;

            _mutators = mutatorFactory?.Invoke(RandomNumberGenerator).ToList() ?? new List<IStudentMutator>();
        }

        public void Run(GlobalDataGeneratorConfig config, GlobalDataGeneratorContext globalData)
        {
            if (config == null)
            {
                throw new InvalidOperationException("Student Generation Coordinator not properly configured");
            }

            PreConfigureInitialization(config, globalData);
            var dataPeriods = config.GlobalConfig.TimeConfig.DataClockConfig.DataPeriods.OrderBy(dp => dp.StartDate).ToList();

            foreach (var districtProfile in config.GlobalConfig.DistrictProfiles)
            {
                foreach (var schoolProfile in districtProfile.SchoolProfiles)
                {
                    var schoolInstructionalDays = globalData.GlobalData.EducationOrgCalendarData.CalendarDates.GetInstructionalDays()
                                                  .Where(day => day.CalendarReference.CalendarIdentity.SchoolReference.ReferencesSchool(schoolProfile)).ToList();

                    foreach (var gradePopulationProfile in schoolProfile.GradeProfiles)
                    {
                        foreach (var studentPopulationProfile in gradePopulationProfile.StudentPopulationProfiles)
                        {
                            var studentProfile = config.GlobalConfig.StudentProfiles.GetProfileByName(studentPopulationProfile.StudentProfileReference);

                            var interchangeGeneratorConfig = _mapper.Map<StudentDataGeneratorConfig>(config);
                            interchangeGeneratorConfig.GlobalData = globalData.GlobalData;
                            interchangeGeneratorConfig.DistrictProfile = districtProfile;
                            interchangeGeneratorConfig.SchoolProfile = schoolProfile;
                            interchangeGeneratorConfig.GradeProfile = gradePopulationProfile;
                            interchangeGeneratorConfig.StudentProfile = studentProfile;

                            Configure(interchangeGeneratorConfig);

                            var gradeLevel = gradePopulationProfile.GetGradeLevel();
                            var seedRecordsForThisProfile = config.SeedRecords.Where(s => s.SchoolId == schoolProfile.SchoolId && s.GradeLevel == gradeLevel).ToList();
                            var totalSeedRecordsToGenerate = Math.Min(seedRecordsForThisProfile.Count, studentPopulationProfile.InitialStudentCount);

                            var totalTransfersIn = Configuration.GlobalConfig.OutputMode == OutputMode.Seed
                                ? 0
                                : studentPopulationProfile.TransfersIn;

                            var totalStudentsToGenerate = studentPopulationProfile.InitialStudentCount + totalTransfersIn;

                            var totalTransfersOut = Configuration.GlobalConfig.OutputMode == OutputMode.Seed
                                ? 0
                                : Math.Min(studentPopulationProfile.TransfersOut, totalStudentsToGenerate);

                            _log.Info(Configuration.GlobalConfig.OutputMode == OutputMode.Seed
                                ? $"Generating {totalStudentsToGenerate} seed records for {interchangeGeneratorConfig.GradeProfile.GradeName}/{interchangeGeneratorConfig.SchoolProfile.SchoolName}/{interchangeGeneratorConfig.DistrictProfile.DistrictName} using profile {studentPopulationProfile.StudentProfileReference}"
                                : $"Generating {totalStudentsToGenerate} students (including {totalSeedRecordsToGenerate} seed records and {totalTransfersIn} transfer students) for {interchangeGeneratorConfig.GradeProfile.GradeName}/{interchangeGeneratorConfig.SchoolProfile.SchoolName}/{interchangeGeneratorConfig.DistrictProfile.DistrictName} using profile {studentPopulationProfile.StudentProfileReference}");

                            var studentIndexes = Enumerable.Range(0, totalStudentsToGenerate).ToList();
                            var transferInStudentIndexes = new HashSet<int>(studentIndexes.GetNRandomItems(RandomNumberGenerator, totalTransfersIn));
                            var transferOutStudentIndexes = new HashSet<int>(studentIndexes.GetNRandomItems(RandomNumberGenerator, totalTransfersOut));

                            for (var i = 0; i < totalStudentsToGenerate; ++i)
                            {
                                var context = new StudentDataGeneratorContext
                                {
                                    GlobalStudentNumber = _totalStudentsGenerated + 1,
                                    GeneratedStudentData = new GeneratedStudentData(),
                                    StudentPerformanceProfile = new StudentPerformanceProfile(),
                                    StudentCharacteristics = new StudentCharacteristics(),
                                    EnrollmentDateRange = GetEnrollmentDateRange(transferInStudentIndexes.Contains(i), transferOutStudentIndexes.Contains(i), schoolInstructionalDays),
                                    SeedRecord = (i < totalSeedRecordsToGenerate) ? seedRecordsForThisProfile[i] : null
                                };

                                Generate(context);

                                for (var dataPeriodIndex = 0; dataPeriodIndex < dataPeriods.Count; dataPeriodIndex++)
                                {
                                    var dataPeriod = dataPeriods[dataPeriodIndex];

                                    //Note: there's an implicit assumption in additive generators
                                    //that they will not be run for data periods where the student 
                                    //is not enrolled.  If the below logic is changed, the generators
                                    //should also be changed.
                                    if (!context.EnrollmentDateRange.Overlaps(dataPeriod.AsDateRange()))
                                        continue;

                                    GenerateAdditiveData(context, dataPeriod);

                                    var dataPeriodEndsDuringSchoolCalendar = dataPeriodIndex < dataPeriods.Count - 1;
                                    if (dataPeriodEndsDuringSchoolCalendar)
                                        RunMutators(context, dataPeriod);

                                    if (Configuration.GlobalConfig.OutputMode == OutputMode.Standard)
                                        _studentDataOutputCoordinator.WriteToOutput(context.GeneratedStudentData, schoolProfile, dataPeriod, context.StudentPerformanceProfile.PerformanceIndex);
                                }

                                if (Configuration.GlobalConfig.OutputMode == OutputMode.Seed)
                                    _seedDataOutputService.WriteToOutput(context.GetSeedRecord(schoolProfile, gradePopulationProfile));

                                ++_totalStudentsGenerated;
                            }

                            LogStats();
                        }
                    }

                    _studentDataOutputCoordinator.FinalizeOutput(schoolProfile, dataPeriods);
                }
            }

            _seedDataOutputService.FlushOutput();
            _mutatorLogOutputService.FlushOutput();
        }

        private void PreConfigureInitialization(GlobalDataGeneratorConfig config, GlobalDataGeneratorContext globalData)
        {
            _studentDataOutputCoordinator.Configure(config.GlobalConfig);
            _seedDataOutputService.Configure(config.GlobalConfig);
            ConfigureMutatorLogService(config.GlobalConfig);

            _totalStudentsGenerated = 0;
        }

        private DateRange GetEnrollmentDateRange(bool enrollAfterSchoolYearStarts, bool withdrawBeforeSchoolYearEnds, List<CalendarDate> instructionalDays)
        {
            var enrollmentStart = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate;
            var enrollmentEnd = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.EndDate;

            if (instructionalDays.Count < 2) return new DateRange(enrollmentStart, enrollmentEnd);

            //In case we have a student that both enrolls late and withdraws early,
            //we need to ensure the enrollment date is always prior to the withdrawal date
            //so we'll keep track of the previously generated random index (if applicable) and use
            //that as the starting index for the next call to get a random index
            var randomIndex = 0;

            if (enrollAfterSchoolYearStarts)
            {
                randomIndex = RandomNumberGenerator.Generate(randomIndex, instructionalDays.Count - 1);
                enrollmentStart = instructionalDays[randomIndex].Date;
            }

            if (withdrawBeforeSchoolYearEnds)
            {
                randomIndex = RandomNumberGenerator.Generate(randomIndex+1, instructionalDays.Count);
                enrollmentEnd = instructionalDays[randomIndex].Date;
            }

            return new DateRange(enrollmentStart, enrollmentEnd);
        }

        private void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            Generators.ForEach(g => (g as StudentDataGenerator)?.GenerateAdditiveData(context, dataPeriod));
        }

        private void ConfigureMutatorLogService(ISampleDataGeneratorConfig sampleDataGeneratorConfig)
        {
            var mutatorLogConfiguration = new MutationLogOutputConfiguration
            {
                SampleDataGeneratorConfig = sampleDataGeneratorConfig
            };

            _mutatorLogOutputService.Configure(mutatorLogConfiguration);
        }

        private void RunMutators(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            _mutators.ForEach(mutator =>
            {
                mutator.Configure(new StudentDataMutatorConfiguration { StudentConfig = Configuration});
                var mutationResult = mutator.MutateData(context, dataPeriod);
                if (mutationResult.Mutated)
                {
                    var logEntry = new MutationLogEntry
                    {
                        Attribute = mutator.EntityField?.FullyQualifiedFieldName,
                        Entity = mutator.Entity.ClassName,
                        Interchange = mutator.InterchangeEntity.Interchange.Name,
                        EntityKey = context.Student.StudentUniqueId,
                        MutationType = mutator.MutationType,
                        MutatorName = mutator.GetType().Name,
                        NewValue = mutationResult.NewValue,
                        OldValue = mutationResult.OldValue
                    };

                    _mutatorLogOutputService.WriteToOutput(logEntry);
                }
            });
        }
    }
}
