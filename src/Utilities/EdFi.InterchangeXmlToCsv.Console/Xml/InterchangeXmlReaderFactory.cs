using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public static class InterchangeXmlReaderFactory
    {
        public static IInterchangeXmlReader BuildXmlReader(string interchangeName)
        {
            var className = $"{interchangeName}InterchangeXmlReader";
            var readerType = GetXmlReaders().SingleOrDefault(r => r.Name.Equals(className, StringComparison.OrdinalIgnoreCase));

            if (readerType == null)
                throw new InvalidOperationException($"No Xml reader found for interchange '{interchangeName}'");

            return (IInterchangeXmlReader) Activator.CreateInstance(readerType);
        }
        
        private static IEnumerable<Type> GetXmlReaders()
        {
            var attributeGeneratorBaseType = typeof(IInterchangeXmlReader);
            var attributeGeneratorTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => attributeGeneratorBaseType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            return attributeGeneratorTypes;
        }
    }
}
