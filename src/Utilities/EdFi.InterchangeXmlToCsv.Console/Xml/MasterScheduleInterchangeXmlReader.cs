using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class MasterScheduleInterchangeXmlReader : InterchangeXmlReader<InterchangeMasterSchedule>
    {
        protected override InterchangeItemCollection GetInterchangeItems(InterchangeMasterSchedule rootInterchangeItem)
        {
            return new InterchangeItemCollection(rootInterchangeItem.Items);
        }
    }
}