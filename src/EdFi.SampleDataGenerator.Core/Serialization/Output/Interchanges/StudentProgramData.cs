using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentProgramInterchangeName, typeof(InterchangeStudentProgram), typeof(ComplexObjectType))]
    public partial class StudentProgramData
    {
        public List<StudentProgramAssociation> StudentProgramAssociations { get; set; } = new List<StudentProgramAssociation>();
        public List<StudentSchoolFoodServiceProgramAssociation> StudentSchoolFoodServiceProgramAssociations { get; set; } = new List<StudentSchoolFoodServiceProgramAssociation>();
    }
}
