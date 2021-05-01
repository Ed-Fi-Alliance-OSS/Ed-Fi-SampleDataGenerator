using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.EducationOrganizationGenerator.Console.Configuration
{
    public class ConfigurationSnippets
    {
        [XmlElement("DistrictProfileConfigurationSnippet")]
        public DistrictProfileConfigurationSnippet DistrictProfileConfigurationSnippet { get; set; }

        [XmlElement("SchoolProfileConfigurationSnippet")]
        public SchoolProfileConfigurationSnippet[] SchoolProfileConfigSnippets { get; set; }

        [XmlElement("GradeProfileConfigurationSnippet")]
        public GradeProfileConfigurationSnippet[] GradeProfileConfigSnippets { get; set; }
    }

    public class DistrictProfileConfigurationSnippet
    {
        [XmlText]
        public string Data { get; set; }
    }

    public class SchoolProfileConfigurationSnippet
    {
        [XmlAttribute]
        public string SchoolType { get; set; }

        [XmlText]
        public string Data { get; set; }
    }

    public class GradeProfileConfigurationSnippet
    {
        [XmlAttribute]
        public string SchoolType { get; set; }

        [XmlAttribute]
        public int MinStudents { get; set; }

        [XmlAttribute]
        public int MaxStudents { get; set; }

        [XmlText]
        public string Data { get; set; }
    }
}
