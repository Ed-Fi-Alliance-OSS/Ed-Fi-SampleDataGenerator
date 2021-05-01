using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class AssessmentMetadataInterchangeXmlReader : InterchangeXmlReader<InterchangeAssessmentMetadata>
    {
        protected override InterchangeItemCollection GetInterchangeItems(InterchangeAssessmentMetadata rootInterchangeItem)
        {
            return new InterchangeItemCollection(rootInterchangeItem.Items);
        }
    }
}
