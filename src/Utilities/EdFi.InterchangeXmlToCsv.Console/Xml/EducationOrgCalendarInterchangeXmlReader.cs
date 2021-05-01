using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class EducationOrgCalendarInterchangeXmlReader : InterchangeXmlReader<InterchangeEducationOrgCalendar>
    {
        protected override InterchangeItemCollection GetInterchangeItems(InterchangeEducationOrgCalendar rootInterchangeItem)
        {
            return new InterchangeItemCollection(rootInterchangeItem.Items);
        }
    }
}