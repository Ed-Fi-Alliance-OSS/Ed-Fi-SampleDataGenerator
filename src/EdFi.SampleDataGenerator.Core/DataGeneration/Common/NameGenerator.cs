using System;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Entities;
using log4net;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class NameGenerator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NameGenerator));

        public static Name Generate(NameFileData nameFileData, IRandomNumberGenerator randomNumberGenerator, SexDescriptor sex, IEthnicityMapping ethnicityMapping)
        {
            try
            {
               
                var firstNameFile = nameFileData.FirstNameFiles[ethnicityMapping, sex];
                var surnameFile = nameFileData.SurnameFiles[ethnicityMapping];

                var firstNameIndex = randomNumberGenerator.Generate(0, firstNameFile.FileRecords.Length);
                var surnameIndex = randomNumberGenerator.Generate(0, surnameFile.FileRecords.Length);

                return new Name
                {
                    FirstName = firstNameFile.FileRecords[firstNameIndex].Name,
                    LastSurname = surnameFile.FileRecords[surnameIndex].Name
                };
            }
            catch (Exception e)
            {
                Log.Debug($"Error while trying to generate name for Ethnicity={ethnicityMapping}, Sex={sex}", e);
                throw;
            }
        }
    }
}
