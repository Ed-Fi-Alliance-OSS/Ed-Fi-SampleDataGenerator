using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Core.Entities
{
    public partial class InterchangeStudent
    {
        public InterchangeStudent()
        {
            xsiSchemaLocation = GetSchemaLocation();
        }

        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation;

        private string GetSchemaLocation()
        {
            var xmlRootAttribute = (XmlRootAttribute) Attribute.GetCustomAttribute(this.GetType(),  typeof (XmlRootAttribute));
            return $"{xmlRootAttribute.Namespace} ../../Schemas/Interchange-Student.xsd";
        }
    }
}
