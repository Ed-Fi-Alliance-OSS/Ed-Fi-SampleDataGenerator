using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class TimeConfigValidatorTester : ValidatorTestBase<TimeConfigValidator, ITimeConfig>
    {
        public TimeConfigValidatorTester() : base(new TimeConfigValidator())
        {
        }

        public static TestTimeConfig ValidConfig => new TestTimeConfig
        {
            DataClockConfig = DataClockConfigValidatorTester.ValidConfig,
            SchoolCalendarConfig = SchoolCalendarConfigValidatorTester.ValidConfig
        };

        [Test]
        public void ShouldPassValidConfig()
        {
            Validate(ValidConfig, true);
        }

        [Test]
        public void ShouldFailMissingSchoolCalendarConfig()
        {
            var testConfig = ValidConfig;
            testConfig.SchoolCalendarConfig = null;
            Validate(testConfig, false);
        }
        
        [Test]
        public void ShouldFailMissingDataClockConfig()
        {
            var testConfig = ValidConfig;
            testConfig.DataClockConfig = null;
            Validate(testConfig, false);
        }

        [Test]
        public void ShouldFailWhenDataClockStartsAfterSchoolYearStart()
        {
            var testConfig = new TestTimeConfig
            {
                DataClockConfig = new TestDataClockConfig
                {
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.AddDays(1).Date,
                    DataPeriods = new List<TestDataPeriod>
                    {
                        new TestDataPeriod
                        {
                            Name = "Test",
                            StartDate = DateTime.Now.Date,
                            EndDate = DateTime.Now.AddDays(1).Date,
                        }
                    }
                },
                SchoolCalendarConfig = new TestSchoolCalendarConfig
                {
                    StartDate = DateTime.Now.AddDays(-1).Date,
                    EndDate = DateTime.Now.AddDays(1).Date
                }
            };

            Validate(testConfig, false);
        }

        [Test]
        public void ShouldFailWhenDataClockEndsAfterSchoolYear()
        {
            var testConfig = new TestTimeConfig
            {
                DataClockConfig = new TestDataClockConfig
                {
                    StartDate = DateTime.Now.AddDays(-1).Date,
                    EndDate = DateTime.Now.AddDays(2).Date,
                    DataPeriods = new List<TestDataPeriod>
                    {
                        new TestDataPeriod
                        {
                            Name = "Test",
                            StartDate = DateTime.Now.AddDays(-1).Date,
                            EndDate = DateTime.Now.AddDays(2).Date,
                        }
                    }
                },
                SchoolCalendarConfig = new TestSchoolCalendarConfig
                {
                    StartDate = DateTime.Now.AddDays(-1).Date,
                    EndDate = DateTime.Now.AddDays(1).Date
                }
            };

            Validate(testConfig, false);
        }
    }

    public class SchoolCalendarConfigValidatorTester : ValidatorTestBase<SchoolCalendarConfigValidator, ISchoolCalendarConfig>
    {
        public SchoolCalendarConfigValidatorTester() : base(new SchoolCalendarConfigValidator())
        {
        }

        public static TestSchoolCalendarConfig ValidConfig => new TestSchoolCalendarConfig
        {
            StartDate = new DateTime(2016, 8, 31),
            EndDate = new DateTime(2017, 5, 31)
        };

        [Test]
        public void ShouldPassValidConfig()
        {
            Validate(ValidConfig, true);
        }

        [Test]
        public void ShouldFailStartDateYearMismatch()
        {
            var config = new TestSchoolCalendarConfig
            {
                StartDate = new DateTime(2016, 8, 31),
                EndDate = new DateTime(1990, 1, 1)
            };
            Validate(config, false);
        }
    }

    public class DataClockConfigValidatorTester : ValidatorTestBase<DataClockConfigValidator, IDataClockConfig>
    {
        public DataClockConfigValidatorTester() : base(new DataClockConfigValidator())
        {
        }

        public static TestDataClockConfig ValidConfig => new TestDataClockConfig
        {
            StartDate = new DateTime(2016, 8, 31),
            EndDate = new DateTime(2017, 5, 31),
            DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017 Part 1",
                    StartDate = new DateTime(2016, 8, 31),
                    EndDate = new DateTime(2016, 11, 26)
                },
                new TestDataPeriod
                {
                    Name = "2016-2017 Part 2",
                    StartDate = new DateTime(2016, 11, 27),
                    EndDate = new DateTime(2017, 2, 26)
                },
                new TestDataPeriod
                {
                    Name = "2016-2017 Part 3",
                    StartDate = new DateTime(2017, 2, 27),
                    EndDate = new DateTime(2017, 5, 31)
                }
            }
        };

        [Test]
        public void ShouldPassValidConfig()
        {
            Validate(ValidConfig, true);
        }

        [Test]
        public void ShouldFailWhenNoDataPeriodsDefined()
        {
            var config = ValidConfig;
            config.DataPeriods = new List<TestDataPeriod>();

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWhenDataPeriodsOverlap()
        {
            var config = ValidConfig;
            config.DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017",
                    StartDate = new DateTime(2016, 8, 31),
                    EndDate = new DateTime(2017, 5, 31)
                },
                new TestDataPeriod
                {
                    Name = "Other 2016-2017",
                    StartDate = new DateTime(2016, 8, 31),
                    EndDate = new DateTime(2017, 5, 31)
                }
            };

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWhenDataPeriodsHaveSameName()
        {
            var config = ValidConfig;
            config.DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017",
                    StartDate = new DateTime(2016, 8, 31),
                    EndDate = new DateTime(2017, 5, 31)
                },
                new TestDataPeriod
                {
                    Name = "2016-2017",
                    StartDate = new DateTime(2017, 6, 1),
                    EndDate = new DateTime(2017, 6, 2)
                }
            };

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWhenDataPeriodsHaveNamesWithFileSystemUnsafeCharacters()
        {
            var config = ValidConfig;

            config.StartDate = new DateTime(2016, 8, 31);
            config.EndDate = new DateTime(2017, 5, 31);
            config.DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017 Part 1",
                    StartDate = new DateTime(2016, 8, 31),
                    EndDate = new DateTime(2016, 11, 26)
                },
                new TestDataPeriod
                {
                    Name = "2016->2017 Part 2",
                    StartDate = new DateTime(2016, 11, 27),
                    EndDate = new DateTime(2017, 2, 26)
                },
                new TestDataPeriod
                {
                    Name = "2016\\2017 Part 3",
                    StartDate = new DateTime(2017, 2, 27),
                    EndDate = new DateTime(2017, 5, 31)
                }
            };

            ShouldFailValidation(config,
                "DataPeriod Names must be safe for use in filenames. Invalid characters: >\\");
        }

        [Test]
        public void ShouldFailWhenDataPeriodsAreNotContiguous()
        {
            var config = ValidConfig;
            config.DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017",
                    StartDate = config.StartDate,
                    EndDate = config.StartDate.AddDays(1)
                },
                new TestDataPeriod
                {
                    Name = "Other 2016-2017",
                    StartDate = config.StartDate.AddDays(3),
                    EndDate = config.EndDate
                }
            };

            ShouldFailValidation(config, "DataPeriods must be a contiguous block of time - '2016-2017' " +
                                         "and 'Other 2016-2017' have a gap in date ranges");
        }

        [Test]
        public void ShouldFailWhenFirstDataPeriodsDoesNotStartOnGlobalStartDate()
        {
            var config = ValidConfig;
            config.DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017",
                    StartDate = config.StartDate.AddDays(-1),
                    EndDate = config.StartDate.AddDays(1)
                },
                new TestDataPeriod
                {
                    Name = "Other 2016-2017",
                    StartDate = config.StartDate.AddDays(2),
                    EndDate = config.EndDate
                }
            };

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWhenLastDataPeriodsDoesNotEndOnGlobalEndDate()
        {
            var config = ValidConfig;
            config.DataPeriods = new List<TestDataPeriod>
            {
                new TestDataPeriod
                {
                    Name = "2016-2017",
                    StartDate = config.StartDate,
                    EndDate = config.StartDate.AddDays(1)
                },
                new TestDataPeriod
                {
                    Name = "Other 2016-2017",
                    StartDate = config.StartDate.AddDays(2),
                    EndDate = config.EndDate.AddDays(1)
                }
            };

            Validate(config, false);
        }


        [Test]
        public void ShouldFailWhenStartDateAfterEndDate()
        {
            var config = ValidConfig;
            config.StartDate = new DateTime(2016, 8, 31);
            config.EndDate = new DateTime(2016, 8, 30);

            Validate(config, false);
        }
    }

    [TestFixture]
    public class DataPeriodValidatorTester : ValidatorTestBase<DataPeriodValidator, IDataPeriod>
    {
        public DataPeriodValidatorTester() : base(new DataPeriodValidator())
        {
        }

        [Test]
        public void ShouldPassValidConfig()
        {
            var config = new TestDataPeriod
            {
                Name = "Test",
                StartDate = new DateTime(2016, 8, 30),
                EndDate = new DateTime(2016, 8, 31),
            };

            Validate(config, true);
        }

        [Test]
        public void ShouldFailOnEmptyName()
        {
            var config = new TestDataPeriod
            {
                Name = "",
                StartDate = new DateTime(2016, 8, 30),
                EndDate = new DateTime(2016, 8, 31),
            };

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWhenStartDateAfterEndDate()
        {
            var config = new TestDataPeriod
            {
                Name = "Test",
                StartDate = new DateTime(2016, 8, 31),
                EndDate = new DateTime(2016, 8, 30),
            };

            Validate(config, false);
        }
    }
}
