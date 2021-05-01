using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class GraduationPlanTemplate : IGraduationPlanTemplate
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("Type")]
        public string GraduationPlanType { get; set; }

        [XmlAttribute]
        public int TotalCreditsRequired { get; set; }

        public GraduationPlanTypeDescriptor GetGraduationPlanTypeDescriptor()
        {
            return GraduationPlanType.ToDescriptorFromCodeValue<GraduationPlanTypeDescriptor>();
        }
    }

    public class GraduationPlanTemplateReference : IGraduationPlanTemplateReference
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}
