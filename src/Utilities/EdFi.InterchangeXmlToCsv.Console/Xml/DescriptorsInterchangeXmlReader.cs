using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class DescriptorsInterchangeXmlReader : InterchangeXmlReader<InterchangeDescriptors>
    {
        protected override InterchangeItemCollection GetInterchangeItems(InterchangeDescriptors rootInterchangeItem)
        {
            return new InterchangeItemCollection(rootInterchangeItem.Items);
        }
    }
}