using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public class InterchangeItemCollection
    {
        private readonly IEnumerable<object> _interchangeItems;

        public InterchangeItemCollection(IEnumerable<object> items)
        {
            _interchangeItems = items;
        }

        public IEnumerable<Type> GetEntityTypesInCollection()
        {
            return _interchangeItems.Select(i => i.GetType()).Distinct();
        }

        public IEnumerable<object> this[Type type]
        {
            get { return _interchangeItems.Where(i => i.GetType() == type); }
        }
    }
}