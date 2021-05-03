using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class GradeLevelHelpers
    {
        public static int GetNumericGradeLevel(this string grade)
        {
            return grade.ParseFromStructuredCodeValue<GradeLevelDescriptor>().GetNumericGradeLevel();
        }

        public static int GetNumericGradeLevel(this GradeLevelDescriptor grade)
        {
            if (grade.CodeValue == GradeLevelDescriptor.Kindergarten.CodeValue) return 0;
            if (grade.CodeValue == GradeLevelDescriptor.FirstGrade.CodeValue) return 1;
            if (grade.CodeValue == GradeLevelDescriptor.SecondGrade.CodeValue) return 2;
            if (grade.CodeValue == GradeLevelDescriptor.ThirdGrade.CodeValue) return 3;
            if (grade.CodeValue == GradeLevelDescriptor.FourthGrade.CodeValue) return 4;
            if (grade.CodeValue == GradeLevelDescriptor.FifthGrade.CodeValue) return 5;
            if (grade.CodeValue == GradeLevelDescriptor.SixthGrade.CodeValue) return 6;
            if (grade.CodeValue == GradeLevelDescriptor.SeventhGrade.CodeValue) return 7;
            if (grade.CodeValue == GradeLevelDescriptor.EighthGrade.CodeValue) return 8;
            if (grade.CodeValue == GradeLevelDescriptor.NinthGrade.CodeValue) return 9;
            if (grade.CodeValue == GradeLevelDescriptor.TenthGrade.CodeValue) return 10;
            if (grade.CodeValue == GradeLevelDescriptor.EleventhGrade.CodeValue) return 11;
            if (grade.CodeValue == GradeLevelDescriptor.TwelfthGrade.CodeValue) return 12;

            return 99;
        }

        /// <summary>
        /// For Grades K-11, get the subsequent Grade level and return true.
        /// For any other Grade, return false.
        /// </summary>
        public static bool TryGetNextK12GradeLevel(this GradeLevelDescriptor grade, out GradeLevelDescriptor nextGrade)
        {
            nextGrade = grade.GetNextK12GradeLevel();

            return nextGrade != null;
        }

        private static GradeLevelDescriptor GetNextK12GradeLevel(this GradeLevelDescriptor grade)
        {
            if (grade.CodeValue == GradeLevelDescriptor.Kindergarten.CodeValue) return GradeLevelDescriptor.FirstGrade;
            if (grade.CodeValue == GradeLevelDescriptor.FirstGrade.CodeValue) return GradeLevelDescriptor.SecondGrade;
            if (grade.CodeValue == GradeLevelDescriptor.SecondGrade.CodeValue) return GradeLevelDescriptor.ThirdGrade;
            if (grade.CodeValue == GradeLevelDescriptor.ThirdGrade.CodeValue) return GradeLevelDescriptor.FourthGrade;
            if (grade.CodeValue == GradeLevelDescriptor.FourthGrade.CodeValue) return GradeLevelDescriptor.FifthGrade;
            if (grade.CodeValue == GradeLevelDescriptor.FifthGrade.CodeValue) return GradeLevelDescriptor.SixthGrade;
            if (grade.CodeValue == GradeLevelDescriptor.SixthGrade.CodeValue) return GradeLevelDescriptor.SeventhGrade;
            if (grade.CodeValue == GradeLevelDescriptor.SeventhGrade.CodeValue) return GradeLevelDescriptor.EighthGrade;
            if (grade.CodeValue == GradeLevelDescriptor.EighthGrade.CodeValue) return GradeLevelDescriptor.NinthGrade;
            if (grade.CodeValue == GradeLevelDescriptor.NinthGrade.CodeValue) return GradeLevelDescriptor.TenthGrade;
            if (grade.CodeValue == GradeLevelDescriptor.TenthGrade.CodeValue) return GradeLevelDescriptor.EleventhGrade;
            if (grade.CodeValue == GradeLevelDescriptor.EleventhGrade.CodeValue) return GradeLevelDescriptor.TwelfthGrade;

            return null;
        }

        public static int GetStudentAgeAtStartOfSchoolYear(this GradeLevelDescriptor grade)
        {

            if (grade.CodeValue == GradeLevelDescriptor.Kindergarten.CodeValue) return 5;
            if (grade.CodeValue == GradeLevelDescriptor.FirstGrade.CodeValue) return 6;
            if (grade.CodeValue == GradeLevelDescriptor.SecondGrade.CodeValue) return 7;
            if (grade.CodeValue == GradeLevelDescriptor.ThirdGrade.CodeValue) return 8;
            if (grade.CodeValue == GradeLevelDescriptor.FourthGrade.CodeValue) return 9;
            if (grade.CodeValue == GradeLevelDescriptor.FifthGrade.CodeValue) return 10;
            if (grade.CodeValue == GradeLevelDescriptor.SixthGrade.CodeValue) return 11;
            if (grade.CodeValue == GradeLevelDescriptor.SeventhGrade.CodeValue) return 12;
            if (grade.CodeValue == GradeLevelDescriptor.EighthGrade.CodeValue) return 13;
            if (grade.CodeValue == GradeLevelDescriptor.NinthGrade.CodeValue) return 14;
            if (grade.CodeValue == GradeLevelDescriptor.TenthGrade.CodeValue) return 15;
            if (grade.CodeValue == GradeLevelDescriptor.EleventhGrade.CodeValue) return 16;
            if (grade.CodeValue == GradeLevelDescriptor.TwelfthGrade.CodeValue) return 17;

            return 0;
        }

        public static int GetNumberOfSchoolYearsUntilGraduation(this GradeLevelDescriptor grade)
        {
            if (grade.CodeValue == GradeLevelDescriptor.Kindergarten.CodeValue) return 12;
            if (grade.CodeValue == GradeLevelDescriptor.FirstGrade.CodeValue) return 11;
            if (grade.CodeValue == GradeLevelDescriptor.SecondGrade.CodeValue) return 10;
            if (grade.CodeValue == GradeLevelDescriptor.ThirdGrade.CodeValue) return 9;
            if (grade.CodeValue == GradeLevelDescriptor.FourthGrade.CodeValue) return 8;
            if (grade.CodeValue == GradeLevelDescriptor.FifthGrade.CodeValue) return 7;
            if (grade.CodeValue == GradeLevelDescriptor.SixthGrade.CodeValue) return 6;
            if (grade.CodeValue == GradeLevelDescriptor.SeventhGrade.CodeValue) return 5;
            if (grade.CodeValue == GradeLevelDescriptor.EighthGrade.CodeValue) return 4;
            if (grade.CodeValue == GradeLevelDescriptor.NinthGrade.CodeValue) return 3;
            if (grade.CodeValue == GradeLevelDescriptor.TenthGrade.CodeValue) return 2;
            if (grade.CodeValue == GradeLevelDescriptor.EleventhGrade.CodeValue) return 1;
            if (grade.CodeValue == GradeLevelDescriptor.TwelfthGrade.CodeValue) return 0;

            return 0;
        }
    }
}
