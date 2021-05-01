using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class StudentProfile : IStudentProfile
    {
        [XmlAttribute]
        public string Name { get; set; }
        
        [XmlElement("RaceConfiguration")]
        public RaceConfiguration RaceConfiguration { get; set; }
        IAttributeConfiguration IStudentProfile.RaceConfiguration => RaceConfiguration;

        [XmlElement("SexConfiguration")]
        public SexConfiguration SexConfiguration { get; set; }
        IAttributeConfiguration IStudentProfile.SexConfiguration => SexConfiguration;

        [XmlElement("EconomicDisadvantageConfiguration")]
        public EconomicDisadvantageConfiguration EconomicDisadvantageConfiguration { get; set; }
        IAttributeConfiguration IStudentProfile.EconomicDisadvantageConfiguration => EconomicDisadvantageConfiguration;

        [XmlElement("HomelessStatusConfiguration")]
        public HomelessConfiguration HomelessStatusConfiguration { get; set; }
        IAttributeConfiguration IStudentProfile.HomelessStatusConfiguration => HomelessStatusConfiguration;

        [XmlElement("ImmigrantProfile")]
        public ImmigrantPopulationProfile ImmigrantPopulationProfile { get; set; }
        IImmigrantPopulationProfile IStudentProfile.ImmigrantPopulationProfile => ImmigrantPopulationProfile;
    }

    public abstract class AttributeConfiguration : IAttributeConfiguration
    {
        public abstract string Name { get; }

        [XmlElement(ElementName = "Option")]
        public AttributeGeneratorConfigurationOption[] AttributeGeneratorConfigurationOptions { get; set; }
        IAttributeGeneratorConfigurationOption[] IAttributeConfiguration.AttributeGeneratorConfigurationOptions => AttributeGeneratorConfigurationOptions;
    }
    
    public class AttributeGeneratorConfigurationOption : IAttributeGeneratorConfigurationOption
    {
        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public double Frequency { get; set; }
    }
}
