using System;
using System.IO;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config.DataFiles
{
    [TestFixture]
    public class NameFileReaderTester
    {
        [Test]
        public void ShouldReadNameFile()
        {
            var testContent = @"Name,Frequency
Test1,0.1
Test2,0.2
Test3,0.3
";
            WriteTestFile(testContent);

            var sut = new NameFileReader();
            var records = sut.Read(TestFilePath);

            records.Count().ShouldBe(3);
            records.First().Frequency.ShouldBe(0.1);
        }

        [Test]
        public void ShouldThrowOnBadData()
        {
            var testContent = @"Name,Frequency
Test1,A
";
            WriteTestFile(testContent);

            var sut = new NameFileReader();
            Assert.Throws<InvalidDataException>(() => sut.Read(TestFilePath));
        }

        [Test]
        public void ShouldThrowOnMissingHeader()
        {
            var testContent = @"Test1,0.1
Test2,0.2
Test3,0.3
";
            WriteTestFile(testContent);

            var sut = new NameFileReader();
            Assert.Throws<InvalidDataException>(() => sut.Read(TestFilePath));
        }

        private void WriteTestFile(string content)
        {
            using (var sw = new StreamWriter(Path.Combine(AssemblyDirectory, TestFilePath)))
            {
                sw.Write(content);
            }
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static string TestFilePath => Path.Combine(AssemblyDirectory, TestFileName);
        private static string TestFileName = "Testfile.txt";
    }
}
