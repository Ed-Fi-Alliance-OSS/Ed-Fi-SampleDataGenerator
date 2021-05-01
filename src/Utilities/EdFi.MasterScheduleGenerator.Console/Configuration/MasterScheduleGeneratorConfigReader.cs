using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.MasterScheduleGenerator.Console.Configuration
{
    public static class MasterScheduleGeneratorConfigReader
    {
        public static MasterScheduleGeneratorConfig Read(CommandLineOptions commandLineOptions)
        {
            return new MasterScheduleGeneratorConfig
            {
                SchoolYear = EnumHelpers.Parse<SchoolYearType>(commandLineOptions.SchoolYear),
                TermDescriptor = TermDescriptor.Semester,
                Schools = MappedCsvFileReader.ReadEntityFile<School>(commandLineOptions.SchoolFilePath),
                ClassPeriods = MappedCsvFileReader.ReadEntityFile<ClassPeriod>(commandLineOptions.ClassPeriodFilePath),
                Courses = MappedCsvFileReader.ReadEntityFile<Course>(commandLineOptions.CourseFilePath),
                Locations = MappedCsvFileReader.ReadEntityFile<Location>(commandLineOptions.LocationFilePath),
                Sessions = MappedCsvFileReader.ReadEntityFile<Session>(commandLineOptions.SessionFilePath)
            };
        }
    }
}
