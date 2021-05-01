using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentCohortInterchangeName, typeof(InterchangeStudentCohort), typeof(ComplexObjectType))]
    public partial class StudentCohortData
    {
        public StudentCohortAssociation[] StudentCohortAssociations { get; set; } = new StudentCohortAssociation[0];
    }
}
