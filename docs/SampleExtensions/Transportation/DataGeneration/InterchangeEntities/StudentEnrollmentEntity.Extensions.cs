using EdFi.SampleDataGenerator.Core.Entities;
namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public partial class StudentEnrollmentEntity
    {
        public static readonly StudentEnrollmentEntity StudentTransportation = new StudentEnrollmentEntity(typeof(EXTENSIONStudentTransportation));
    }
}