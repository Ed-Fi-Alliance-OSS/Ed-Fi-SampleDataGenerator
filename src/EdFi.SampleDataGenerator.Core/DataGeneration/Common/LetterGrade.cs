using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using Headspring;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public sealed class LetterGrade : Enumeration<LetterGrade>
    {
        public string Grade { get; }
        public int MinNumericGrade { get; }
        public int MaxNumericGrade { get; }

        private LetterGrade(int value, string letterGrade, int minNumericGrade, int maxNumericGrade) : base(value, letterGrade)
        {
            Grade = letterGrade;
            MaxNumericGrade = maxNumericGrade;
            MinNumericGrade = minNumericGrade;
        }

        public int GetMaxGradePoints(int coursesTaken)
        {
            return MaxNumericGrade*coursesTaken;
        }

        public static LetterGrade A = new LetterGrade(4, "A", 90, 100);
        public static LetterGrade B = new LetterGrade(3, "B", 80, 89);
        public static LetterGrade C = new LetterGrade(2, "C", 70, 79);
        public static LetterGrade D = new LetterGrade(1, "D", 60, 69);
        public static LetterGrade F = new LetterGrade(0, "F", 0, 59);

        public static LetterGrade GetLetterGradeFromNumericGrade(int numericGrade)
        {
            var letterGrades = GetAll();
            return letterGrades.First(g => g.MinNumericGrade <= numericGrade && g.MaxNumericGrade >= numericGrade);
        }

        public PerformanceBaseConversionDescriptor ToPerformanceBaseConversionDescriptor()
        {
            switch (Grade)
            {
                case "A":
                    return PerformanceBaseConversionDescriptor.Advanced;
                case "B":
                    return PerformanceBaseConversionDescriptor.Proficient;
                case "C":
                    return PerformanceBaseConversionDescriptor.Basic;
                case "D":
                    return PerformanceBaseConversionDescriptor.BelowBasic;
                default:
                    return PerformanceBaseConversionDescriptor.Fail;
            }
        }
    }
}
