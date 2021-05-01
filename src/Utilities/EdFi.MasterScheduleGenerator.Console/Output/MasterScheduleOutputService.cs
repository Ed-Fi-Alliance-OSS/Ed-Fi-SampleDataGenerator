using System.Collections.Generic;
using System.IO;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.MasterScheduleGenerator.Console.Output
{
    public static class MasterScheduleOutputService
    {
        public static void WriteOutputFiles(string outputPath, MasterScheduleData data)
        {
            WriteOutputFile(outputPath, data.CourseOfferings);
            WriteOutputFile(outputPath, data.Sections);
        }

        public static void WriteOutputFile<TEntity>(string outputPath, IEnumerable<TEntity> entities)
        {
            var fileName = GetOutputFileName<TEntity>(outputPath);
            MappedCsvFileWriter.WriteEntityFile(fileName, entities);
        }

        private static string GetOutputFileName<TEntity>(string outputPath)
        {
            return Path.Combine(outputPath, $"{typeof(TEntity).Name}.csv");
        }
    }
}
