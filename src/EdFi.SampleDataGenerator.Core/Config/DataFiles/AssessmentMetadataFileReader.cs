using System;
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

            return data;
        }

        protected override Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc => config => config.DataFileConfig.AssessmentMetadataFiles;
    }
}
