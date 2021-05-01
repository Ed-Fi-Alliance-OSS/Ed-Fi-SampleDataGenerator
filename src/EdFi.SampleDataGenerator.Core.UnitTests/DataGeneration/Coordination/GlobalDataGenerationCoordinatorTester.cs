using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Coordination;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Coordination
{
    [TestFixture]
    public class GlobalDataGenerationCoordinatorTester : GeneratorTestBase
    {
        private class StubInterchangeFileOutputService : IInterchangeFileOutputService
        {
            private readonly List<KeyValuePair<string, object>> _output = new List<KeyValuePair<string, object>>();
            private readonly List<KeyValuePair<string, Manifest>> _manifests = new List<KeyValuePair<string, Manifest>>();

            public void WriteOutputToFile<TInterchangeEntity>(string outputFilePath, TInterchangeEntity interchangeEntity)
            {
                _output.Add(new KeyValuePair<string, object>(outputFilePath, interchangeEntity));
            }

            public void WriteManifestToFile(string outputFilePath, Manifest manifest)
            {
                _manifests.Add(new KeyValuePair<string, Manifest>(outputFilePath, manifest));
            }

            public string[] OutputFilePaths
            {
                get { return _output.Select(x => x.Key).ToArray(); }
            }

            public TInterchangeEntity GetOutput<TInterchangeEntity>(string outputFilePath)
            {
                return (TInterchangeEntity)_output.Single(x => x.Key == outputFilePath).Value;
            }

            public string[] ManifestFilePaths
            {
                get { return _manifests.Select(x => x.Key).ToArray(); }
            }

            public Manifest GetManifest(string outputFilePath)
            {
                return _manifests.Single(x => x.Key == outputFilePath).Value;
            }
        }

        private RandomNumberGenerator randomNumberGenerator;
        private StubInterchangeFileOutputService fileOutputService;
        private Mock<IBufferedEntityOutputService<MutationLogEntry, MutationLogOutputConfiguration>> mutationLogOutputService;
        private Mock<IGlobalMutator> mutator;
        private GlobalDataGenerationCoordinator globalDataGenerationCoordinator;
        private IDataPeriod dataPeriod1;
        private IDataPeriod dataPeriod2;
        private IDataPeriod dataPeriod3;

        [SetUp]
        public void SetUp()
        {
            randomNumberGenerator = new RandomNumberGenerator();

            fileOutputService = new StubInterchangeFileOutputService();

            mutationLogOutputService = new Mock<IBufferedEntityOutputService<MutationLogEntry, MutationLogOutputConfiguration>>();
            mutationLogOutputService.Setup(x => x.WriteToOutput(It.IsAny<MutationLogEntry>()));
            mutationLogOutputService.Setup(x => x.FlushOutput());

            mutator = new Mock<IGlobalMutator>();
            mutator.Setup(x => x.Configure(It.IsAny<GlobalDataMutatorConfiguration>()));
            mutator.Setup(x => x.MutateData(It.IsAny<GlobalDataGeneratorContext>(), It.IsAny<IDataPeriod>())).Returns(new[] { MutationResult.NewMutation("foo", "bar") });

            var globalDataOutputService = new GlobalDataOutputService(fileOutputService);

            globalDataGenerationCoordinator = new GlobalDataGenerationCoordinator(
                randomNumberGenerator,
                globalDataOutputService,
                mutationLogOutputService.Object,
                rng => mutator.Object.Yield()
            );

            dataPeriod1 = new TestDataPeriod { StartDate = new DateTime(2016, 8, 23), EndDate = new DateTime(2016, 8, 24), Name = "Part 1" };
            dataPeriod2 = new TestDataPeriod { StartDate = new DateTime(2016, 8, 25), EndDate = new DateTime(2016, 8, 26), Name = "Part 2" };
            dataPeriod3 = new TestDataPeriod { StartDate = new DateTime(2016, 8, 27), EndDate = new DateTime(2016, 8, 28), Name = "Part 3" };

            var orderedDataPeriods = new[] {dataPeriod1, dataPeriod2, dataPeriod3}.OrderBy(x => x.StartDate).ToArray();

            var sampleDataGeneratorConfig = GetSampleDataGeneratorConfig();
            var globalDataGeneratorConfig = GetGlobalDataGeneratorConfig(sampleDataGeneratorConfig);
            var timeConfig = TestTimeConfig.Default;
            timeConfig.DataClockConfig = new TestDataClockConfig
            {
                DataPeriods = orderedDataPeriods,
                StartDate = orderedDataPeriods.First().StartDate,
                EndDate = orderedDataPeriods.Last().EndDate
            };
            sampleDataGeneratorConfig.TimeConfig = timeConfig;

            globalDataGenerationCoordinator.Run(globalDataGeneratorConfig);
        }

        [Test]
        public void ShouldRunMutatorsOnAllDataPeriodsButTheLast()
        {
            mutator.Verify(x => x.MutateData(It.IsAny<GlobalDataGeneratorContext>(), dataPeriod1), Times.Exactly(1));
            mutator.Verify(x => x.MutateData(It.IsAny<GlobalDataGeneratorContext>(), dataPeriod2), Times.Exactly(1));
            mutator.Verify(x => x.MutateData(It.IsAny<GlobalDataGeneratorContext>(), dataPeriod3), Times.Never);
        }

        [Test]
        public void ShouldLogMutations()
        {
            mutationLogOutputService.Verify(x => x.WriteToOutput(It.IsAny<MutationLogEntry>()), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldGroupOutputByDataPeriodAcrossSchools()
        {
            fileOutputService.ManifestFilePaths.Length.ShouldBe(3);

            var targetInterchanges = new[]
            {
                Interchange.Descriptors,
                Interchange.Standards,
                Interchange.EducationOrganization,
                Interchange.EducationOrgCalendar,
                Interchange.MasterSchedule,
                Interchange.StaffAssociation,
                Interchange.StudentEnrollment,
                Interchange.AssessmentMetadata,
                Interchange.StudentCohort,
                Interchange.StudentGradebook
            };

            var manifestPart1 = fileOutputService.GetManifest(".\\ManifestGlobalData-Part 1.xml").ToXml().ToString();
            var manifestPart2 = fileOutputService.GetManifest(".\\ManifestGlobalData-Part 2.xml").ToXml().ToString();
            var manifestPart3 = fileOutputService.GetManifest(".\\ManifestGlobalData-Part 3.xml").ToXml().ToString();

            foreach (var targetInterchange in targetInterchanges)
            {
                manifestPart1.ShouldContain(ExpectedManifestEntry(targetInterchange, "Part 1"));
                manifestPart2.ShouldContain(ExpectedManifestEntry(targetInterchange, "Part 2"));
                manifestPart3.ShouldContain(ExpectedManifestEntry(targetInterchange, "Part 3"));
            }

            fileOutputService.OutputFilePaths.Length.ShouldBe(30);

            fileOutputService.GetOutput<InterchangeDescriptors>(".\\Descriptors-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStandards>(".\\Standards-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeEducationOrganization>(".\\EducationOrganization-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeEducationOrgCalendar>(".\\EducationOrgCalendar-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeMasterSchedule>(".\\MasterSchedule-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStaffAssociation>(".\\StaffAssociation-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentEnrollment>(".\\StudentEnrollment-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeAssessmentMetadata>(".\\AssessmentMetadata-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentCohort>(".\\StudentCohort-Part 1.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentGradebook>(".\\StudentGradebook-Part 1.xml").ShouldNotBeNull();

            fileOutputService.GetOutput<InterchangeDescriptors>(".\\Descriptors-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStandards>(".\\Standards-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeEducationOrganization>(".\\EducationOrganization-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeEducationOrgCalendar>(".\\EducationOrgCalendar-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeMasterSchedule>(".\\MasterSchedule-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStaffAssociation>(".\\StaffAssociation-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentEnrollment>(".\\StudentEnrollment-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeAssessmentMetadata>(".\\AssessmentMetadata-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentCohort>(".\\StudentCohort-Part 2.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentGradebook>(".\\StudentGradebook-Part 2.xml").ShouldNotBeNull();

            fileOutputService.GetOutput<InterchangeDescriptors>(".\\Descriptors-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStandards>(".\\Standards-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeEducationOrganization>(".\\EducationOrganization-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeEducationOrgCalendar>(".\\EducationOrgCalendar-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeMasterSchedule>(".\\MasterSchedule-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStaffAssociation>(".\\StaffAssociation-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentEnrollment>(".\\StudentEnrollment-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeAssessmentMetadata>(".\\AssessmentMetadata-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentCohort>(".\\StudentCohort-Part 3.xml").ShouldNotBeNull();
            fileOutputService.GetOutput<InterchangeStudentGradebook>(".\\StudentGradebook-Part 3.xml").ShouldNotBeNull();
        }

        private static string ExpectedManifestEntry(Interchange interchange, string dataPeriodName) => 
$@"  <Interchange>
    <Filename>{interchange.Name}-{dataPeriodName}.xml</Filename>
    <Type>{interchange.Name}</Type>
  </Interchange>";
    }
}