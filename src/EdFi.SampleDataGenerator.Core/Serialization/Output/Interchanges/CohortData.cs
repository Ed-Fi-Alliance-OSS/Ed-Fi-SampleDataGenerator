using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentCohortInterchangeName, typeof(InterchangeStudentCohort), typeof(ComplexObjectType))]
    public class CohortData
    {
        public List<Cohort> Cohorts { get; set; } = new List<Cohort>();
        public List<StaffCohortAssociation> StaffCohortAssociations { get; set; } = new List<StaffCohortAssociation>();
    }
}
