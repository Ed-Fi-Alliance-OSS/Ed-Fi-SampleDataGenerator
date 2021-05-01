using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class StandardsFileReader : InterchangeFileReaderBase<StandardsData>
    {
        public override StandardsData Read(ISampleDataGeneratorConfig config)
        {
            var standardsData = new StandardsData
            {
                LearningStandards = ReadEntityFile<LearningStandard>(config),
                LearningObjectives = ReadEntityFile<LearningObjective>(config)
            };

            return standardsData;
        }

        protected override Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc => config => config.DataFileConfig.StandardsFiles;
    }
}
