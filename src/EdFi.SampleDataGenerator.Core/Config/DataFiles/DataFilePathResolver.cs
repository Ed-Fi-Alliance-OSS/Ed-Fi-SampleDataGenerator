using System;
using System.IO;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class DataFilePathResolver : IDataFilePathResolver
    {
        private const string InterchangeText = "Interchange";
        private string _basePath;

        public void SetBasePath(string basePath)
        {
            _basePath = basePath;
        }

        public string GetPathForSurnameFile(string ethnicity)
        {
            return Path.Combine(_basePath, $"Surname-{ethnicity}.csv");
        }

        public string GetPathForFirstNameFile(string ethnicity, string gender)
        {
            return Path.Combine(_basePath, $"FirstName-{ethnicity}-{gender}.csv");
        }

        public string GetPathForStreeNameFile()
        {
            return Path.Combine(_basePath, "StreetNames.csv");
        }
        
        public string GetPathForInterchangeType(Type interchangeType)
        {
            if (!interchangeType.Name.StartsWith(InterchangeText)) throw new InvalidOperationException($"Type {interchangeType.Name} does not appear to be an Interchange type");
            var interchangeName = interchangeType.Name.Replace(InterchangeText, "");

            return Path.Combine(_basePath, $"{interchangeName}");
        }
        
        public string GetPathForInterchangeEntityTypeFile(Type interchangeType, Type entityType)
        {
            if (!interchangeType.Name.StartsWith(InterchangeText)) throw new InvalidOperationException($"Type {interchangeType.Name} does not appear to be an Interchange type");
            var interchangeName = interchangeType.Name.Replace(InterchangeText, "");
            var typeName = entityType.Name;

            return Path.Combine(_basePath, $"{interchangeName}", $"{typeName}.csv");
        }
    }
}