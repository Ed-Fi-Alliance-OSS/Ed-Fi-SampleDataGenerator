using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class EducationOrganizationInterchangeXmlReader : InterchangeXmlReader<InterchangeEducationOrganization>
    {
        protected override InterchangeItemCollection GetInterchangeItems(InterchangeEducationOrganization rootInterchangeItem)
        {
            return new InterchangeItemCollection(rootInterchangeItem.Items);
        }
    }
}
