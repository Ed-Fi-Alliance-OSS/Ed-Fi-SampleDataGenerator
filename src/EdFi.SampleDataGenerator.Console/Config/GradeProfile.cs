using System.Linq;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class GradeProfile : IGradeProfile
    {
        [XmlAttribute]
        public string GradeName { get; set;  }

        [XmlElement("StudentPopulationProfile")]
        public StudentPopulationProfile[] StudentPopulationProfiles { get; set; }
        IStudentPopulationProfile[] IGradeProfile.StudentPopulationProfiles => StudentPopulationProfiles;

        [XmlElement("AssessmentParticipationRate")]
        public AssessmentParticipationConfiguration[] AssessmentParticipationConfigurations { get; set; }
        IAssessmentParticipationConfiguration[] IGradeProfile.AssessmentParticipationConfigurations => AssessmentParticipationConfigurations;

        [XmlElement("GraduationPlan")]
        public GraduationPlanTemplateReference[] GraduationPlanTemplateReferences { get; set; }
        IGraduationPlanTemplateReference[] IGradeProfile.GraduationPlanTemplateReferences => GraduationPlanTemplateReferences;

        public int InitialStudentCount { get { return StudentPopulationProfiles.Sum(p => p.InitialStudentCount); } }
    }
}