using System;
using System.IO;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Core.Config.Xml
{
    public static class XmlConfigHelpers
    {
        public static T ParseConfigFileToObject<T>(string xmlFilename)
        {
            if (string.IsNullOrEmpty(xmlFilename))
            {
                throw new ArgumentException("File name required", nameof(xmlFilename));
            }

            using (var xmlStream = new StreamReader(xmlFilename))
            {
                var serializer = new XmlSerializer(typeof(T));
                var result = (T)serializer.Deserialize(xmlStream);

                return result;
            }
        }

        public static void WriteConfigFileFromObject<T>(T configObject, string xmlFilename)
        {
            if (string.IsNullOrEmpty(xmlFilename))
            {
                throw new ArgumentException("File name required", nameof(xmlFilename));
            }

            using (var xmlStream = new StreamWriter(xmlFilename))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(xmlStream, configObject);
            }
        }
    }
}
