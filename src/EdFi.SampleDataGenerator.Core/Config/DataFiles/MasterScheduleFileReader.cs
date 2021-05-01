using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class MasterScheduleFileReader : InterchangeFileReaderBase<MasterScheduleData>
    {
        public override MasterScheduleData Read(ISampleDataGeneratorConfig config)
        {
            var data = new MasterScheduleData
            {
                CourseOfferings = ReadEntityFile<CourseOffering>(config),
                Sections = ReadEntityFile<Section>(config)
            };

            return data;
        }

        protected override Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc => config => config.DataFileConfig.MasterScheduleFiles;
    }
}