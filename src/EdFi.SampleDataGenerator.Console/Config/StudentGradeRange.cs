using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class StudentGradeRange : IStudentGradeRange
    {
        public double LowerPerformanceIndex { get; set; }
        public double UpperPerformanceIndex { get; set; }
        public int MinNumericGrade { get; set; }
        public int MaxNumericGrade { get; set; }
    }
}
