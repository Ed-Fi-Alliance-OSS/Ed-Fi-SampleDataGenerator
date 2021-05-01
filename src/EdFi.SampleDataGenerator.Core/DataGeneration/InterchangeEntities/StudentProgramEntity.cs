using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentProgramEntity : Entity
    {
        private StudentProgramEntity(Type entityType) : base(entityType, Interchange.StudentProgram)
        {
        }

        public static readonly StudentProgramEntity StudentProgramAssociation = new StudentProgramEntity(typeof(StudentProgramAssociation));
        public static readonly StudentProgramEntity RestraintEvent = new StudentProgramEntity(typeof(RestraintEvent));
        public static readonly StudentProgramEntity StudentCTEProgramAssociation = new StudentProgramEntity(typeof(StudentCTEProgramAssociation));
        public static readonly StudentProgramEntity StudentMigrantEducationProgramAssociation = new StudentProgramEntity(typeof(StudentMigrantEducationProgramAssociation));
        public static readonly StudentProgramEntity StudentSpecialEducationProgramAssociation = new StudentProgramEntity(typeof(StudentSpecialEducationProgramAssociation));
        public static readonly StudentProgramEntity StudentTitleIPartAProgramAssociation = new StudentProgramEntity(typeof(StudentTitleIPartAProgramAssociation));
        public static readonly StudentProgramEntity StudentSchoolFoodServiceProgramAssociation = new StudentProgramEntity(typeof(StudentSchoolFoodServiceProgramAssociation));
    }
}