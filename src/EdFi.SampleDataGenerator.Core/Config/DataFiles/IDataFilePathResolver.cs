using System;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public interface IDataFilePathResolver
    {
        void SetBasePath(string basePath);
        string GetPathForSurnameFile(string ethnicity);
        string GetPathForFirstNameFile(string ethnicity, string gender);
        string GetPathForStreeNameFile();
        string GetPathForInterchangeType(Type interchangeType);
        string GetPathForInterchangeEntityTypeFile(Type interchangeType, Type entityType);
    }
}