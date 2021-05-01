using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentGradeEntity : Entity
    {
        private StudentGradeEntity(Type entityType) : base(entityType, Interchange.StudentGrade)
        {
        }

        public static readonly StudentGradeEntity CompetencyObjective = new StudentGradeEntity(typeof(CompetencyObjective));
        public static readonly StudentGradeEntity Grade = new StudentGradeEntity(typeof(Grade));
        public static readonly StudentGradeEntity LearningObjective = new StudentGradeEntity(typeof(LearningObjective));
        public static readonly StudentGradeEntity ReportCard = new StudentGradeEntity(typeof(ReportCard));
        public static readonly StudentGradeEntity StudentCompetencyObjective = new StudentGradeEntity(typeof(StudentCompetencyObjective));
        public static readonly StudentGradeEntity StudentLearningObjective = new StudentGradeEntity(typeof(StudentLearningObjective));
    }
}
