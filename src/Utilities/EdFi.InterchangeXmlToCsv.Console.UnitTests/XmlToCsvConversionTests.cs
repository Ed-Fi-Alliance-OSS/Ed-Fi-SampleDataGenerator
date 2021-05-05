using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace EdFi.InterchangeXmlToCsv.Console.UnitTests
{
    [TestFixture, Explicit]
    public class XmlToCsvConversionTests
    {
        private static readonly string SampleXmlFolderName = Path.Combine(AssemblyDirectory, "Sample XML");
        private static readonly string ExpectedCsvOutputFolderName = Path.Combine(AssemblyDirectory, "Expected CSV Output");
        private static readonly string ActualCsvOutputFolderName = Path.Combine(AssemblyDirectory, "Actual CSV Output");

        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }


        [OneTimeSetUp]
        public void Setup()
        {
            DeleteDirectory(ActualCsvOutputFolderName);
            Directory.CreateDirectory(ActualCsvOutputFolderName);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            DeleteDirectory(ActualCsvOutputFolderName);
        }

        [Test,TestCaseSource(nameof(GetInputDirectories))]
        public void XmlToCsvConversionTest(string inputFileName)
        {
            XmlShouldGenerateExpectedOutput(inputFileName, GetInterchangeName(inputFileName));
        }

        private static string[] GetInputDirectories()
        {
            return Directory.GetDirectories(SampleXmlFolderName);
        }

        private static string GetInterchangeName(string inputPath)
        {
            return Path.GetFileNameWithoutExtension(inputPath);
        }

        private static void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                string[] dirs = Directory.GetDirectories(directoryPath);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(directoryPath, true);
            }
        }

        private static void XmlShouldGenerateExpectedOutput(string xmlDirectory, string interchangeName)
        {
            var interchangeXmlToCsvConfig = new InterchangeXmlToCsvConfig
            {
                InputPath = xmlDirectory,
                InterchangeName = interchangeName,
                OutputPath = Path.Combine(Directory.GetCurrentDirectory(), ActualCsvOutputFolderName),
                Recurse = true
            };

            new InterchangeXmlToCsvConverter().Convert(interchangeXmlToCsvConfig);

            var expectedOutputPath = Path.Combine(ExpectedCsvOutputFolderName, interchangeName);
            var expectedCsvOutputFiles = Directory.GetFiles(expectedOutputPath, $"*.csv");
            foreach (var expectedCsvOutput in expectedCsvOutputFiles)
            {
                var fileName = Path.GetFileName(expectedCsvOutput);
                var expectedOutput = File.ReadAllLines(expectedCsvOutput);
                var actualOutput = File.ReadAllLines(Path.Combine(ActualCsvOutputFolderName, interchangeName, fileName));

                for (var i = 0; i < expectedOutput.Length; i++)
                {
                    actualOutput[i].ShouldContainWithoutWhitespace(expectedOutput[i],$"Interchange: {interchangeName}, CSV File: {fileName}, Line: {i+1}");
                }
            }
        }
    }
}
