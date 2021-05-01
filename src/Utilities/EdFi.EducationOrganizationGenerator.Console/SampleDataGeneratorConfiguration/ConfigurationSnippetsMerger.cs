using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using EdFi.EducationOrganizationGenerator.Console.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.SampleDataGeneratorConfiguration
{
    public class ConfigurationSnippetsMerger
    {
        private static readonly Dictionary<string, Func<DistrictProfile, string>> DistrictProfileReplacements = new Dictionary <string, Func<DistrictProfile, string>>
        {
            { "{DistrictName}", p => p.DistrictName},
            { "{StateId}", p => p.StateId },
            { "{State}", p => p.LocationInfo.State },
            { "{City}", p => string.Join("\r\n", p.LocationInfo.Cities.Select(CreateCityXml)) },
        }; 

        private static readonly Dictionary<string, Func<School, string>> SchoolProfileReplacements = new Dictionary<string, Func<School, string>>
        {
            { "{SchoolId}", s => s.SchoolId.ToString() },
            { "{SchoolName}", s => s.NameOfInstitution },
        };


        private static readonly Dictionary<string, Func<GradeProfile, string>> GradeProfileReplacements = new Dictionary<string, Func<GradeProfile, string>>
        {
            { "{GradeLevel}", p => p.GradeLevel },
            { "{InitialStudentCount}", p => p.InitialStudentCount.ToString() }
        };

        public string Merge(ConfigurationSnippets snippets, EducationOrganizationGeneratorConfig generatorConfig, EducationOrganizationData educationOrganizationData)
        {
            var result = snippets.DistrictProfileConfigurationSnippet.Data;
            foreach (var replacement in DistrictProfileReplacements.Keys)
            {
                var replacementFunc = DistrictProfileReplacements[replacement];
                result = result.Replace(replacement, replacementFunc(generatorConfig.DistrictProfile));
            }

            var schoolNodes = new StringBuilder();
            foreach (var school in educationOrganizationData.Schools)
            {
                var schoolType = school.SchoolCategory.First();
                var schoolSnippet = snippets.SchoolProfileConfigSnippets.First(s => s.SchoolType == schoolType);
                var gradeSnippet = snippets.GradeProfileConfigSnippets.First(s => s.SchoolType == schoolType);
                var schoolProfile = generatorConfig.DistrictProfile.SchoolProfiles.First(sp => sp.SchoolType == schoolType);

                schoolNodes.AppendLine(MergeSchoolProfile(schoolSnippet, schoolProfile, school, gradeSnippet));
            }

            result = result.Replace("{SchoolProfile}", schoolNodes.ToString());

            return result;
        }

        private static string MergeSchoolProfile(SchoolProfileConfigurationSnippet schoolSnippet, SchoolProfile profile, School school, GradeProfileConfigurationSnippet gradeSnippet)
        {
            var result = schoolSnippet.Data;
            foreach (var replacment in SchoolProfileReplacements.Keys)
            {
                var replacementFunc = SchoolProfileReplacements[replacment];
                result = result.Replace(replacment, replacementFunc(school));
            }

            var gradeNodes = new StringBuilder();
            foreach (var gradeProfile in profile.GradeProfiles)
            {
                gradeNodes.AppendLine(MergeGradeProfile(gradeSnippet, gradeProfile));
            }

            result = result.Replace("{GradeProfile}", gradeNodes.ToString());

            return result;
        }

        private static string MergeGradeProfile(GradeProfileConfigurationSnippet snippet, GradeProfile profile)
        {
            var result = snippet.Data;
            foreach (var replacment in GradeProfileReplacements.Keys)
            {
                var replacementFunc = GradeProfileReplacements[replacment];
                result = result.Replace(replacment, replacementFunc(profile));
            }

            return result;
        }
        
        private static string CreateCityXml(City city)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty, });
            var xmlSettings = new XmlWriterSettings { OmitXmlDeclaration = true};

            var serializer = new XmlSerializer(typeof(City));
            using (var sw = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(sw, xmlSettings))
                {
                    serializer.Serialize(xmlWriter, city, emptyNamespaces);
                    return sw.ToString();
                }
            }
        }
    }
}
