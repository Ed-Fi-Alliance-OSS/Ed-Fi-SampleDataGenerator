using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class NameFileReader : INameFileReader
    {
        public IEnumerable<NameFileRecord> Read(string fileName)
        {
            return ReadFileRecords(fileName);
        }

        public static IEnumerable<NameFileRecord> ReadFileRecords(string fileName)
        {
            using (var textReader = new StreamReader(fileName))
            {
                using (var csvReader = new CsvFactory().CreateReader(textReader))
                {
                    try
                    {
                        return csvReader.GetRecords<NameFileRecord>().ToList();
                    }
                    catch (CsvMissingFieldException)
                    {
                        throw new InvalidDataException("Name file must include a header");
                    }

                    catch (CsvTypeConverterException)
                    {
                        throw new InvalidDataException("Name file format is not correct: File should be CSV with Name,Frequency fields");
                    }
                }
            }
        }
    }
}