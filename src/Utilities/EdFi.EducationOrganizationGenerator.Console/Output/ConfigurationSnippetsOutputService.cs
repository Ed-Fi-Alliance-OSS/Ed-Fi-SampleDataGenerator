using System.IO;
using EdFi.EducationOrganizationGenerator.Console.Configuration;

namespace EdFi.EducationOrganizationGenerator.Console.Output
{
    public static class ConfigurationSnippetsOutputService
    {
        public static void WriteOutputFile(string outputPath, DistrictProfile districtProfile, string configSnippet)
        {
            var xmlSnippetFileName = $"{districtProfile.DistrictName}_DistrictProfileConfig.xml";
            var outputFileName = Path.Combine(outputPath, xmlSnippetFileName);

            File.WriteAllText(outputFileName, configSnippet);
        }
    }
}
