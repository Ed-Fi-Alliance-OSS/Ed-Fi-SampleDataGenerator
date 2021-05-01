using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Entities;
 
namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    public partial class StudentEnrollmentData
    {
        public List<EXTENSIONStudentTransportation> StudentTransportations { get; set; } = new List<EXTENSIONStudentTransportation>();
    }
}