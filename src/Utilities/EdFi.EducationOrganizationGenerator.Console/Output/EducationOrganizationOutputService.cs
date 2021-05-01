using System.Collections.Generic;
using System.IO;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Output
{
    public static class EducationOrganizationOutputService
    {
        public static void WriteOutputFiles(string outputPath, EducationOrganizationData data)
        {
            WriteOutputFile(outputPath, data.AccountabilityRatings);
            WriteOutputFile(outputPath, data.ClassPeriods);
            WriteOutputFile(outputPath, data.Courses);
            WriteOutputFile(outputPath, data.EducationServiceCenters);
            WriteOutputFile(outputPath, data.LocalEducationAgencies);
            WriteOutputFile(outputPath, data.Locations);
            WriteOutputFile(outputPath, data.Programs);
            WriteOutputFile(outputPath, data.Schools);
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
