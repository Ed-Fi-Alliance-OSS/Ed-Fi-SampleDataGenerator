using System;
using System.IO;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class DescriptorFileReader : IDescriptorFileReader
    {
        public DescriptorData Read(IDescriptorFileMapping fileMapping)
        {
            using (var textReader = new StreamReader(fileMapping.FilePath))
            {
                var descriptorType = DescriptorHelpers.DescriptorTypeFromName(fileMapping.DescriptorName);
                if (descriptorType == null)
                {
                    throw new InvalidOperationException($"No DescriptorType found with name '{fileMapping.DescriptorName}'");
                }

                var classMap = CsvClassMapRegistry.MapFor(descriptorType);

                if (classMap == null)
                {
                    throw new InvalidOperationException($"No CsvClassMap defined for type {descriptorType.Name}");
                }

                try
                {
                    var csvReader = new InterchangeCsvReader(classMap, textReader);
                    var descriptorRecords = csvReader.ReadRecords(descriptorType);

                    return new DescriptorData
                    {
                        DescriptorName = fileMapping.DescriptorName,
                        DescriptorType = descriptorType,
                        Descriptors = descriptorRecords.Select(r => r as DescriptorType).ToList()
                    };
                }
                catch (Exception e)
                {
                    throw new Exception($"Error while parsing '{fileMapping.FilePath}' for type {descriptorType.Name}.  See InnerException for details.", e);
                }
            }
        }
    }
}
