using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class AssessmentMetadataFileReader : InterchangeFileReaderBase<AssessmentMetadataData>
    {
        public override AssessmentMetadataData Read(ISampleDataGeneratorConfig config)
        {
            var data = new AssessmentMetadataData
            {
                Assessments = ReadEntityFile<Assessment>(config)
            };

            RemoveBlankAssessedGradeLevels(data.Assessments);

            return data;
        }

        protected override Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc => config => config.DataFileConfig.AssessmentMetadataFiles;

        private static void RemoveBlankAssessedGradeLevels(List<Assessment> assessments)
        {
            // CsvHelper cannot be configured to omit blank items for a string array
            // property, even when using its ConvertUsing and TypeConverter features,
            // so we remove blanks immediately after reading the CSV.

            foreach (var assessment in assessments)
            {
                assessment.AssessedGradeLevel =
                    assessment.AssessedGradeLevel.Where(x => !String.IsNullOrWhiteSpace(x))
                        .ToArray();
            }
        }
    }
}
