using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class ManifestTester
    {
        [Test]
        public void ShouldBuildValidManifestXml()
        {
            var manifest = new Manifest();

            manifest.Add(Interchange.Standards, "Standards.xml");
            manifest.Add(Interchange.EducationOrganization, "EducationOrganization.xml");

            manifest.ToXml().ToString().StripLineEndings().ShouldBe(Expectation.StripLineEndings());
        }

        private static string Expectation => @"
<Interchanges>
  <Interchange>
    <Filename>Standards.xml</Filename>
    <Type>Standards</Type>
  </Interchange>
  <Interchange>
    <Filename>EducationOrganization.xml</Filename>
    <Type>EducationOrganization</Type>
  </Interchange>
</Interchanges>".Trim();
    }
}
