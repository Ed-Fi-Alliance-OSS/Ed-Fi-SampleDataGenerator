using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class StandardsInterchangeXmlReader : InterchangeXmlReader<InterchangeStandards>
    {
        protected override InterchangeItemCollection GetInterchangeItems(InterchangeStandards rootInterchangeItem)
        {
            return new InterchangeItemCollection(rootInterchangeItem.Items);
        }
    }
}
