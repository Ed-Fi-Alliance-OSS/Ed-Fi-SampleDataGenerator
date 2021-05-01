using EdFi.SampleDataGenerator.Core.Serialization.Output;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class StudentPerformanceProfileOutputTester
    {
        [Test]
        public void ShouldBuildValidOutputXml()
        {
            var spOutput = new StudentPerformanceProfileOutput();

            spOutput.Add("000016", 0.28830903736831892);
            spOutput.Add("000017", 0.56956021043894056);

            spOutput.ToXml().ToString().ShouldBe(Expectation);
        }

        private static string Expectation => @"
<StudentPerformanceProfile>
  <Student>
    <StudentUniqueId>000016</StudentUniqueId>
    <PerformanceIndex>0.28830903736831892</PerformanceIndex>
  </Student>
  <Student>
    <StudentUniqueId>000017</StudentUniqueId>
    <PerformanceIndex>0.56956021043894056</PerformanceIndex>
  </Student>
</StudentPerformanceProfile>".Trim();
    }
}
