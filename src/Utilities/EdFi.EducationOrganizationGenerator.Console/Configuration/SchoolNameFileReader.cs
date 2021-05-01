using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EdFi.EducationOrganizationGenerator.Console.Configuration
{
    public static class SchoolNameFileReader
    {
        public static List<string> Read(string inputFile)
        {
            var fileContents = File.ReadAllLines(inputFile, Encoding.UTF8).Select(l => l.Trim());
            return fileContents.ToList();
        }
    }
}
