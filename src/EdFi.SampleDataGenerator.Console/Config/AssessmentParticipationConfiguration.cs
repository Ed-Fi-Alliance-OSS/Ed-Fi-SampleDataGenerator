using System.Text.RegularExpressions;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class AssessmentParticipationConfiguration : IAssessmentParticipationConfiguration
    {
        private string _assessmentTitle;

        [XmlAttribute]
        public string AssessmentTitle
        {
            get { return _assessmentTitle; }
            set { _assessmentTitle = value; AssessmentTitleMatchExpression = new Regex(_assessmentTitle, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase); }
        }

        [XmlAttribute]
        public bool RegexMatch { get; set; }

        [XmlElement("ParticipationRate")]
        public AssessmentParticipationRate[] ParticipationRates { get; set; }
        IAssessmentParticipationRate[] IAssessmentParticipationConfiguration.ParticipationRates => ParticipationRates;

        public Regex AssessmentTitleMatchExpression { get; set; }
    }

    public class AssessmentParticipationRate : IAssessmentParticipationRate
    {
        [XmlAttribute]
        public double LowerPerformancePercentile { get; set; }
        [XmlAttribute]
        public double UpperPerformancePercentile { get; set; }
        [XmlAttribute]
        public double Probability { get; set; }
    }
}
