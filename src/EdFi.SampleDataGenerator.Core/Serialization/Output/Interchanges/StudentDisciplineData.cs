using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentDisciplineInterchangeName, typeof(InterchangeStudentDiscipline), typeof(ComplexObjectType))]
    public partial class StudentDisciplineData
    {
        public List<DisciplineIncident> DisciplineIncidents { get; set; } = new List<DisciplineIncident>();
        public List<DisciplineAction> DisciplineActions { get; set; } = new List<DisciplineAction>();
        public List<StudentDisciplineIncidentAssociation> StudentDisciplineIncidentAssociations { get; set; } = new List<StudentDisciplineIncidentAssociation>();
    }
}