using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface IStudentPerformanceProfileOutput
    {
        XDocument ToXml();
        void Add(string studentUniqueId, double performanceIndex);
        void FlushOutput(string fileName);
    }

    public class StudentPerformanceProfileOutput : IStudentPerformanceProfileOutput
    {
        private class StudentPerformanceProfileData
        {
            public StudentPerformanceProfileData(string studentUniqueId, double performanceIndex)
            {
                StudentUniqueId = studentUniqueId;
                PerformanceIndex = performanceIndex;
            }

            public string StudentUniqueId { get; }
            public double PerformanceIndex { get; }
        }

        private List<StudentPerformanceProfileData> _items = new List<StudentPerformanceProfileData>();

        public void Add(string studentUniqueId, double performanceIndex)
        {
            _items.Add(new StudentPerformanceProfileData(studentUniqueId, performanceIndex));
        }

        public XDocument ToXml()
        {
            return new XDocument(
                new XElement("StudentPerformanceProfile",
                    _items.ToList().Select(item =>
                        new XElement("Student",
                            new XElement("StudentUniqueId", item.StudentUniqueId),
                            new XElement("PerformanceIndex", item.PerformanceIndex))
                            )));
        }

        public void FlushOutput(string fileName)
        {
            if (_items.Any())
                ToXml().Save(fileName);
            _items = new List<StudentPerformanceProfileData>();
        }
    }
}
